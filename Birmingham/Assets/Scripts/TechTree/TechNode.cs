using UnityEngine;

namespace Game.Tech
{
    public interface IReadOnlyTechNode
    {
        /// <summary>
        /// 这个建筑是否能在运河时代建造
        /// </summary>
        public bool IsCanalAge { get; }

        /// <summary>
        /// 这个建筑是否能在铁路时代建造
        /// </summary>
        public bool IsRailwayAge { get; }

        /// <summary>
        /// 建造这个建筑需要的金币
        /// </summary>
        public ushort Cost { get; }

        /// <summary>
        /// 这个等级的建筑是否可被研发
        /// </summary>
        public bool CanUpgrade { get; }

        /// <summary>
        /// 建造此建筑需要哪些资源
        /// </summary>
        public IReadOnlyTechTypeVector<ushort> ItemCost { get; }

        /// <summary>
        /// 相同建筑的数量
        /// </summary>
        public ushort Count { get; }

        /// <summary>
        /// 得分收益
        /// </summary>
        public ushort Score { get; }

        /// <summary>
        /// 道路价值
        /// </summary>
        public ushort RoadValue { get; }

        /// <summary>
        /// 经济收益
        /// </summary>
        public ushort EconomyValue { get; }

        /// <summary>
        /// 建造此建筑后需要哪些资源才能够使其产生收益。
        /// </summary>
        public IReadOnlyTechTypeVector<ushort> ItemNeeds { get; }

        /// <summary>
        /// 下一层结点
        /// </summary>
        public IReadOnlyTechNode Next { get; }

        /// <summary>
        /// 克隆出一个可修改的科技树，以方便记录玩家科技进度
        /// </summary>
        /// <returns>从当前科技树中克隆出一个可修改的科技树</returns>
        public TechNode Clone();
    }

    /// <summary>
    /// 科技树结点
    /// </summary>
    public class TechNode : IReadOnlyTechNode
    {
        public TechNodeData Data;
        public TechNode Next = null;

        #region Interface Implementes
        bool IReadOnlyTechNode.IsCanalAge => Data.IsCanalAge;
        bool IReadOnlyTechNode.IsRailwayAge => Data.IsRailwayAge;
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
            return new TechNode(Data.Clone(), Next?.Clone());
        }
    }

    [System.Serializable]
    public class TechNodeData
    {
        /// <summary>
        /// 这个建筑是否能在运河时代建造
        /// </summary>
        [Tooltip("这个建筑是否能在运河时代建造")]
        public bool IsCanalAge;

        /// <summary>
        /// 这个建筑是否能在铁路时代建造
        /// </summary>
        [Tooltip("这个建筑是否能在铁路时代建造")]
        public bool IsRailwayAge;

        /// <summary>
        /// 建造这个建筑需要的金币
        /// </summary>
        [Tooltip("建造这个建筑需要的金币")]
        public ushort Cost;

        /// <summary>
        /// 这个等级的建筑是否可被研发
        /// </summary>
        [Tooltip("这个等级的建筑是否可被研发")]
        public bool CanUpgrade;

        /// <summary>
        /// 建造此建筑需要哪些资源
        /// </summary>
        [Tooltip("建造此建筑需要哪些资源")]
        public TechTypeVector<ushort> ItemCost = new TechTypeVector<ushort>();

        /// <summary>
        /// 同等级下相同建筑的数量
        /// </summary>
        [Tooltip("同等级下相同建筑的数量")]
        public ushort Count;

        /// <summary>
        /// 得分收益
        /// </summary>
        [Tooltip("得分收益")]
        public ushort Score;

        /// <summary>
        /// 道路价值
        /// </summary>
        [Tooltip("道路价值")]
        public ushort RoadValue;

        /// <summary>
        /// 经济收益
        /// </summary>
        [Tooltip("经济收益")]
        public ushort EconomyValue;

        /// <summary>
        /// 建造此建筑后需要哪些资源才能够使其产生收益。
        /// </summary>
        [Tooltip("建造此建筑后需要哪些资源才能够使其产生收益。")]
        public TechTypeVector<ushort> ItemNeeds = new TechTypeVector<ushort>();

        
        /// <summary>
        /// 深拷贝这个科技树结点数据信息
        /// </summary>
        /// <returns></returns>
        public TechNodeData Clone()
        {
            TechNodeData newData = new TechNodeData();
            newData.IsCanalAge = IsCanalAge;
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