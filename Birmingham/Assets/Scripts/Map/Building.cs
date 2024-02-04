using Game.Player;
using Game.Tech;

namespace Game.Map
{
    /// <summary>
    /// 游戏中的建筑
    /// </summary>
    public class Building
    {
        /// <summary>
        /// 建筑类型
        /// </summary>
        public TechType Type { get; private set; }

        /// <summary>
        /// 在哪座城市上建造
        /// </summary>
        public CityName City { get; private set; }

        /// <summary>
        /// 在这座城市的第几个下标的格子处建造
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 该建筑的等级等科技树相关信息
        /// </summary>
        public IReadOnlyTechNode Tech { get; private set; }

        /// <summary>
        /// 该建筑所属哪个玩家
        /// </summary>
        public PlayerEnum Player { get; private set; }

        /// <summary>
        /// 该建筑产生收益所需要的物品。根据科技类型不同，这个字段代表的意思也不同。
        /// </summary>
        public TechTypeVector<ushort> ItemNeeds { get; private set; }

        /// <summary>
        /// 该建筑是否正在产生收益
        /// </summary>
        public bool IsProfiting
        {
            get
            {
                foreach(var num in ItemNeeds)
                {
                    if (num.Value != 0)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 创建一个建筑信息
        /// </summary>
        /// <param name="type">建筑类型</param>
        /// <param name="city">在哪座城市上建造</param>
        /// <param name="index">在这座城市的第几个下标的格子处建造</param>
        /// <param name="tech">该建筑的等级等科技树相关信息</param>
        /// <param name="player">该建筑所属哪个玩家</param>
        public Building(TechType type, CityName city, int index, IReadOnlyTechNode tech, PlayerEnum player)
        {
            Type = type;
            City = city;
            Index = index;
            Tech = tech;
            Player = player;
            ItemNeeds = tech.ItemNeeds.Clone();
        }

        /// <summary>
        /// 两个建筑是否在同一位置上
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool IsSamePosition(Building other) => IsSamePosition(this, other);

        /// <summary>
        /// 两个建筑是否在同一位置上
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSamePosition(Building a, Building b)
        {
            return a.City == b.City && a.Index == b.Index;
        }
    }
}