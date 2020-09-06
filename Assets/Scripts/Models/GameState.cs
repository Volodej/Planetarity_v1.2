using System.Collections.Generic;

namespace Planetarity.Models
{
    public class GameState
    {
        public PlanetState PlayerPlanet;
        public List<PlanetState> EnemyPlanets;
        public List<RocketState> Rockets;
    }
}