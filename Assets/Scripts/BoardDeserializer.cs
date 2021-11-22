using System;
using System.IO;
using TheseusAndMinotaur.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur
{
    public static class BoardDeserializer
    {
        private const char HorizontalWallChar = '_';
        private const char VerticalWallChar = '|';
        private const char TheseusStartPoint = 'T';
        private const char MinotaurStartPoint = 'M';
        private const char Exit = 'E';
        
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


            if (string.IsNullOrEmpty(textData))
            {
                throw new ParseMazeException("string is empty or null, you must provide something");
            }
            var reader = new StringReader(textData);
            var line = reader.ReadLine();
            
            var rawBoard = new RawBoard();
            int y = 0; // get 2 lines from source file and merge them in to the one
            
            while (line != null)
            {
                // check top wall from current line
                for (int x = 1; x < line.Length; x += 2)
                {
                    if (line[x] == HorizontalWallChar)
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
                    for (int x = 0; x < line.Length; x ++)
                    {
                        if (line[x] == VerticalWallChar)
                        {
                            rawBoard[y, x / 2] |= Direction.Left;
                        }

                        if (++x < line.Length)
                        {
                            var nextChar = line[x];
                            var boardPosition = new Vector2Int(x / 2, y);
                            switch (nextChar)
                            {
                                case MinotaurStartPoint:
                                    rawBoard.MinotaurStartPosition.Value = boardPosition;
                                    break;
                                case TheseusStartPoint:
                                    rawBoard.TheseusStartPosition.Value = boardPosition;
                                    break;
                                case Exit:
                                    rawBoard.ExitPosition.Value = boardPosition;
                                    break;
                            }
                        } 
                    }
                }
                y++;
                line = reader.ReadLine(); 
            }
            
            return rawBoard.ConvertToBoard();
        }
    }
}