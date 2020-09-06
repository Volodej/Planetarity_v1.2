using System.Linq;
using Planetarity.Celestials;
using Planetarity.Models;

namespace Planetarity.Systems
{
    public class GameStateGrabber
    {
        public GameState GetFromSession(GameSession gameSession)
        {
            return new GameState
            {
                PlayerPlanet = GetStateFromPlanet(gameSession.PlayerPlanet),
                EnemyPlanets = gameSession.EnemyPlanets.Select(GetStateFromPlanet).ToList(),
                Rockets = gameSession.Rockets.Select(rocket => rocket.GetRocketState()).ToList()
            };
        }

        private PlanetState GetStateFromPlanet(PlanetMover planetMover)
        {
            var planetComponent = planetMover.GetComponent<Planet>();
            var damageablePlanet = planetMover.GetComponent<DamageablePlanet>();
            var rocketLauncher = planetMover.GetComponent<RocketLauncher>();

            var orbitalState = new OrbitalState
            {
                Radius = planetMover.Radius,
                PhasingFactor =planetMover.PhasingFactor,
                CurrentRotation = planetComponent.CurrentRotation,
                RotationPerSecond = planetComponent.RotationPerSecond
            };

            var planetProperties = new PlanetProperties
            {
                Size = planetComponent.Size,
                GravitationalParameter = planetComponent.GravitationalParameter
            };

            var healthState = new HealthState
            {
                CurrentHealth = damageablePlanet.CurrentHealth,
                TotalHealth = damageablePlanet.TotalHealth
            };

            var rocketLauncherState = new RocketLauncherState
            {
                RocketType = rocketLauncher.RocketType,
                LeftCooldown = rocketLauncher.LeftCooldown
            };

            var planetState = new PlanetState
            {
                ID = planetComponent.ID,
                OrbitalState = orbitalState,
                PlanetProperties = planetProperties,
                HealthState = healthState,
                RocketLauncherState = rocketLauncherState,
                RandomSeed = planetComponent.RandomSeed
            };

            return planetState;
        }
    }
}