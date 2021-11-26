using System;

namespace TheseusAndMinotaur.Data.Converter
{
    public class ParseMazeException : Exception
    {
        public ParseMazeException(string message) : base(message)
        {
        }
    }
}