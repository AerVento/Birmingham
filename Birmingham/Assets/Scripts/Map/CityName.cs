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
    /// �ṩ��һЩ����ķ�������CityName���Ϸ��ʡ�
    /// </summary>
    public static class Cities
    {
        /// <summary>
        /// ���г���
        /// </summary>
        public static IEnumerable<CityName> All => (IEnumerable<CityName>)System.Enum.GetValues(typeof(CityName));

        /// <summary>
        /// ��ͨ���У������ֵĳ������и��ӿ��Խ��콨���ĳ���
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
        /// �ó����Ƿ�����ͨ����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsCommon(CityName name) => Common.Contains(name);

        /// <summary>
        /// ������������������Ʒ�ĳ���
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
        /// �ó����Ƿ��ǽ�����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsMarket(CityName name) => Markets.Contains(name);

        /// <summary>
        /// �����Ƴ�����
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
        /// �ó����Ƿ��������Ƴ�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsNameless(CityName name) => Nameless.Contains(name);
    }
}