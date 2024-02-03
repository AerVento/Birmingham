using System.Collections;
using System.Collections.Generic;

namespace Game.Map
{
    /// <summary>
    /// 对路进行包装，使得不用在乎方向性
    /// </summary>
    public class MapRoads: IDictionary<(CityName a, CityName b), Road>
    {
        /// <summary>
        /// 保存在其中时，a的值一定小于b的值
        /// </summary>
        private Dictionary<(CityName a, CityName b), Road> _roads;

        public Road this[(CityName a, CityName b) key] 
        {
            get
            {
                if ((int)key.b < (int)key.a)
                    Swap(ref key.a, ref key.b);

                return _roads[key];
            }
            set
            {
                if ((int)key.b < (int)key.a)
                    Swap(ref key.a, ref key.b);

                _roads[key] = value;
            }
        }

        public ICollection<(CityName a, CityName b)> Keys => ((IDictionary<(CityName a, CityName b), Road>)_roads).Keys;

        public ICollection<Road> Values => ((IDictionary<(CityName a, CityName b), Road>)_roads).Values;

        public int Count => ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).IsReadOnly;

        public void Add((CityName a, CityName b) key, Road value)
        {
            if ((int)key.b < (int)key.a)
                Swap(ref key.a, ref key.b);
            ((IDictionary<(CityName a, CityName b), Road>)_roads).Add(key, value);
        }

        public void Add(KeyValuePair<(CityName a, CityName b), Road> item)
        {
            if ((int)item.Key.b < (int)item.Key.a)
                item = new KeyValuePair<(CityName a, CityName b), Road>((item.Key.b, item.Key.a), item.Value);
            ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).Clear();
        }

        public bool Contains(KeyValuePair<(CityName a, CityName b), Road> item)
        {
            if ((int)item.Key.b < (int)item.Key.a)
                item = new KeyValuePair<(CityName a, CityName b), Road>((item.Key.b, item.Key.a), item.Value);
            return ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).Contains(item);
        }

        public bool ContainsKey((CityName a, CityName b) key)
        {
            return _roads.ContainsKey(key) || _roads.ContainsKey((key.b, key.a));
        }

        public void CopyTo(KeyValuePair<(CityName a, CityName b), Road>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<(CityName a, CityName b), Road>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<(CityName a, CityName b), Road>>)_roads).GetEnumerator();
        }

        public bool Remove((CityName a, CityName b) key)
        {
            if ((int)key.b < (int)key.a)
                Swap(ref key.a, ref key.b);
            return ((IDictionary<(CityName a, CityName b), Road>)_roads).Remove(key);
        }

        public bool Remove(KeyValuePair<(CityName a, CityName b), Road> item)
        {
            if ((int)item.Key.b < (int)item.Key.a)
                item = new KeyValuePair<(CityName a, CityName b), Road>((item.Key.b, item.Key.a), item.Value);
            return ((ICollection<KeyValuePair<(CityName a, CityName b), Road>>)_roads).Remove(item);
        }

        public bool TryGetValue((CityName a, CityName b) key, out Road value)
        {
            if ((int)key.b < (int)key.a)
                Swap(ref key.a, ref key.b);
            return ((IDictionary<(CityName a, CityName b), Road>)_roads).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_roads).GetEnumerator();
        }

        private void Swap(ref CityName a,  ref CityName b)
        {
            (b, a) = (a, b);
        }
    }
}