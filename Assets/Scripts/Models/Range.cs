using System;

namespace Planetarity.Models
{
    public struct Range
    {
        public float Min { get; }
        public float Max { get; }
        

        public Range(float min, float max)
        {
            if (min > max)
                throw new ArgumentException($"Min value '{min}' can't be bigger than max value '{max}'");
            
            Min = min;
            Max = max;
        }
    }
}