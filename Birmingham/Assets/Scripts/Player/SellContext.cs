using Game.Map;
using Game.Tech;

namespace Game.Player
{
    /// <summary>
    /// ����������Ʒʱ�Ļص�������
    /// </summary>
    public class SellContext
    {
        /// <summary>
        /// ����ʱ����������Ľ�����Ϣ
        /// </summary>
        public Building Building { get; private set; }

        /// <summary>
        /// ������������ʱ���Ƶ��ṩ�ߡ����Ϊ�գ���᳢��ʹ��Ŀ�꽻��վ�ľơ�
        /// </summary>
        public Building WineProvider { get; private set; }

        /// <summary>
        /// ����ʱ���յ�Ľ���վ������
        /// </summary>
        public CityName Destination {  get; private set; }

        /// <summary>
        /// ����ʱ�����յ㽻��վ����һ������������
        /// </summary>
        public ushort Index { get; private set; }

        /// <summary>
        /// ��������Ʒ������
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