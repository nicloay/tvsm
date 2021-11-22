using System;

namespace TheseusAndMinotaur.Data
{
    public class ParseMazeException : Exception
    {
        public ParseMazeException(string message) : base(message)
        {
        }
    }
}