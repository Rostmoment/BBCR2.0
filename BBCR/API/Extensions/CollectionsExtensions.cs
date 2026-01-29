using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBCR.API.Extensions
{
    public static class CollectionsExtensions
    {
        public static T Find<T>(this T[] array, Func<T, bool> func)
        {
            IEnumerable<T> t = array.Where(func);
            if (t.Count() > 0) return t.First();
            return default;
        }
        public static bool EmptyOrNull<T>(this IEnumerable<T> values)
        {
            if (values == null) return true;
            return values.Count() == 0;
        }
        public static T[] ChooseRandom<T>(this IEnumerable<T> list, int count)
        {
            if (list.Count() <= count)
                return list.ToArray();
            List<T> result = new List<T>();
            List<T> tmp = list.ToList();
            while (result.Count != count)
            {
                T add = tmp.ChooseRandom();
                tmp.Remove(add);
                result.Add(add);
            }
            return result.ToArray();
        }
        public static T ChooseRandom<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return default;

            return list.ToList()[UnityEngine.Random.Range(0, list.Count())];
        }

        public static List<List<T>> SplitList<T>(this List<T> values, int chunkSize)
        {
            List<List<T>> res = new List<List<T>>();
            for (int i = 0; i < values.Count; i += chunkSize)
            {
                res.Add(values.GetRange(i, Math.Min(chunkSize, values.Count - i)));
            }
            return res;
        }
    }
}
