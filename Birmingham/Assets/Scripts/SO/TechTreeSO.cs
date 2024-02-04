using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tech
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

        private bool _isInitialized = false;

        /// <summary>
        /// �Ƽ���ԭ�ͣ�ÿ����ҵĿƼ������ɴ˿�¡��
        /// </summary>
        private TechTypeVector<TechTree> _prototype = new TechTypeVector<TechTree>(); 

        private void Init()
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
            if(!_isInitialized)
            {
                Init();
                _isInitialized = true;
            }
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
            /// �������������Ϣ
            /// </summary>
            public TechNodeData Data;
        }
    }


}
