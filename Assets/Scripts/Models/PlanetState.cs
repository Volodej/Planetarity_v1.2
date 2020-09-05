using System;

namespace Planetarity.Models
{
    [Serializable]
    public class PlanetState
    {
        public int RandomSeed;
        public OrbitalState OrbitalState;
        public PlanetProperties PlanetProperties;
        public HealthState HealthState;
    }
}