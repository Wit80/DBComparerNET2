using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary
{
    public static class Comparers
    {
        public static bool EnumEquals<T>(List<T> list1, List<T> list2)
            where T : IEquatable<T>
        {
            if (null == list2)
                return null == list1;
            if (null == list1)
                return false;
            if (object.ReferenceEquals(list1, list2))
                return true;
            if (list1.Count != list2.Count)
                return false;

            foreach (T item in list1) 
            {
                if (!list2.Contains(item))
                    return false;
            }
            return true;

            /*var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {

                    return false;
                }
            }
            foreach (var value in cnt.Values)
            {
                if (0 != value)
                    return false;
            }
            return true;*/
        }

        public static bool DictEquals<TKey, TValue>(IDictionary<TKey, TValue> dict1, IDictionary<TKey, TValue> dict2) 
            where TKey: IEquatable<TKey>
            where TValue: IEquatable<TValue>
        {
            if (null == dict2)
                return null == dict1;
            if (null == dict1)
                return false;
            if (object.ReferenceEquals(dict1, dict2))
                return true;
            if (dict1.Count != dict2.Count)
                return false;

            // проверим ключи
            foreach (TKey k in dict1.Keys)
                if (!dict2.ContainsKey(k))
                    return false;

            // проверим значения
            foreach (TKey k in dict1.Keys)
            {
                if (!dict1[k].Equals(dict2[k]))
                    return false;
            }

            return true;
        }
    }
}
