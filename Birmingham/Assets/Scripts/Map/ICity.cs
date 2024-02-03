using Game.TechTree;

namespace Game.Map
{
    /// <summary>
    /// ��ͼ�ϳ��еĳ���ӿ�
    /// </summary>
    public interface ICity
    {
        /// <summary>
        /// ��������
        /// </summary>
        public CityName Name { get; }

        /// <summary>
        /// �ó����м������ӿ��Խ��콨��
        /// </summary>
        public ushort GridCount { get; }

        /// <summary>
        /// ��ȡ��ǰ����ָ���±괦���ӵ��ܽ�����Щ����
        /// </summary>
        /// <param name="index">�±�</param>
        /// <returns>һ�����������Ϊtrue�����˸��ӿ��Խ���ÿƼ����͵Ľ���</returns>
        public TechTypeVector<bool> GetAvailables(ushort index);

        /// <summary>
        /// ��ͼ�����仯ʱ�Ļص�
        /// </summary>
        public MapChangingEvent Callback { get; }

        /// <summary>
        /// ����һ������֮���Ƿ����ͨ���޽��˺ӵķ�ʽʹ������
        /// </summary>
        /// <param name="other">��һ������</param>
        public bool IsConnectedByCanal(CityName other);

        /// <summary>
        /// ����һ������֮���Ƿ����ͨ���޽���·�ķ�ʽʹ������
        /// </summary>
        /// <param name="other">��һ������</param>
        public bool IsConnectedByRailway(CityName other);
    }

    public static class CityFactory
    {
        public static ICity GetCity(CityName name)
        {
            throw new System.NotImplementedException();
        }    
    }
}