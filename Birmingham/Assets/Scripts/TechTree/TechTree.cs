using UnityEngine;

namespace Game.TechTree
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
    [System.Serializable]
    public class TechTree : IReadOnlyTechTree
    {
        [SerializeField]
        private TechType _type;
        /// <summary>
        /// �Ƽ���������
        /// </summary>
        public TechType Type => _type;

        [SerializeField]
        private TechNode _root;
        /// <summary>
        /// �Ƽ����ĸ��ڵ�
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