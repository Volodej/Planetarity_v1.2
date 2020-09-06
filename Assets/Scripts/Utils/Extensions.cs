using System.Collections.Generic;
using UnityEngine;

namespace Planetarity.Utils
{
    public static class Extensions
    {
        public static T RandomElement<T>(this IReadOnlyList<T> collection) => collection[Random.Range(0, collection.Count)];

        public static T RandomElement<T>(this IReadOnlyList<T> collection, System.Random random) =>
            collection[random.Next(collection.Count)];

        public static void Shuffle<T>(this IList<T> list, System.Random random)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = random.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable) => new HashSet<T>(enumerable);
    }
}