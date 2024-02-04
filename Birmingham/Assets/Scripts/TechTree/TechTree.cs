using UnityEngine;

namespace Game.Tech
{
    /// <summary>
    /// �Ƽ���
    /// </summary>
    public interface IReadOnlyTechTree
    {
        /// <summary>
        /// �Ƽ���������
        /// </summary>
        public TechType Type { get; }
        /// <summary>
        /// �Ƽ����ĸ��ڵ�
        /// </summary>
        public IReadOnlyTechNode Root { get; }

        /// <summary>
        /// ���һ�ÿƼ���
        /// </summary>
        /// <returns>������Ŀ����޸ĿƼ���</returns>
        public TechTree Clone();
    }
    
    /// <summary>
    /// �Ƽ���
    /// </summary>
    public class TechTree : IReadOnlyTechTree
    {
        private TechType _type;
        /// <summary>
        /// �Ƽ���������
        /// </summary>
        public TechType Type => _type;

        private TechNode _root = null;
        
        /// <summary>
        /// �Ƽ����ĸ��ڵ�
        /// </summary>
        public TechNode Root
        {
            get { return _root; }
            set { _root = value; }
        }

        /// <summary>
        /// �ÿƼ����У���ÿһ���Ƽ���������ӵõ����ܺ�
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