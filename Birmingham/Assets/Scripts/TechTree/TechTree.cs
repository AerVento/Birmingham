using UnityEngine;

namespace Game.Tech
{
    /// <summary>
    /// 科技树
    /// </summary>
    public interface IReadOnlyTechTree
    {
        /// <summary>
        /// 科技树的种类
        /// </summary>
        public TechType Type { get; }
        /// <summary>
        /// 科技树的根节点
        /// </summary>
        public IReadOnlyTechNode Root { get; }

        /// <summary>
        /// 深拷贝一棵科技树
        /// </summary>
        /// <returns>拷贝后的可以修改科技树</returns>
        public TechTree Clone();
    }
    
    /// <summary>
    /// 科技树
    /// </summary>
    public class TechTree : IReadOnlyTechTree
    {
        private TechType _type;
        /// <summary>
        /// 科技树的种类
        /// </summary>
        public TechType Type => _type;

        private TechNode _root = null;
        
        /// <summary>
        /// 科技树的根节点
        /// </summary>
        public TechNode Root
        {
            get { return _root; }
            set { _root = value; }
        }

        /// <summary>
        /// 该科技树中，将每一级科技的数量相加得到的总和
        /// </summary>
        /// <see cref="TechNodeData.Count"/>
        public ushort Total
        {
            get
            {
                ushort ans = 0;
                TechNode node = _root;
                while(node != null)
                {
                    ans += node.Data.Count;
                    node = node.Next;
                }
                return ans;
            }
        }

        #region Interface Implements
        IReadOnlyTechNode IReadOnlyTechTree.Root => _root;
        #endregion

        public TechTree(TechType type, TechNode root = null)
        {
            _type = type;
            _root = root;
        }

        public TechTree Clone()
        {
            return new TechTree(_type, _root?.Clone());
        }

    }


}