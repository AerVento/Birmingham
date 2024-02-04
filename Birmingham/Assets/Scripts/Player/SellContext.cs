using Game.Map;
using Game.Tech;

namespace Game.Player
{
    /// <summary>
    /// 售卖经济商品时的回调上下文
    /// </summary>
    public class SellContext
    {
        /// <summary>
        /// 售卖时，售卖货物的建筑信息
        /// </summary>
        public Building Building { get; private set; }

        /// <summary>
        /// 售卖经济作物时，酒的提供者。如果为空，则会尝试使用目标交易站的酒。
        /// </summary>
        public Building WineProvider { get; private set; }

        /// <summary>
        /// 售卖时，终点的交易站的名字
        /// </summary>
        public CityName Destination {  get; private set; }

        /// <summary>
        /// 售卖时，向终点交易站的哪一个格子售卖。
        /// </summary>
        public ushort Index { get; private set; }

        /// <summary>
        /// 售卖的物品的数量
        /// </summary>
        public ushort Count { get; private set; }

        public SellContext(Building building, CityName destination, ushort index, ushort sellCount)
        {
            Building = building;
            Destination = destination;
            Index = index;
            Count = sellCount;
        }
    }
}