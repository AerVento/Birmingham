using Game.Player;
using Game.Tech;

namespace Game.Map
{
    /// <summary>
    /// ��Ϸ�еĽ���
    /// </summary>
    public class Building
    {
        /// <summary>
        /// ��������
        /// </summary>
        public TechType Type { get; private set; }

        /// <summary>
        /// �����������Ͻ���
        /// </summary>
        public CityName City { get; private set; }

        /// <summary>
        /// ���������еĵڼ����±�ĸ��Ӵ�����
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// �ý����ĵȼ��ȿƼ��������Ϣ
        /// </summary>
        public IReadOnlyTechNode Tech { get; private set; }

        /// <summary>
        /// �ý��������ĸ����
        /// </summary>
        public PlayerEnum Player { get; private set; }

        /// <summary>
        /// �ý���������������Ҫ����Ʒ�����ݿƼ����Ͳ�ͬ������ֶδ������˼Ҳ��ͬ��
        /// </summary>
        public TechTypeVector<ushort> ItemNeeds { get; private set; }

        /// <summary>
        /// �ý����Ƿ����ڲ�������
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
        /// ����һ��������Ϣ
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="city">�����������Ͻ���</param>
        /// <param name="index">���������еĵڼ����±�ĸ��Ӵ�����</param>
        /// <param name="tech">�ý����ĵȼ��ȿƼ��������Ϣ</param>
        /// <param name="player">�ý��������ĸ����</param>
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
        /// ���������Ƿ���ͬһλ����
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool IsSamePosition(Building other) => IsSamePosition(this, other);

        /// <summary>
        /// ���������Ƿ���ͬһλ����
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