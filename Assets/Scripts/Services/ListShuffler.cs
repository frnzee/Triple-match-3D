using System.Collections.Generic;

namespace Services
{
    public static class ListShuffler
    {
        private static readonly System.Random Random = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}