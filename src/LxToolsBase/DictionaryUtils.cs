using System;
using System.Collections.Generic;
using System.IO;

namespace LxTools
{
    public static class DictionaryUtils
    {
        public static Dictionary<string, string> Read(string filename)
        {
            var dict = new Dictionary<string, string>();
            using (var sr = new StreamReader(filename))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Length == 0 || s.StartsWith(";")) continue;
                    string[] xs = s.Split(",".ToCharArray(), 2);
                    dict[xs[0]] = xs[1];
                }
            }
            return dict;
        }
        public static Dictionary<string, TValue> Read<TValue>(string filename, Func<string, TValue> transform)
        {
            var dict = new Dictionary<string, TValue>();
            using (var sr = new StreamReader(filename))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Length == 0 || s.StartsWith(";")) continue;
                    string[] xs = s.Split(",".ToCharArray(), 2);
                    dict[xs[0]] = transform(xs[1]);
                }
            }
            return dict;
        }

        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other)
        {
            Merge(dictionary, other, false);
        }
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other, bool replaceExistingKeys)
        {
            foreach (var kvp in other)
            {
                // if not replacing values, skip already existing keys
                if (!replaceExistingKeys && dictionary.ContainsKey(kvp.Key)) continue;
                dictionary[kvp.Key] = kvp.Value;
            }
        }

        public static TKey GetValueOrDefault<TKey>(this IDictionary<TKey, TKey> dictionary,
            TKey key)
        {
            TKey value;
            return dictionary.TryGetValue(key, out value) ? value : key;
        }
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, TValue defaultValue)
        {
            TValue value;
            if (key == null) return defaultValue;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}
