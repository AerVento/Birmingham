using UnityEngine;

namespace Game.TechTree
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
    [System.Serializable]
    public class TechTree : IReadOnlyTechTree
    {
        [SerializeField]
        private TechType _type;
        /// <summary>
        /// 科技树的种类
        /// </summary>
        public TechType Type => _type;

        [SerializeField]
        private TechNode _root;
        /// <summary>
        /// 科技树的根节点
        /// </summary>
        public TechNode Root => _root;


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
            return new TechTree(_type, _root ?? _root.Clone());
        }

    }


}