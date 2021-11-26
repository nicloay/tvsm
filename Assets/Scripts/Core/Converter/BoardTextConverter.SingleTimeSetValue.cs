using System;

namespace TheseusAndMinotaur.Data.Converter
{
    public static partial class BoardTextConverter
    {
        /// <summary>
        ///     Holder for the value which can be set only one single time
        /// </summary>
        internal class SingleTimeSetValue<T>
        {
            private T _value;
            public bool IsValueSet { get; private set; }

            public T Value
            {
                get => _value;
                set
                {
                    if (IsValueSet)
                    {
                        throw new Exception("Value can be set only single time");
                    }

                    IsValueSet = true;
                    _value = value;
                }
            }
        }
    }
}