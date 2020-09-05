using System.Collections.Generic;

namespace Planetarity.Models
{
    public class GameState
    {
        public PlanetState PlayerPlanet;
        public List<PlanetState> EnemyPlanets;
    }
}