using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TechTree
{
    /// <summary>
    /// �Ƽ�����SO�����ļ�
    /// </summary>
    [CreateAssetMenu(fileName = "NewTechTreeSOFile", menuName = "SO/TechTreeSO")]
    public class TechTreeSO : ScriptableObject
    {
        /// <summary>
        /// �Ƽ����������
        /// </summary>
        [SerializeField]
        private SerializedDictionary<TechType, List<InternalTechNodeData>> _initial;

        /// <summary>
        /// �Ƽ���ԭ�ͣ�ÿ����ҵĿƼ������ɴ˿�¡��
        /// </summary>
        private TechTypeVector<TechTree> _prototype = new TechTypeVector<TechTree>(); 

        private void Awake()
        {
            foreach(var pair in  _initial)
            {
                if(pair.Value.Count == 0) 
                    continue;
                TechTree tree = new TechTree(pair.Key, new TechNode(pair.Value[0].Data));
                
                TechNode pointer = tree.Root;
                for(int i = 1; i < pair.Value.Count; i++)
                {
                    TechNode tmp = new TechNode(pair.Value[i].Data);
                    pointer.Next = tmp;
                    pointer = tmp;
                }

                _prototype[pair.Key] = tree;
            }
        }

        public TechTypeVector<TechTree> GetTree()
        {
            // ���ﲻֱ�ӵ���Clone����ΪTechTypeVectorֻ���¡�Լ����������¡�ڲ�����
            TechTypeVector<TechTree> newTree = new TechTypeVector<TechTree>();
            foreach (var pair in _prototype)
            {
                newTree[pair.Key] = _prototype[pair.Key].Clone();
            }
            return newTree;
        }

        [System.Serializable]
        class InternalTechNodeData
        {
            /// <summary>
            /// ����UI�����棬�����÷����桢��·��ֵ����������
            /// </summary>
            public Sprite Front;

            /// <summary>
            /// ����UI�ı��棬����ʹ�䴴����������Ҫ����Դ
            /// </summary>
            public Sprite Back;

            /// <summary>
            /// �������������Ϣ
            /// </summary>
            public TechNodeData Data;
        }
    }


}
