using System.Collections.Generic;
using System.Linq;

namespace Game.Map
{
    public enum CityName
    {
        // Normal citys
        Stone = 0,
        Stoke_On_Trent,
        Leek,
        Belper,
        Uttoxeter,
        Derby,
        Stafford,
        Burton_On_Trent,
        Cannock,
        CoalBrookDale,
        WolverHampton,
        Walsall,
        Tamworth,
        Dudley,
        Birmingham,
        Nuneaton,
        Coventry,
        KidderMinster,
        Worcester,
        Redditch,

        // Markets
        Gloucester,
        Shrewsbury,
        Warrington,
        Nottingham,
        Oxford,

        // Nameless wine factory
        Cannock_Left_Wine,
        Worcester_Left_Wine,
    }

    /// <summary>
    /// 提供了一些方便的方法来对CityName集合访问。
    /// </summary>
    public static class Cities
    {
        /// <summary>
        /// 所有城市
        /// </summary>
        public static IEnumerable<CityName> All => (IEnumerable<CityName>)System.Enum.GetValues(typeof(CityName));

        /// <summary>
        /// 普通城市：有名字的城市且有格子可以建造建筑的城市
        /// </summary>
        public static IEnumerable<CityName> Common
        {
            get
            {
                for (int i = (int)CityName.Stone; i < (int)CityName.Redditch; i++)
                {
                    yield return (CityName)i;
                }
            }
        }

        /// <summary>
        /// 该城市是否是普通城市
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsCommon(CityName name) => Common.Contains(name);

        /// <summary>
        /// 交易所：用作交易商品的城市
        /// </summary>
        public static IEnumerable<CityName> Markets
        {
            get
            {
                for (int i = (int)CityName.Gloucester; i < (int)CityName.Oxford; i++)
                {
                    yield return (CityName)i;
                }
            }
        }

        /// <summary>
        /// 该城市是否是交易所
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsMarket(CityName name) => Markets.Contains(name);

        /// <summary>
        /// 无名酒厂格子
        /// </summary>
        public static IEnumerable<CityName> Nameless
        {
            get
            {
                for (int i = (int)CityName.Cannock_Left_Wine; i < (int)CityName.Worcester_Left_Wine; i++)
                {
                    yield return (CityName)i;
                }
            }
        }

        /// <summary>
        /// 该城市是否是无名酒厂
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsNameless(CityName name) => Nameless.Contains(name);
    }
}