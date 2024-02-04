using Game.Map;
using Game.Tech;
using System.Collections.Generic;

namespace Game.Player
{
    /// <summary>
    /// ��Ҳ���ö��
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// ��ҽ����˽���
        /// </summary>
        CreateBuilding,

        /// <summary>
        /// ��ҽ�����һ��·
        /// </summary>
        CreateRoad,

        /// <summary>
        /// ��ҽ�����һ���з�
        /// </summary>
        Upgrade,

        /// <summary>
        /// ��ҽ�����һ������
        /// </summary>
        Sell,

        /// <summary>
        /// ��ҽ����˴���
        /// </summary>
        Loan,

        /// <summary>
        /// ��ҽ��������
        /// </summary>
        Investigate,
    }

    /// <summary>
    /// ���һ�λغϲ�����ί�к���
    /// </summary>
    /// <param name="player">��Ҷ���</param>
    /// <param name="context">��������Ϣ</param>
    public delegate void PlayerOperationEvent(GamePlayer player, PlayerOperationContext context);

    /// <summary>
    /// ����ҽ�����һ�λغϲ���ʱ�����ڻص��������ġ�
    /// </summary>
    public class PlayerOperationContext
    { 
        /// <summary>
        /// ������һ����ҽ����˲�����
        /// </summary>
        public PlayerEnum Player { get; private set; }

        /// <summary>
        /// ��Ҵ˻غϽ��еĲ���
        /// </summary>
        public Operation Operation { get; private set; }

        /// <summary>
        /// �����Ҵ˻غϽ����˽��콨����������Դ������ҽ���Ľ�����Ϣ��
        /// </summary>
        public Building Building { get; private set; }

        /// <summary>
        /// �����Ҵ˻غϽ����˽����·��������Դ������ҽ���ĵ�·��Ϣ��
        /// </summary>
        public Road Road { get; private set; }

        /// <summary>
        /// �����Ҵ˻غϽ������з���������Դ����������з��ĿƼ������Լ�������
        /// </summary>
        public List<(IReadOnlyTechNode Tech, ushort Count)> Techs { get; private set; } 

        /// <summary>
        /// �����Ҵ˻غϽ���������������������Դ����������е�����������List�е�һ��Ԫ�ش���һ�������ϵ���Ʒ����������������
        /// </summary>
        public List<SellContext> SellContexts { get; private set; }

        /// <summary>
        /// ��ͨ�Ĺ��캯����ֻ�����һ��������
        /// </summary>
        /// <param name="player"></param>
        /// <param name="operation"></param>
        public PlayerOperationContext(PlayerEnum player, Operation operation)
        {
            Player = player;
            Operation = operation;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="player"></param>
        /// <param name="building"></param>
        public PlayerOperationContext(PlayerEnum player, Building building) : this(player, Operation.CreateBuilding)
        {
            Building = building;
        }

        /// <summary>
        /// ������·����
        /// </summary>
        /// <param name="player"></param>
        /// <param name="road"></param>
        public PlayerOperationContext(PlayerEnum player, Road road) : this(player, Operation.CreateRoad)
        {
            Road = road;
        }

        /// <summary>
        /// ���в���
        /// </summary>
        /// <param name="player"></param>
        /// <param name="techs"></param>
        public PlayerOperationContext(PlayerEnum player, List<(IReadOnlyTechNode Tech, ushort Count)> techs) : this(player, Operation.Upgrade)
        {
            Techs = techs;
        }

        /// <summary>
        /// ����Ʒ����
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sellContexts"></param>
        public PlayerOperationContext(PlayerEnum player, List<SellContext> sellContexts) : this(player, Operation.Sell)
        {
            SellContexts = sellContexts;
        }
    }
}