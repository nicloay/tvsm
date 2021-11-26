using System.IO;
using System.Text;
using UnityEngine;

namespace TheseusAndMinotaur.Data.Converter
{
    /// <summary>
    /// Convert text in to the board configuration and back
    /// </summary>
    public static partial class BoardTextConverter
    {
        private const char HorizontalWallChar = '_';
        private const char VerticalWallChar = '|';
        private const char TheseusStartPoint = 'T';
        private const char MinotaurStartPoint = 'M';
        private const char Exit = 'E';
        
        /// <summary>
        ///     Get resource content from the resource
        /// </summary>
        public static string GetResourceContent(this string resourcePath)
        {
            var textAsset = Resources.Load<TextAsset>(resourcePath);
            var data = textAsset.text;
            Resources.UnloadAsset(textAsset);
            return data;
        }

        /// <summary>
        ///     Get level name from resource path
        /// </summary>
        public static string GetLevelName(this string resourcePath)
        {
            return Path.GetFileNameWithoutExtension(resourcePath);
        }

        /// <summary>
        ///     Convert board config to the text file
        /// </summary>
        public static string ConvertToTextConfig(this BoardConfig config)
        {
            var result = new StringBuilder();
            var top = new StringBuilder();
            var left = new StringBuilder();
            for (var y = config.Height - 1; y >= 0; y--)
            {
                top.Clear();
                left.Clear();
                for (var x = 0; x < config.Width; x++)
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
        ///     Deserialize from text />
        /// </summary>
        /// <param name="textData">board config in text format Example.txt at StreamingAssets folder</param>
        /// <returns>board</returns>
        public static BoardConfig ToBoardConfig(this string textData)
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