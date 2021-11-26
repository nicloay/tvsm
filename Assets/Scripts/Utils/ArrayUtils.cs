using System.Collections.Generic;
using UnityEngine;

namespace TheseusAndMinotaur.Utils
{
    public static class ArrayUtils
    {

        /// <summary>
        /// Convert single index in to 2dim array coordinates (x + y)
        /// e.g. we can have id{1,2,3,4}
        /// and for 2d array it would be {0,0},{0,1},{1,0},{1,1},
        /// So this function convert 2 in to Vector2Int(0,1) (2d row and 1st column) 
        /// 
        /// </summary>
        public static Vector2Int Get2DCoordinates(this int value, int columnSize)
        {
            var rowId = value / columnSize;
            var columnId = value % columnSize;
            return new Vector2Int(columnId, rowId);
        }
        
        
        /// <summary>
        /// get sequence 0,1,2,3...n
        /// </summary>
        public static int[] GetOrderedSequence(int arraySize)
        {
            var result = new int[arraySize];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = i;
            }

            return result;
        }
        
        
        /// <summary>
        /// shuffle elements in the array
        /// </summary>
        public static void ShuffleArray(this IList<int> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                var random = UnityEngine.Random.Range(0, array.Count);
                (array[random], array[i]) = (array[i], array[random]);
            }
        }
    }
}