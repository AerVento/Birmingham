using Game.Tech;
using System.Collections.Generic;

namespace Game.Map
{
    /// <summary>
    /// �������У�ÿһ�����ӿ������������͡�
    /// </summary>
    public enum MarketSellType
    {
        /// <summary>
        /// ��
        /// </summary>
        Empty = -1, // ����ʹ��-1����CottonΪ0, ��TechType��Cotton���뷽��ת����

        /// <summary>
        /// �޻�
        /// </summary>
        Cotton,

        /// <summary>
        /// �մ�
        /// </summary>
        Ceramics,

        /// <summary>
        /// ������
        /// </summary>
        Crate,

        /// <summary>
        /// ͬʱ������������
        /// </summary>
        Triple,
    }
    
    /// <summary>
    /// ��ʹ�õ��������ľ�֮����һ����ܵ��Ľ�������
    /// </summary>
    public enum MarketBonusType
    {
        /// <summary>
        /// ֱ�ӵĽ�Ǯ����
        /// </summary>
        Money,

        /// <summary>
        /// �÷ֽ���
        /// </summary>
        Score,

        /// <summary>
        /// �������潱��
        /// </summary>
        Economy,

        /// <summary>
        /// ���е㽱����һ�����е������Կ�������һ��������
        /// </summary>
        Upgrade,
    }

    /// <summary>
    /// ��ͼ�Ͻ������ĳ�����
    /// </summary>
    public abstract class Market : ICity
    {
        /// <summary>
        /// ��ͼ��7�����������ӵķ���
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
        /// �������м������ӿ���������Ʒ
        /// </summary>
        public abstract ushort SellCount { get; }

        /// <summary>
        /// ��ȡ��ǰ������������Ʒ
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MarketSellType GetSelling(ushort index)
        {
            return _sells[index];
        }

        /// <summary>
        /// ���õ�ǰ������������Ʒ
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
        /// ��ʹ�õ��������ľ�֮����һ����ܵ��Ľ�������
        /// </summary>
        public abstract MarketBonusType BonusType { get; }

        /// <summary>
        /// ��ʹ�õ��������ľ�֮����һ����ܵ��Ľ�����ֵ���������Ͳ�ͬ�������ͬ����˼��
        /// </summary>
        public abstract ushort BonusValue { get; }

        /// <summary>
        /// ��������ǰ�±괦�Ƿ��н������Ƿ��оƣ�
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsBonusAvailable(ushort index)
        {
            return _bonusAvailable[index];
        }

        /// <summary>
        /// ���ý�������ǰ�±괦�Ƿ��н������Ƿ��оƣ�
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

        public ushort GridCount => 0; // �������϶�û�и��������콨��

        public abstract MapChangingEvent Callback { get; }
        public abstract IEnumerable<CityName> Neighbours { get; }

        public TechTypeVector<bool> GetAvailables(ushort index) // ��������Ȼû�и��ӣ�index����ȡ��ֵ����Խ��
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
        /// ���������������������Ƿ�����˿Ƽ����͡�
        /// </summary>
        /// <param name="sellType">������������������</param>
        /// <param name="techType">�Ƽ�����</param>
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