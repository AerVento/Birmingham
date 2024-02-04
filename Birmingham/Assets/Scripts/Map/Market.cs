using Game.Tech;
using System.Collections.Generic;

namespace Game.Map
{
    /// <summary>
    /// 交易所中，每一个格子可以售卖的类型。
    /// </summary>
    public enum MarketSellType
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty = -1, // 这里使用-1是让Cotton为0, 与TechType的Cotton对齐方便转换。

        /// <summary>
        /// 棉花
        /// </summary>
        Cotton,

        /// <summary>
        /// 陶瓷
        /// </summary>
        Ceramics,

        /// <summary>
        /// 板条箱
        /// </summary>
        Crate,

        /// <summary>
        /// 同时售卖以上三种
        /// </summary>
        Triple,
    }
    
    /// <summary>
    /// 当使用掉交易所的酒之后，玩家会享受到的奖励类型
    /// </summary>
    public enum MarketBonusType
    {
        /// <summary>
        /// 直接的金钱奖励
        /// </summary>
        Money,

        /// <summary>
        /// 得分奖励
        /// </summary>
        Score,

        /// <summary>
        /// 经济收益奖励
        /// </summary>
        Economy,

        /// <summary>
        /// 科研点奖励。一个科研点代表可以科研跳过一个建筑。
        /// </summary>
        Upgrade,
    }

    /// <summary>
    /// 地图上交易所的抽象类
    /// </summary>
    public abstract class Market : ICity
    {
        /// <summary>
        /// 地图上7个交易所格子的分配
        /// </summary>
        public static readonly MarketSellType[] Sells = { 
            MarketSellType.Empty, 
            MarketSellType.Empty, 
            MarketSellType.Empty,
            MarketSellType.Cotton,
            MarketSellType.Cotton, 
            MarketSellType.Ceramics,
            MarketSellType.Crate,
            MarketSellType.Crate, 
            MarketSellType.Triple 
        };
        
        protected MarketSellType[] _sells;
        protected bool[] _bonusAvailable;

        public Market()
        {
            _sells = new MarketSellType[SellCount];
            System.Array.Fill(_sells, MarketSellType.Empty);

            _bonusAvailable = new bool[SellCount];
            System.Array.Fill(_bonusAvailable, false);
        }

        #region Sell
        /// <summary>
        /// 交易所有几个格子可以售卖物品
        /// </summary>
        public abstract ushort SellCount { get; }

        /// <summary>
        /// 获取当前格子售卖的物品
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MarketSellType GetSelling(ushort index)
        {
            return _sells[index];
        }

        /// <summary>
        /// 设置当前格子售卖的物品
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetSelling(ushort index, MarketSellType value)
        {
            _sells[index] = value;
        }
        #endregion

        #region Bonus

        /// <summary>
        /// 当使用掉交易所的酒之后，玩家会享受到的奖励类型
        /// </summary>
        public abstract MarketBonusType BonusType { get; }

        /// <summary>
        /// 当使用掉交易所的酒之后，玩家会享受到的奖励数值。根据类型不同，会代表不同的意思。
        /// </summary>
        public abstract ushort BonusValue { get; }

        /// <summary>
        /// 交易所当前下标处是否还有奖励（是否有酒）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsBonusAvailable(ushort index)
        {
            return _bonusAvailable[index];
        }

        /// <summary>
        /// 设置交易所当前下标处是否还有奖励（是否有酒）
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetBonusAvailable(ushort index, bool value)
        {
            _bonusAvailable[index] = value;
        }
        #endregion


        #region ICity Implements
        public abstract CityName Name { get; }

        public ushort GridCount => 0; // 交易所肯定没有格子来建造建筑

        public abstract MapChangingEvent Callback { get; }
        public abstract IEnumerable<CityName> Neighbours { get; }

        public TechTypeVector<bool> GetAvailables(ushort index) // 交易所既然没有格子，index无论取何值都会越界
        {
            throw new System.IndexOutOfRangeException($"The market {Name} has no grid for building.");
        }

        public abstract bool IsConnectedByCanal(CityName other);
        public abstract bool IsConnectedByRailway(CityName other);
        #endregion
    }

    public static class MarketUtils
    {
        /// <summary>
        /// 交易所格子售卖的类型是否包含此科技类型。
        /// </summary>
        /// <param name="sellType">交易所格子售卖类型</param>
        /// <param name="techType">科技类型</param>
        /// <returns></returns>
        public static bool IsSelling(this MarketSellType sellType, TechType techType)
        {
            switch (sellType)
            {
                case MarketSellType.Cotton:
                case MarketSellType.Ceramics:
                case MarketSellType.Crate:
                    return (int)techType == (int)sellType;
                case MarketSellType.Triple:
                    return techType == TechType.Cotton || techType == TechType.Ceramics || techType == TechType.Crate;
                
                case MarketSellType.Empty:
                default:
                    return false;
            }
        }
    }
}