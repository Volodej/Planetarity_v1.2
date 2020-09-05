using System;

namespace Planetarity.Models
{
    [Serializable]
    public class PlanetGenerationProperties
    {
        public float MinSize = 0.4f;
        public float MaxSize = 1f;
        public float GravitationalParameterPerUnitSize = 10;
        public float PlanetHealth = 100;
    }
}