using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TechTree
{
    /// <summary>
    /// �Ƽ�����
    /// </summary>
    public enum TechType
    {
        /// <summary>
        /// �޻�
        /// </summary>
        Cotton = 0,

        /// <summary>
        /// ��
        /// </summary>
        Wine,

        /// <summary>
        /// �մ�
        /// </summary>
        Ceramics,

        /// <summary>
        /// ������
        /// </summary>
        Crate,

        /// <summary>
        /// ú̿��
        /// </summary>
        Coal,

        /// <summary>
        /// ������
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
        /// ����������
        /// </summary>
        /// <returns></returns>
        public TechTypeVector<T> Clone();
    }

    /// <summary>
    /// һ�����������ڷ����װ�غ�TechType�����йص����ݡ�
    /// </summary>
    /// <typeparam name="T">Ҫ�洢����������T</typeparam>
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


}