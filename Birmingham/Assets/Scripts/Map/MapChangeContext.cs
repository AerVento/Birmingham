using Game.Player;
using Game.TechTree;

namespace Game.Map
{
    /// <summary>
    /// ��ͼ�ı�ʱ��ί��
    /// </summary>
    /// <param name="map">�����仯�ĵ�ͼ����</param>
    /// <param name="context">�任�ص�������</param>
    public delegate void MapChangingEvent(Map map, MapChangeContext context);

    /// <summary>
    /// ÿ�ε�ͼ�ı�ʱ�ص���������
    /// </summary>
    public class MapChangeContext
    {
        private ChangeTypeFlag _flag;

        /// <summary>
        /// ��ε�ͼ�ı��Ƿ�������·
        /// </summary>
        public bool IsAddRoad => (_flag & ChangeTypeFlag.Road) > 0 && (_flag & ChangeTypeFlag.Adding) > 0;
        /// <summary>
        /// ��ε�ͼ�ı��Ƿ��Ǽ���·
        /// </summary>
        public bool IsRemoveRoad => (_flag & ChangeTypeFlag.Road) > 0 && (_flag & ChangeTypeFlag.Removing) > 0;
        /// <summary>
        /// ��ε�ͼ�ı��Ƿ������ӽ���
        /// </summary>
        public bool IsAddBuilding => (_flag & ChangeTypeFlag.Building) > 0 && (_flag & ChangeTypeFlag.Adding) > 0;
        /// <summary>
        /// ��ε�ͼ�ı��Ƿ��Ǽ��ٽ���
        /// </summary>
        public bool IsRemoveBuilding => (_flag & ChangeTypeFlag.Building) > 0 && (_flag & ChangeTypeFlag.Removing) > 0;

        /// <summary>
        /// ����ص�����������/����·���������ָ��������/���ٵ�·��
        /// </summary>
        public Road Road { get; private set; }

        /// <summary>
        /// ����ص�����������/���ٽ������������ָ��������/���ٵĽ��������Ϣ��
        /// </summary>
        public Building Building { get; private set; }

        /// <summary>
        /// ����һ�����·�йص�������
        /// </summary>
        /// <param name="road">�����仯�ĵ�·������Ϣ</param>
        /// <param name="isAdd">�Ƿ������ӵ�·</param>
        public MapChangeContext(Road road, bool isAdd)
        {
            _flag = ChangeTypeFlag.Road | (isAdd ? ChangeTypeFlag.Adding : ChangeTypeFlag.Removing);
            Road = road;
        }

        /// <summary>        
        /// ����һ���뽨���йص�������
        /// </summary>
        /// <param name="city">����ĳ���</param>
        /// <param name="index">����ĸ�������</param>
        /// <param name="building">�����ĿƼ����ڵ�</param>
        /// <param name="buildingType">��������</param>
        /// <param name="isAdd">�Ƿ����½����·</param>
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
