using System.Collections.Generic;

namespace Graph.Core.Utils
{
    public static class DictionaryExtensions
    {
        public static void SafeAdd<T, K, KL>(this IDictionary<T, KL> dictionray, T key, K value)
            where K : class, new()
            where KL : ICollection<K>, new()
        {
            if (dictionray.TryGetValue(key, out var list))
            {
                if (list == null)
                {
                    dictionray[key] = new KL { value };
                }
                else
                {
                    dictionray[key].Add(value);
                }
            }
        }
    }
}
