using System;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    /// Holder for the value which can be set only one single time
    /// </summary>
    public class SingleTimeSetValue<T>
    {
        public bool IsValueSet { get; private set; }
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (IsValueSet)
                {
                    throw new Exception("Value can be set only single time");
                }
                else
                {
                    IsValueSet = true;
                    _value = value;
                }
            }
        }
    }
}