using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal static class Utilities
    {
        public static void DeleteWithSwapAndPop<T>(List<T> collection, Predicate<T> deletePredicate)
        {
            int i = 0;
            while (i < collection.Count)
            {
                if (!deletePredicate.Invoke(collection[i]))
                {
                    i++;
                    continue;
                }

                // Swap and pop
                int lastIndex = collection.Count - 1;
                collection[i] = collection[lastIndex];
                collection.RemoveAt(lastIndex);
            }
        }
    }
}
