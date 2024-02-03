using Game.Player;
using Game.TechTree;

namespace Game.Map
{
    /// <summary>
    /// 地图改变时的委托
    /// </summary>
    /// <param name="map">发生变化的地图对象</param>
    /// <param name="context">变换回调上下文</param>
    public delegate void MapChangingEvent(Map map, MapChangeContext context);

    /// <summary>
    /// 每次地图改变时回调的上下文
    /// </summary>
    public class MapChangeContext
    {
        private ChangeTypeFlag _flag;

        /// <summary>
        /// 这次地图改变是否是增加路
        /// </summary>
        public bool IsAddRoad => (_flag & ChangeTypeFlag.Road) > 0 && (_flag & ChangeTypeFlag.Adding) > 0;
        /// <summary>
        /// 这次地图改变是否是减少路
        /// </summary>
        public bool IsRemoveRoad => (_flag & ChangeTypeFlag.Road) > 0 && (_flag & ChangeTypeFlag.Removing) > 0;
        /// <summary>
        /// 这次地图改变是否是增加建筑
        /// </summary>
        public bool IsAddBuilding => (_flag & ChangeTypeFlag.Building) > 0 && (_flag & ChangeTypeFlag.Adding) > 0;
        /// <summary>
        /// 这次地图改变是否是减少建筑
        /// </summary>
        public bool IsRemoveBuilding => (_flag & ChangeTypeFlag.Building) > 0 && (_flag & ChangeTypeFlag.Removing) > 0;

        /// <summary>
        /// 如果回调类型是增加/减少路，则该属性指的是增加/减少的路。
        /// </summary>
        public Road Road { get; private set; }

        /// <summary>
        /// 如果回调类型是增加/减少建筑，则该属性指的是增加/减少的建筑相关信息。
        /// </summary>
        public Building Building { get; private set; }

        /// <summary>
        /// 创建一个与道路有关的上下文
        /// </summary>
        /// <param name="road">产生变化的道路数据信息</param>
        /// <param name="isAdd">是否是增加道路</param>
        public MapChangeContext(Road road, bool isAdd)
        {
            _flag = ChangeTypeFlag.Road | (isAdd ? ChangeTypeFlag.Adding : ChangeTypeFlag.Removing);
            Road = road;
        }

        /// <summary>        
        /// 创建一个与建筑有关的上下文
        /// </summary>
        /// <param name="city">建造的城市</param>
        /// <param name="index">建造的格子坐标</param>
        /// <param name="building">建筑的科技树节点</param>
        /// <param name="buildingType">建筑类型</param>
        /// <param name="isAdd">是否是新建造道路</param>
        public MapChangeContext(Building building, bool isAdd)
        {
            _flag = ChangeTypeFlag.Building | (isAdd ? ChangeTypeFlag.Adding : ChangeTypeFlag.Removing);
            Building = building;
        }

        [System.Flags]
        enum ChangeTypeFlag
        {
            None = 0,
            Road = 1,
            Building = 2,
            Adding = 4,
            Removing = 8,
        }
    }
}
