using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tech
{
    /// <summary>
    /// 科技种类
    /// </summary>
    public enum TechType
    {
        /// <summary>
        /// 棉花
        /// </summary>
        Cotton = 0,

        /// <summary>
        /// 陶瓷
        /// </summary>
        Ceramics,

        /// <summary>
        /// 板条箱
        /// </summary>
        Crate,

        /// <summary>
        /// 酒
        /// </summary>
        Wine,

        /// <summary>
        /// 煤炭厂
        /// </summary>
        Coal,

        /// <summary>
        /// 钢铁厂
        /// </summary>
        Iron,
    }

    public interface IReadOnlyTechTypeVector<T> : IEnumerable<KeyValuePair<TechType, T>>
    {
        public T this[TechType type]
        {
            get;
        }

        /// <summary>
        /// 深拷贝这个向量
        /// </summary>
        /// <returns></returns>
        public TechTypeVector<T> Clone();
    }

    /// <summary>
    /// 一个向量，用于方便的装载和TechType种类有关的数据。
    /// </summary>
    /// <typeparam name="T">要存储的数据类型T</typeparam>
    [System.Serializable]
    public class TechTypeVector<T> : IReadOnlyTechTypeVector<T>
    {
        [SerializeField]
        private T[] _array = new T[6];

        public T this[TechType type]
        {
            get
            {
                return _array[(int)type];
            }

            set
            {
                _array[(int)type] = value;
            }
        }

        public TechTypeVector<T> Clone()
        {
            TechTypeVector<T> newVec = new TechTypeVector<T>();
            System.Array.Copy(_array, newVec._array, _array.Length);
            return newVec;
        }

        public IEnumerator<KeyValuePair<TechType, T>> GetEnumerator()
        {
            for(int i = 0; i < _array.Length; i++)
            {
                yield return new KeyValuePair<TechType, T>((TechType)i, _array[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class TechTypeUtils
    {
        /// <summary>
        /// 该科技类型是否属于经济建筑。经济建筑包含板条箱、棉花、陶瓷。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEconomy(this TechType type)
        {
            return type == TechType.Cotton || type == TechType.Ceramics || type == TechType.Crate;
        }
    }
}