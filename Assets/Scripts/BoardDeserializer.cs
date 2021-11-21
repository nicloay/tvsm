using System.IO;
using TheseusAndMinotaur.Data;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur
{
    public static class BoardDeserializer
    {
        /// <summary>
        /// Source format contains switching line one after another in the following way (first column wall is skipped)
        ///     ._._._ // which is for Bottom Walls
        ///     |   |  // for left walls
        ///     ._._._ // again walls and so on
        /// </summary>
        private enum RowType
        {
            BottomWall,
            LeftWall
        }

        public static Board DeserializeFrom(string textData)
        {
            const char horizontalWallChar = '_';
            const char verticalWallChar = '|';

            Assert.IsFalse(string.IsNullOrEmpty(textData));
            var reader = new StringReader(textData);
            var line = reader.ReadLine();


            var rawBoard = new RawBoard();
            int y = 0; // get 2 lines from source file and merge them in to the one
            
            while (line != null)
            {
                // check top wall from current line
                for (int x = 1; x < line.Length; x += 2)
                {
                    if (line[x] == horizontalWallChar)
                    {
                        rawBoard[y, x / 2] |= Direction.Top;
                    }
                }

                // Check side walls on next line (but it can be null for the last line
                line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                else
                {
                    for (int x = 0; x < line.Length; x += 2) // TODO: implement start points and exit here
                    {
                        if (line[x] == verticalWallChar)
                        {
                            rawBoard[y, x / 2] |= Direction.Left;
                        }
                    }
                }
                y++;
                line = reader.ReadLine(); // TODO: error handling
            }

            return rawBoard.ConvertToBoard();
        }
    }
}