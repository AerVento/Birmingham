using Game.Player;

namespace Game.Map
{
    /// <summary>
    /// ��·����
    /// </summary>
    public enum RoadType
    {
        /// <summary>
        /// �˺�
        /// </summary>
        Canal,
        
        /// <summary>
        /// ��·
        /// </summary>
        Railway
    }
    /// <summary>
    /// ��Ϸ�еĵ�·
    /// </summary>
    public class Road
    {
        /// <summary>
        /// ·����ʼ����
        /// </summary>
        public CityName A { get; private set; }

        /// <summary>
        /// ·����ֹ����
        /// </summary>
        public CityName B { get; private set; }

        /// <summary>
        /// ·�����������
        /// </summary>
        public PlayerEnum Player { get; private set; }

        /// <summary>
        /// ��·����
        /// </summary>
        public RoadType RoadType { get; private set; }

        /// <summary>
        /// ����һ��·����Ϣ
        /// </summary>
        /// <param name="a">·����ʼ����</param>
        /// <param name="b">·����ֹ����</param>
        /// <param name="player">·�����������</param>
        /// <param name="roadType">��·����</param>
        public Road(CityName a, CityName b, PlayerEnum player, RoadType roadType)
        {
            A = a;
            B = b;
            Player = player;
            RoadType = roadType;
        }

        /// <summary>
        /// ����·��Ϣ�Ƿ���ͬһ��λ����
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool IsSamePosition(Road other) => IsSamePosition(this, other);
        
        /// <summary>
        /// ����·��Ϣ�Ƿ���ͬһ��λ����
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool IsSamePosition(Road a, Road b)
        {
            return (a.A == b.A && a.B == b.B) || (a.A == b.B && a.B == b.A);
        }
    }
}