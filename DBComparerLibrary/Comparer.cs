using DBComparerLibrary.DBSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary
{
    public static class Comparer
    {
        public static List<T> GetDifferenceGen<T>(List<T> list1, List<T> list2)
            where T : IEquatable<T>
        {
            List<T> difference = new List<T>();
            foreach (T item in list1) 
            {
                if(!list2.Contains(item))
                    difference.Add(item);
            }
            foreach (T item in list2)
            {
                if (!list1.Contains(item))
                    difference.Add(item);
            }

            return difference;
        }
        public static List<string> GetDifference(List<string> list1, List<string> list2)
        {
            Dictionary<string,string> l1= new Dictionary<string,string>();
            Dictionary<string, string> l2 = new Dictionary<string, string>();
            foreach (string item in list1) 
            {
                l1.Add(item.ToUpper(),item);
            }
            foreach (string item in list2)
            {
                l2.Add(item.ToUpper(), item);
            }
            var difference = GetDifferenceGen(new List<string>( l1.Keys), new List<string>( l2.Keys));
            for (int i = 0; i < difference.Count; i++) 
            {
                if(l1.ContainsKey(difference[i]))
                    difference[i] = l1[difference[i]];
                else
                    difference[i] = l2[difference[i]];
            }
            return difference;
        }
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
        public static bool CompareStrings(string o1, string o2) 
        {
            return object.ReferenceEquals(o1, o2) ||
                    o1 != null &&
                    o1.Equals(o2,StringComparison.OrdinalIgnoreCase);
        }
        public static Dictionary<CompareItogEnum, List<string>> dbItemComtare<TKey, TValue>(IDictionary<string, TValue> items1, IDictionary<string, TValue> items2)
            where TKey : IEquatable<string>
            where TValue : IEquatable<TValue>
        {
            var diffMissingKeys = GetDifference(new List<string>(items1.Keys), new List<string>(items2.Keys));
            List<string> diffExtendedProp = new List<string>();
            foreach (var item in items1.Keys)
            {
                if (diffMissingKeys.Contains(item))
                    continue;

                if (items1[item].Equals(items2[item]))
                    continue;

                diffExtendedProp.Add(item);

            }
            return new Dictionary<CompareItogEnum, List<string>>() {
                {CompareItogEnum.missing, diffMissingKeys} ,
                {CompareItogEnum.extendetDifference, diffExtendedProp}
            };
        }

    }
}
