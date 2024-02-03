using Game.TechTree;

namespace Game.Map
{
    /// <summary>
    /// 地图上城市的抽象接口
    /// </summary>
    public interface ICity
    {
        /// <summary>
        /// 城市名字
        /// </summary>
        public CityName Name { get; }

        /// <summary>
        /// 该城市有几个格子可以建造建筑
        /// </summary>
        public ushort GridCount { get; }

        /// <summary>
        /// 获取当前城市指定下标处格子的能建造哪些建筑
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>一个向量，如果为true则代表此格子可以建造该科技类型的建筑</returns>
        public TechTypeVector<bool> GetAvailables(ushort index);

        /// <summary>
        /// 地图发生变化时的回调
        /// </summary>
        public MapChangingEvent Callback { get; }

        /// <summary>
        /// 与另一个城市之间是否可以通过修建运河的方式使其相连
        /// </summary>
        /// <param name="other">另一个城市</param>
        public bool IsConnectedByCanal(CityName other);

        /// <summary>
        /// 与另一个城市之间是否可以通过修建铁路的方式使其相连
        /// </summary>
        /// <param name="other">另一个城市</param>
        public bool IsConnectedByRailway(CityName other);
    }

    public static class CityFactory
    {
        public static ICity GetCity(CityName name)
        {
            throw new System.NotImplementedException();
        }    
    }
}