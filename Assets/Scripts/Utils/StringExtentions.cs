    using System.Collections.Generic;

    public static class StringExtentions
    {
        public static string RemoveAt(this string s, int index)
        {
            return s.Remove(index, 1);
        }
        
        public static IEnumerable<int> AllIndexesOf(this string str, char searchChar)
        {
            int minIndex = str.IndexOf(searchChar);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchChar, minIndex + 1);
            }
        }
    }
