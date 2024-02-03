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
    /// ��ǰ�������
    /// </summary>
    public enum Age
    {
        /// <summary>
        /// �˺�ʱ��
        /// </summary>
        Canal,

        /// <summary>
        /// ��·ʱ��
        /// </summary>
        Railway
    }

    public class Map
    {
        /// <summary>
        /// ��ǰ��ͼ�ϵ�·����������key��˵��������·
        /// </summary>
        private IDictionary<(CityName A, CityName B), Road> _roads = new MapRoads();
        
        /// <summary>
        /// ��ǰ��ͼ�ϵĳ��У�һ�������key
        /// </summary>
        private Dictionary<CityName, (ICity City, Building[] Buildings)> _cities = new();

        /// <summary>
        /// ��ǰ��ͼ��ÿ����ҵĹ�ҵ��
        /// </summary>
        private Dictionary<PlayerEnum, HashSet<CityName>> _industrial = new ();

        /// <summary>
        /// ��ǰ����ʱ����
        /// </summary>
        public Age CurrentAge { get; private set; } = Age.Canal;

        public event MapChangingEvent OnMapChanged;

        public Map()
        {
            Init();
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            // ��յ�·
            _roads.Clear();

            // ��ʼ������
            _cities.Clear();
            foreach (var cityName in Cities.All)
            {
                ICity city = CityFactory.GetCity(cityName);
                _cities[cityName] = (city, new Building[city.GridCount]);
                OnMapChanged += city.Callback;
            }

            // �������һ���ֳ����ǽ�����������ĳ��У�����������Ҳ�в�ͬ�ĸ�����������������ʱ�Խ����������Ķ����������
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
            // ��ʼ����ҵ��
            _industrial.Clear();
            foreach(var player in System.Enum.GetValues(typeof(PlayerEnum)))
            {
                _industrial.Add((PlayerEnum)player, new HashSet<CityName>());
            }
        }

        /// <summary>
        /// ��ȡһ��������һ�����Ӵ�������Ϣ
        /// </summary>
        /// <param name="cityName">������</param>
        /// <param name="index">�����±�</param>
        /// <returns>������Ϣ�����������û�н������򷵻�null��</returns>
        public Building GetBuilding(CityName cityName, ushort index)
        {
            if (index >= _cities[cityName].Buildings.Length)
                return null;
            return _cities[cityName].Buildings[index];
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="player">���콨�������</param>
        /// <param name="type">��������</param>
        /// <param name="city">����ĳ���</param>
        /// <param name="index">����ĸ��ӵ��±�</param>
        /// <param name="tech">�����ĿƼ��������Ϣ</param>
        /// <param name="checkIndustrial">�Ƿ��鹤ҵ��</param>
        /// <param name="isCheckOnly">�Ƿ�����Ǽ������Զ���ʵ�ʽ���</param>
        /// <returns>�Ƿ�ɹ�����</returns>
        /// <exception cref="System.IndexOutOfRangeException">����ĸ��ӵ��±곬���˳�������֧�ֵĸ���</exception>
        public ProgramMessage CreateBuilding(PlayerEnum player, TechType type, CityName city, ushort index, IReadOnlyTechNode tech, bool checkIndustrial = false, bool isCheckOnly = false)
        {
            // ���û������ͨ���л������Ƴ����죬���ڽ��������죬��������
            if (Cities.IsMarket(city))
            {
                return ProgramMessage.Failure(-1, $"����{city}�������콨����");
            }

            // �����ǰ��������ʱ���뵱ǰʱ�������ϣ���׼����
            switch (CurrentAge)
            {
                case Age.Canal:
                    if(!tech.IsCanalAge)
                        return ProgramMessage.Failure(-1, "��ǰ�������������˺�ʱ�����졣");
                    break;
                case Age.Railway:
                    if(!tech.IsRailwayAge)
                        return ProgramMessage.Failure(-1, "��ǰ��������������·ʱ�����졣");
                    break;

                default: break;
            }

            // ����ó��н���ĸ��Ӵ���������ཨ���Ľ���
            if (!_cities[city].City.GetAvailables(index)[type])
                return ProgramMessage.Failure(-1, $"����{city}�ĵ�{index}�񴦲���������ཨ����");


            // ��ǰ���˺�ʱ������һ��������һ�����ֻ�ܽ���һ����������·ʱ������Ӱ��
            if (CurrentAge == Age.Canal)
            {
                Building[] buildings = _cities[city].Buildings;
                for (int i = 0; i < buildings.Length; i++)
                {
                    // ��ǰҪ����ĸ����±괦�Ѿ��н�����
                    if (i == index && buildings[i] != null)
                        return ProgramMessage.Failure(-1, "��ǰ�������н�����");
                    // ��ǰҪ����ĸ����±�֮�⣬������һ�����Ӳ����н��������ҽ�����Ҳ�Ǹ���ң���׼��
                    if (i != index && buildings[i] != null && buildings[i].Player == player)
                        return ProgramMessage.Failure(-1, "��ǰ���˺�ʱ����һ�������н�����һ����ͬһλ��ҽ���Ľ�����");
                }
            }


            // ��鹤ҵ�����
            if (checkIndustrial && _industrial[player].Count > 0)
            {
                // �����������Worcester���ľƳ�������������KidderMinster��Worcester֮�佨���·����ҡ�
                if (city == CityName.Worcester_Left_Wine)
                {
                    if (_roads.TryGetValue((CityName.KidderMinster, CityName.Worcester), out Road road) && road.Player != player)
                    {
                        return ProgramMessage.Failure(-1, "�˸�����������KidderMinster��Worcester֮�佨���·����ҡ�");
                    }
                }
                // ��ҵ���Ƿ��иý���ĳ���
                else if (!_industrial[player].Contains(city))
                    return ProgramMessage.Failure(-1, "��ǰ���в��ڹ�ҵ���С�");
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
        /// ����һ����·
        /// </summary>
        /// <param name="player">�����·�����</param>
        /// <param name="a">��·��ʼ����</param>
        /// <param name="b">��·��������</param>
        /// <param name="roadType">��·����</param>
        /// <param name="checkIndustrial">�Ƿ��鹤ҵ��</param>
        /// <param name="isCheckOnly">�Ƿ�����Ǽ������Զ���ʵ�ʽ���</param>
        /// <returns></returns>
        public ProgramMessage CreateRoad(PlayerEnum player, CityName a, CityName b, RoadType roadType, bool checkIndustrial = false, bool isCheckOnly = false)
        {
            // ����ܷ���������·
            if (CurrentAge == Age.Canal && roadType != RoadType.Canal)
                return ProgramMessage.Failure(-1, "��ǰ���˺�ʱ�������ܽ���ˮ·��");
            if (CurrentAge == Age.Railway && roadType != RoadType.Railway)
                return ProgramMessage.Failure(-1, "��ǰ����·ʱ�������ܽ�����·��");

            // �����������֮��ɷ�����������·
            ICity cityA = _cities[a].City;
            if (roadType == RoadType.Canal && !cityA.IsConnectedByCanal(b))
                return ProgramMessage.Failure(-1, $"����{a}�����{b}֮�䲻���Խ����˺ӡ�");
            else if(roadType == RoadType.Railway && !cityA.IsConnectedByRailway(b))
                return ProgramMessage.Failure(-1, $"����{a}�����{b}֮�䲻���Խ�����·��");

            // ���˴��Ƿ����������ĵ�·
            if(_roads.ContainsKey((a, b)))
                return ProgramMessage.Failure(-1, $"����{a}�����{b}֮�����е�·��");

            // ���õ��Ƿ����ڹ�ҵ����
            if (checkIndustrial && _industrial[player].Count > 0)
            {
               if (!_industrial[player].Contains(a) && !_industrial[player].Contains(b))
                    return ProgramMessage.Failure(-1, "��ǰ��·���빤ҵ��������");
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
        /// ��ø���ҵ�ǰ��ͼ��·�ͽ����ķ�����������·�ֺͽ�����
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public uint GetCurrentScore(PlayerEnum player)
        {
            uint ans = 0;
            
            // �����·��
            foreach(var pair in _roads)
            {
                if(pair.Value.Player != player)
                    continue;

                // ����A�����н����ĵ�·��
                foreach(var building in _cities[pair.Key.A].Buildings)
                {
                    if (building != null)
                        ans += building.Tech.RoadValue;
                }

                // ����B�����н����ĵ�·��
                foreach (var building in _cities[pair.Key.B].Buildings)
                {
                    if (building != null)
                        ans += building.Tech.RoadValue;
                }
            }

            // ���㽨����
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
        /// ��ø�����ڵ�ͼ�����н�������ľ������롣
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
        /// ���˺�ʱ��תΪ��·ʱ���������Ϊ��ʹ�õ�ͼ��һЩ�������Ƴ���
        /// </summary>
        /// <returns></returns>
        public ProgramMessage ToRailwayAge()
        {
            if(CurrentAge == Age.Railway)
                return ProgramMessage.Failure(-1, "��ǰʱ���Ѿ�����·ʱ����");


            // ���˽��������˺�ʱ��������ݻ�
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

            // �˺�ʱ��ֻ�����˺ӣ�����ֱ�ӽ�����·ȥ����
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