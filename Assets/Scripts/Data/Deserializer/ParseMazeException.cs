using System;

namespace TheseusAndMinotaur.Data.Deserializer
{
    public class ParseMazeException : Exception
    {
        public ParseMazeException(string message) : base(message)
        {
        }
    }
}