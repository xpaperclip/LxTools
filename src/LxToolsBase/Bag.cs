using System;
using System.Collections.Generic;
using System.Linq;

namespace LxTools
{
    public class Bag : IEnumerable<KeyValuePair<string,string>>
    {
        private static Bag _empty = new Bag();
        public static Bag Empty { get { return _empty; } }

        public Bag()
        {
        }
        public Bag(params string[] paramset)
        {
            SetParams(paramset);
        }

        private readonly Dictionary<string, string> dict = new Dictionary<string, string>();
        public string this[string param]
        {
            get { return dict[param]; }
            set { dict[param] = value; }
        }

        public Dictionary<string, string> BackingStore
        {
            get { return dict; }
        }
        public int Count
        {
            get { return dict.Count; }
        }

        public void Clear()
        {
            dict.Clear();
        }
        public void SetParams(params string[] paramset)
        {
            if (paramset.Length % 2 != 0)
                throw new ArgumentException("Parameter list must be a set of key/value pairs.");

            for (int i = 0; i < paramset.Length; i += 2)
            {
                string key = paramset[i];
                string value = paramset[i + 1];

                this[key] = value;
            }
        }
        public void Merge(Bag other, bool replace)
        {
            dict.Merge(other.dict, replace);
        }
        public void MergeGrouping<TKey, TElement>(IGrouping<TKey, TElement> lookup, Func<TElement, string> key, Func<TElement, string> value)
        {
            foreach (var item in lookup)
            {
                this[key(item)] = value(item);
            }
        }

        public bool ContainsKey(string key)
        {
            return dict.ContainsKey(key);
        }

        #region IEnumerable Implementation
        
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }
        
        #endregion
    }
}
