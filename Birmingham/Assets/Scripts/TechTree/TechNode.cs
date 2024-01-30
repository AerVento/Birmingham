using UnityEngine;

namespace Game.TechTree
{
    public interface IReadOnlyTechNode
    {
        /// <summary>
        /// 这个建筑是否只能在运河时代建造
        /// </summary>
        public bool IsTemp { get; }

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
        /// 这个建筑是否只能在运河时代建造
        /// </summary>
        public bool IsTemp;

        /// <summary>
        /// 建造这个建筑需要的金币
        /// </summary>
        public ushort Cost;

        /// <summary>
        /// 这个等级的建筑是否可被研发
        /// </summary>
        public bool CanUpgrade;

        /// <summary>
        /// 建造此建筑需要哪些资源
        /// </summary>
        public TechTypeVector<ushort> ItemCost = new TechTypeVector<ushort>();

        /// <summary>
        /// 相同建筑的数量
        /// </summary>
        public ushort Count;

        /// <summary>
        /// 得分收益
        /// </summary>
        public ushort Score;

        /// <summary>
        /// 道路价值
        /// </summary>
        public ushort RoadValue;

        /// <summary>
        /// 经济收益
        /// </summary>
        public ushort EconomyValue;

        /// <summary>
        /// 建造此建筑后需要哪些资源才能够使其产生收益。
        /// </summary>
        public TechTypeVector<ushort> ItemNeeds = new TechTypeVector<ushort>();

        
        /// <summary>
        /// 深拷贝这个科技树结点数据信息
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