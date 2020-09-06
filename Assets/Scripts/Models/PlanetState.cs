using System;

namespace Planetarity.Models
{
    [Serializable]
    public class PlanetState
    {
        public int RandomSeed;
        public string ID;
        public OrbitalState OrbitalState;
        public PlanetProperties PlanetProperties;
        public HealthState HealthState;
        public RocketLauncherState RocketLauncherState;
    }
}