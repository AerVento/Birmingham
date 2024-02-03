using Game.Player;

namespace Game.Map
{
    /// <summary>
    /// 道路类型
    /// </summary>
    public enum RoadType
    {
        /// <summary>
        /// 运河
        /// </summary>
        Canal,
        
        /// <summary>
        /// 铁路
        /// </summary>
        Railway
    }
    /// <summary>
    /// 游戏中的道路
    /// </summary>
    public class Road
    {
        /// <summary>
        /// 路的起始城市
        /// </summary>
        public CityName A { get; private set; }

        /// <summary>
        /// 路的终止城市
        /// </summary>
        public CityName B { get; private set; }

        /// <summary>
        /// 路所归属的玩家
        /// </summary>
        public PlayerEnum Player { get; private set; }

        /// <summary>
        /// 道路类型
        /// </summary>
        public RoadType RoadType { get; private set; }

        /// <summary>
        /// 创建一条路的信息
        /// </summary>
        /// <param name="a">路的起始城市</param>
        /// <param name="b">路的终止城市</param>
        /// <param name="player">路所归属的玩家</param>
        /// <param name="roadType">道路类型</param>
        public Road(CityName a, CityName b, PlayerEnum player, RoadType roadType)
        {
            A = a;
            B = b;
            Player = player;
            RoadType = roadType;
        }

        /// <summary>
        /// 两个路信息是否是同一个位置上
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool IsSamePosition(Road other) => IsSamePosition(this, other);
        
        /// <summary>
        /// 两个路信息是否是同一个位置上
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool IsSamePosition(Road a, Road b)
        {
            return (a.A == b.A && a.B == b.B) || (a.A == b.B && a.B == b.A);
        }
    }
}