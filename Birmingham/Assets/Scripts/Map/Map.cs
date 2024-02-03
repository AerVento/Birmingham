using Framework.Message;
using Game.Player;
using Game.TechTree;
using Game.Util;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Map
{
    /// <summary>
    /// 当前所处年代
    /// </summary>
    public enum Age
    {
        /// <summary>
        /// 运河时代
        /// </summary>
        Canal,

        /// <summary>
        /// 铁路时代
        /// </summary>
        Railway
    }

    public class Map
    {
        /// <summary>
        /// 当前地图上的路，如果有这个key就说明有这条路
        /// </summary>
        private IDictionary<(CityName A, CityName B), Road> _roads = new MapRoads();
        
        /// <summary>
        /// 当前地图上的城市，一定有这个key
        /// </summary>
        private Dictionary<CityName, (ICity City, Building[] Buildings)> _cities = new();

        /// <summary>
        /// 当前地图上每个玩家的工业区
        /// </summary>
        private Dictionary<PlayerEnum, HashSet<CityName>> _industrial = new ();

        /// <summary>
        /// 当前所处时代。
        /// </summary>
        public Age CurrentAge { get; private set; } = Age.Canal;

        public event MapChangingEvent OnMapChanged;

        public Map()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            // 清空道路
            _roads.Clear();

            // 初始化城市
            _cities.Clear();
            foreach (var cityName in Cities.All)
            {
                ICity city = CityFactory.GetCity(cityName);
                _cities[cityName] = (city, new Building[city.GridCount]);
                OnMapChanged += city.Callback;
            }

            // 这里，由于一部分城市是交易所（特殊的城市），交易所中也有不同的格子用作卖东西，此时对交易所售卖的东西随机生成
            var sells = Market.Sells.Shuffle();
            var enumerator = sells.GetEnumerator();
            int sellsCount = sells.Count();
            int gridCount = 0;
            foreach (var cityName in Cities.Markets)
            {
                Market market = _cities[cityName].City as Market;
                ushort i = 0;
                while(enumerator.MoveNext())
                {
                    market.SetSelling(i, enumerator.Current);
                    gridCount++;
                    i++;
                }
            }
            if (gridCount < sellsCount)
                Debug.LogWarning($"The market grids count({gridCount}) is lesser than the count of IEnumerable object Market.Sells({sellsCount}). The extra element in IEnumerable Object will be ignored.");
            else if(gridCount > sellsCount)
                Debug.LogWarning($"The market grids count({gridCount}) is greater than the count of IEnumerable object Market.Sells({sellsCount}). The selling item of extra grids will be set to MarketSellType.Empty.");
            // 初始化工业区
            _industrial.Clear();
            foreach(var player in System.Enum.GetValues(typeof(PlayerEnum)))
            {
                _industrial.Add((PlayerEnum)player, new HashSet<CityName>());
            }
        }

        /// <summary>
        /// 获取一个城市中一个格子处建筑信息
        /// </summary>
        /// <param name="cityName">城市名</param>
        /// <param name="index">格子下标</param>
        /// <returns>建筑信息。如果格子上没有建筑，则返回null。</returns>
        public Building GetBuilding(CityName cityName, ushort index)
        {
            if (index >= _cities[cityName].Buildings.Length)
                return null;
            return _cities[cityName].Buildings[index];
        }

        /// <summary>
        /// 建造一个建筑
        /// </summary>
        /// <param name="player">建造建筑的玩家</param>
        /// <param name="type">建筑类型</param>
        /// <param name="city">建造的城市</param>
        /// <param name="index">建造的格子的下标</param>
        /// <param name="tech">建筑的科技树相关信息</param>
        /// <param name="checkIndustrial">是否检查工业区</param>
        /// <param name="isCheckOnly">是否仅仅是检查可行性而不实际建造</param>
        /// <returns>是否成功建造</returns>
        /// <exception cref="System.IndexOutOfRangeException">建造的格子的下标超出了城市所能支持的格子</exception>
        public ProgramMessage CreateBuilding(PlayerEnum player, TechType type, CityName city, ushort index, IReadOnlyTechNode tech, bool checkIndustrial = false, bool isCheckOnly = false)
        {
            // 如果没有在普通城市或无名酒厂建造，而在交易所建造，则不允许建造
            if (Cities.IsMarket(city))
            {
                return ProgramMessage.Failure(-1, $"城市{city}不允许建造建筑。");
            }

            // 如果当前建筑所需时代与当前时代不符合，则不准建造
            switch (CurrentAge)
            {
                case Age.Canal:
                    if(!tech.IsCanalAge)
                        return ProgramMessage.Failure(-1, "当前建筑不允许在运河时代建造。");
                    break;
                case Age.Railway:
                    if(!tech.IsRailwayAge)
                        return ProgramMessage.Failure(-1, "当前建筑不允许在铁路时代建造。");
                    break;

                default: break;
            }

            // 如果该城市建造的格子处不允许此类建筑的建造
            if (!_cities[city].City.GetAvailables(index)[type])
                return ProgramMessage.Failure(-1, $"城市{city}的第{index}格处不允许建造此类建筑。");


            // 当前是运河时代，则一个城市里一个玩家只能建造一个建筑，铁路时代则不受影响
            if (CurrentAge == Age.Canal)
            {
                Building[] buildings = _cities[city].Buildings;
                for (int i = 0; i < buildings.Length; i++)
                {
                    // 当前要建造的格子下标处已经有建筑了
                    if (i == index && buildings[i] != null)
                        return ProgramMessage.Failure(-1, "当前格子已有建筑！");
                    // 当前要建造的格子下标之外，还存在一个格子不仅有建筑，而且建造者也是该玩家，则不准建
                    if (i != index && buildings[i] != null && buildings[i].Player == player)
                        return ProgramMessage.Failure(-1, "当前是运河时代，一座城市中仅能有一个由同一位玩家建造的建筑。");
                }
            }


            // 检查工业区情况
            if (checkIndustrial && _industrial[player].Count > 0)
            {
                // 特殊情况处理：Worcester左侧的酒厂格子隶属于在KidderMinster和Worcester之间建造道路的玩家。
                if (city == CityName.Worcester_Left_Wine)
                {
                    if (_roads.TryGetValue((CityName.KidderMinster, CityName.Worcester), out Road road) && road.Player != player)
                    {
                        return ProgramMessage.Failure(-1, "此格子隶属于在KidderMinster和Worcester之间建造道路的玩家。");
                    }
                }
                // 工业区是否含有该建造的城市
                else if (!_industrial[player].Contains(city))
                    return ProgramMessage.Failure(-1, "当前城市不在工业区中。");
            }


            if (!isCheckOnly)
            {
                if (index < 0 || index >= _cities[city].City.GridCount)
                {
                    throw new System.IndexOutOfRangeException($"The index {index} out of range. It should be in [0, {_cities[city].City.GridCount}) .");
                }
                Building building = new Building(type, city, index, tech, player);
                _cities[city].Buildings[index] = building;
                _industrial[player].Add(city);
                OnMapChanged.Invoke(this, new MapChangeContext(building, isAdd: true));
            }
            return ProgramMessage.Success();
        }

        /// <summary>
        /// 建造一条道路
        /// </summary>
        /// <param name="player">建造道路的玩家</param>
        /// <param name="a">道路起始城市</param>
        /// <param name="b">道路结束城市</param>
        /// <param name="roadType">道路类型</param>
        /// <param name="checkIndustrial">是否检查工业区</param>
        /// <param name="isCheckOnly">是否仅仅是检查可行性而不实际建造</param>
        /// <returns></returns>
        public ProgramMessage CreateRoad(PlayerEnum player, CityName a, CityName b, RoadType roadType, bool checkIndustrial = false, bool isCheckOnly = false)
        {
            // 检查能否修这样的路
            if (CurrentAge == Age.Canal && roadType != RoadType.Canal)
                return ProgramMessage.Failure(-1, "当前是运河时代，仅能建造水路。");
            if (CurrentAge == Age.Railway && roadType != RoadType.Railway)
                return ProgramMessage.Failure(-1, "当前是铁路时代，仅能建造铁路。");

            // 检查两座城市之间可否建造这样的铁路
            ICity cityA = _cities[a].City;
            if (roadType == RoadType.Canal && !cityA.IsConnectedByCanal(b))
                return ProgramMessage.Failure(-1, $"城市{a}与城市{b}之间不可以建造运河。");
            else if(roadType == RoadType.Railway && !cityA.IsConnectedByRailway(b))
                return ProgramMessage.Failure(-1, $"城市{a}与城市{b}之间不可以建造铁路。");

            // 检查此处是否已有这样的道路
            if(_roads.ContainsKey((a, b)))
                return ProgramMessage.Failure(-1, $"城市{a}与城市{b}之间已有道路。");

            // 检查该地是否属于工业区中
            if (checkIndustrial && _industrial[player].Count > 0)
            {
               if (!_industrial[player].Contains(a) && !_industrial[player].Contains(b))
                    return ProgramMessage.Failure(-1, "当前道路不与工业区相连。");
            }

            if (!isCheckOnly)
            {
                Road road = new Road(a, b, player, roadType);
                _roads.Add((a, b), road);

                _industrial[player].Add(a);
                _industrial[player].Add(b);
                OnMapChanged.Invoke(this, new MapChangeContext(road, isAdd: true));
            }
            return ProgramMessage.Success();
        }

        /// <summary>
        /// 获得该玩家当前地图上路和建筑的分数，包含道路分和建筑分
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public uint GetCurrentScore(PlayerEnum player)
        {
            uint ans = 0;
            
            // 计算道路分
            foreach(var pair in _roads)
            {
                if(pair.Value.Player != player)
                    continue;

                // 城市A中所有建筑的道路分
                foreach(var building in _cities[pair.Key.A].Buildings)
                {
                    if (building != null)
                        ans += building.Tech.RoadValue;
                }

                // 城市B中所有建筑的道路分
                foreach (var building in _cities[pair.Key.B].Buildings)
                {
                    if (building != null)
                        ans += building.Tech.RoadValue;
                }
            }

            // 计算建筑分
            foreach (var city in _cities.Values)
            {
                foreach(var building in city.Buildings)
                {
                    if (building != null && building.Player == player && building.IsProfiting)
                        ans += building.Tech.Score;
                }
            }

            return ans;
        }

        /// <summary>
        /// 获得该玩家在地图上所有建筑创造的经济收入。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public int GetCurrentIncome(PlayerEnum player)
        {
            int ans = 0;
            foreach (var city in _cities.Values)
            {
                foreach (var building in city.Buildings)
                {
                    if (building != null && building.Player == player && building.IsProfiting)
                        ans += building.Tech.EconomyValue;
                }
            }
            return ans;
        }

        /// <summary>
        /// 将运河时代转为铁路时代。这个行为会使得地图上一些建筑被移除。
        /// </summary>
        /// <returns></returns>
        public ProgramMessage ToRailwayAge()
        {
            if(CurrentAge == Age.Railway)
                return ProgramMessage.Failure(-1, "当前时代已经是铁路时代。");


            // 若此建筑仅限运河时代，则将其摧毁
            foreach (var city in _cities.Values)
            {
                for(int i = 0; i < city.Buildings.Length; i++)
                {
                    var building = city.Buildings[i];
                    if (building != null && building.Tech.IsCanalAge)
                    {
                        city.Buildings[i] = null;
                        OnMapChanged.Invoke(this, new MapChangeContext(building, isAdd: false));
                    }
                }
            }

            // 运河时代只能修运河，所以直接将所有路去除。
            foreach(var road in _roads.Values)
            {
                OnMapChanged.Invoke(this, new MapChangeContext(road, isAdd: false));
            }
            _roads.Clear();

            CurrentAge = Age.Railway;

            return ProgramMessage.Success();
        }
    }
}