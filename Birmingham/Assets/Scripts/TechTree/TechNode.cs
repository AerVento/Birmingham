using UnityEngine;

namespace Game.TechTree
{
    public interface IReadOnlyTechNode
    {
        /// <summary>
        /// ��������Ƿ�ֻ�����˺�ʱ������
        /// </summary>
        public bool IsTemp { get; }

        /// <summary>
        /// �������������Ҫ�Ľ��
        /// </summary>
        public ushort Cost { get; }

        /// <summary>
        /// ����ȼ��Ľ����Ƿ�ɱ��з�
        /// </summary>
        public bool CanUpgrade { get; }

        /// <summary>
        /// ����˽�����Ҫ��Щ��Դ
        /// </summary>
        public IReadOnlyTechTypeVector<ushort> ItemCost { get; }

        /// <summary>
        /// ��ͬ����������
        /// </summary>
        public ushort Count { get; }

        /// <summary>
        /// �÷�����
        /// </summary>
        public ushort Score { get; }

        /// <summary>
        /// ��·��ֵ
        /// </summary>
        public ushort RoadValue { get; }

        /// <summary>
        /// ��������
        /// </summary>
        public ushort EconomyValue { get; }

        /// <summary>
        /// ����˽�������Ҫ��Щ��Դ���ܹ�ʹ��������档
        /// </summary>
        public IReadOnlyTechTypeVector<ushort> ItemNeeds { get; }

        /// <summary>
        /// ��һ����
        /// </summary>
        public IReadOnlyTechNode Next { get; }

        /// <summary>
        /// ��¡��һ�����޸ĵĿƼ������Է����¼��ҿƼ�����
        /// </summary>
        /// <returns>�ӵ�ǰ�Ƽ����п�¡��һ�����޸ĵĿƼ���</returns>
        public TechNode Clone();
    }

    /// <summary>
    /// �Ƽ������
    /// </summary>
    [System.Serializable]
    public class TechNode : IReadOnlyTechNode
    {
        public TechNodeData Data;
        public TechNode Next = null;

        #region Interface Implementes
        bool IReadOnlyTechNode.IsTemp => Data.IsTemp;
        ushort IReadOnlyTechNode.Cost => Data.Cost;
        bool IReadOnlyTechNode.CanUpgrade => Data.CanUpgrade;
        IReadOnlyTechTypeVector<ushort> IReadOnlyTechNode.ItemCost => Data.ItemCost;
        ushort IReadOnlyTechNode.Count => Data.Count;
        ushort IReadOnlyTechNode.Score => Data.Score;
        ushort IReadOnlyTechNode.RoadValue => Data.RoadValue;
        ushort IReadOnlyTechNode.EconomyValue => Data.EconomyValue;
        IReadOnlyTechTypeVector<ushort> IReadOnlyTechNode.ItemNeeds => Data.ItemNeeds;
        IReadOnlyTechNode IReadOnlyTechNode.Next => Next;

        #endregion

        public TechNode(TechNodeData data, TechNode next = null)
        {
            Data = data;
            Next = next;
        }

        public TechNode Clone()
        {
            return new TechNode(Data.Clone(), Next ?? Next.Clone());
        }
    }

    [System.Serializable]
    public class TechNodeData
    {
        /// <summary>
        /// ��������Ƿ�ֻ�����˺�ʱ������
        /// </summary>
        public bool IsTemp;

        /// <summary>
        /// �������������Ҫ�Ľ��
        /// </summary>
        public ushort Cost;

        /// <summary>
        /// ����ȼ��Ľ����Ƿ�ɱ��з�
        /// </summary>
        public bool CanUpgrade;

        /// <summary>
        /// ����˽�����Ҫ��Щ��Դ
        /// </summary>
        public TechTypeVector<ushort> ItemCost = new TechTypeVector<ushort>();

        /// <summary>
        /// ��ͬ����������
        /// </summary>
        public ushort Count;

        /// <summary>
        /// �÷�����
        /// </summary>
        public ushort Score;

        /// <summary>
        /// ��·��ֵ
        /// </summary>
        public ushort RoadValue;

        /// <summary>
        /// ��������
        /// </summary>
        public ushort EconomyValue;

        /// <summary>
        /// ����˽�������Ҫ��Щ��Դ���ܹ�ʹ��������档
        /// </summary>
        public TechTypeVector<ushort> ItemNeeds = new TechTypeVector<ushort>();

        
        /// <summary>
        /// �������Ƽ������������Ϣ
        /// </summary>
        /// <returns></returns>
        public TechNodeData Clone()
        {
            TechNodeData newData = new TechNodeData();
            newData.IsTemp = IsTemp;
            newData.Cost = Cost;
            newData.CanUpgrade = CanUpgrade;
            newData.ItemCost = ItemCost.Clone();
            newData.Count = Count;
            newData.Score = Score;
            newData.RoadValue = RoadValue;
            newData.EconomyValue = EconomyValue;
            newData.ItemNeeds = ItemNeeds.Clone();
            return newData;
        }
    }
}