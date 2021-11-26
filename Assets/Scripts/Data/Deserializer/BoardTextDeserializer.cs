using System.IO;
using System.Text;
using TheseusAndMinotaur.Utils;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Deserializer
{
    public static partial class BoardTextDeserializer
    {
        private const char HorizontalWallChar = '_';
        private const char VerticalWallChar = '|';
        private const char TheseusStartPoint = 'T';
        private const char MinotaurStartPoint = 'M';
        private const char Exit = 'E';

        /// <summary>
        ///     Deserialize from streaming asset file
        /// </summary>
        /// <param name="relativePath">file path, relative to streaming assets folder</param>
        /// <returns>board</returns>
        public static BoardConfig DeserializeFromStreamingAssets(string relativePath)
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            var data = File.ReadAllText(fullPath);
            return DeserializeFrom(data);
        }


        /// <summary>
        /// Generate random board with size
        /// horizontal WallsNumber will be spawned on random position on the cell, the same vertical number,
        ///
        /// exit/Thesaus/Minotaur positions will be spawned randomly
        /// </summary>
        /// <param name="size"></param>
        /// <param name="horizontalWallsNumber"></param>
        /// <param name="verticalWallsNumber"></param>
        /// <returns></returns>
        public static BoardConfig GenerateRandom(Vector2Int size, int horizontalWallsNumber, int verticalWallsNumber)
        {
            var map = new Direction[size.y, size.x];

            var randomMap = ArrayUtils.GetOrderedSequence(size.x * size.y);
            randomMap.ShuffleArray();

            for (int i = 0; i < horizontalWallsNumber; i++)
            {
                var randomId = randomMap[i];
                var position = randomId.Get2DCoordinates(size.x);
                map[position.y, position.x] |= Direction.Left;
            }
            
            randomMap.ShuffleArray();
            for (int i = 0; i < verticalWallsNumber; i++)
            {
                var randomId = randomMap[i];
                var position = randomId.Get2DCoordinates(size.x);
                map[position.y, position.x] |= Direction.Up;
            }
            
            randomMap.ShuffleArray();
            
            RawBoard.FillNeighbourWalls(map);
            
            return new BoardConfig(map,
                randomMap[0].Get2DCoordinates(size.x),
                randomMap[1].Get2DCoordinates(size.x),
                randomMap[2].Get2DCoordinates(size.x));
            
        }

        /// <summary>
        /// Convert board config to the text file
        /// </summary>
        public static string ConvertToTextConfig(this BoardConfig config)
        {
            var result = new StringBuilder();
            var top = new StringBuilder();
            var left = new StringBuilder();
            for (int y = config.Height -1 ; y >=0 ; y--)
            {
                top.Clear();
                left.Clear();
                for (int x = 0; x < config.Width; x++)
                {
                    var direction = config[y, x];
                    top.Append(".");
                    top.Append(direction.HasTop() ? HorizontalWallChar : " ");
                    left.Append(direction.HasLeft() ? VerticalWallChar : " ");
                    left.Append(" ");
                }

                if (y == config.MinotaurStartPosition.y)
                {
                    left[1 + config.MinotaurStartPosition.x * 2] = MinotaurStartPoint;
                }
                
                if (y == config.TheseusStartPosition.y)
                {
                    left[1 + config.TheseusStartPosition.x * 2] = TheseusStartPoint;
                }
                
                if (y == config.Exit.y)
                {
                    left[1 + config.Exit.x * 2] = Exit;
                }
                
                result.AppendLine(top.ToString());
                result.AppendLine(left.ToString());
            }

            return result.ToString();
        }
        
        /// <summary>
        ///     Deserialize from text Data <see cref="Example.txt"/>
        /// </summary>
        /// <param name="textData">board config in text format <see cref="Example.txt"/> at StreamingAsset folder</param>
        /// <returns>board</returns>
        public static BoardConfig DeserializeFrom(string textData)
        {
            if (string.IsNullOrEmpty(textData))
            {
                throw new ParseMazeException("string is empty or null, you must provide something");
            }

            var reader = new StringReader(textData);
            var line = reader.ReadLine();

            var rawBoard = new RawBoard();
            var y = 0; // get 2 lines from source file and merge them in to the one

            while (line != null)
            {
                // check top wall from current line
                for (var x = 1; x < line.Length; x += 2)
                {
                    if (line[x] == HorizontalWallChar)
                    {
                        rawBoard[y, x / 2] |= Direction.Up;
                    }
                }

                // Check side walls on next line (but it can be null for the last line
                line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                for (var x = 0; x < line.Length; x++)
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

                rawBoard[rawBoard.MinotaurStartPosition.Value] |= Direction.None;
                rawBoard[rawBoard.TheseusStartPosition.Value] |= Direction.None;
                rawBoard[rawBoard.ExitPosition.Value] |= Direction.None;
                
                y++;
                line = reader.ReadLine();
            }

            return rawBoard.ConvertToBoard();
        }
    }
}