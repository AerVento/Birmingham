using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tech
{
    /// <summary>
    /// 科技树的SO配置文件
    /// </summary>
    [CreateAssetMenu(fileName = "NewTechTreeSOFile", menuName = "SO/TechTreeSO")]
    public class TechTreeSO : ScriptableObject
    {
        /// <summary>
        /// 科技树相关设置
        /// </summary>
        [SerializeField]
        private SerializedDictionary<TechType, List<InternalTechNodeData>> _initial;

        private bool _isInitialized = false;

        /// <summary>
        /// 科技树原型，每个玩家的科技树都由此克隆出
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
            // 这里不直接调用Clone是因为TechTypeVector只会克隆自己，而不会克隆内部对象
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
            /// 建筑相关数据信息
            /// </summary>
            public TechNodeData Data;
        }
    }


}
