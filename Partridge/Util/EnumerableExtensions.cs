using System;
using System.Collections.Generic;
using System.Linq;

namespace Partridge.Util
{
    public static class EnumerableExtensions
    {

        /// <summary>
        /// Executes the action for each item in the source. This function causes side effects 
        /// to the global state and should be used accordingly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");

            foreach (T target in source)
                action(target);
        }

        /// <summary>
        /// Executes the action for each item in the source and supplies the index to the action. This 
        /// function causes side effects to the global state and should be used accordingly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        public static void EachWithIndex<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null) throw new ArgumentNullException("source");

            for (int i = 0; i < source.Count(); i++)
            {
                action(source.ElementAt(i), i);
            }
        }

        /// <summary>
        /// Executes the action for each item in the source. This function causes side effects 
        /// to the global state and should be used accordingly.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static void Each<K,V>(this IDictionary<K,V> source, Action<K,V> action)
        {
            if (source == null) throw new ArgumentNullException("source");

            foreach (KeyValuePair<K, V> target in source)
                action(target.Key, target.Value);
        }
    }
}
