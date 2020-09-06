using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Planetarity.Celestials;
using Planetarity.Controllers;
using Planetarity.Models;
using Planetarity.Models.Interfaces;
using Planetarity.Rockets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Planetarity.Systems
{
    public class GameSession
    {
        private readonly TaskCompletionSource<bool> _cancelTcs;
        private IReadOnlyCollection<ICelestial> _allCelestials;

        public GameSession(Sun sun, PlanetMover playerPlanet, IReadOnlyList<PlanetMover> enemyPlanets, HashSet<Rocket> rockets)
        {
            PlayerPlanet = playerPlanet;
            EnemyPlanets = enemyPlanets;
            Rockets = rockets;
            _cancelTcs = new TaskCompletionSource<bool>();
            _allCelestials = enemyPlanets.Append(playerPlanet)
                .Select(mover => (ICelestial) mover.GetComponent<Planet>())
                .Append(sun)
                .ToList();
        }
        

        public bool IsActive { get; private set; }
        public PlanetMover PlayerPlanet { get; }
        public IReadOnlyList<PlanetMover> EnemyPlanets { get; }
        public HashSet<Rocket> Rockets { get; } 

        public async Task<GameResult> Play()
        {
            IsActive = true;
            SubscribeToRocketsCreation();
            
            var playerDestroyed = GetDestroyedTask(PlayerPlanet.gameObject);
            var allEnemiesDestroyed = Task.WhenAll(EnemyPlanets.Select(mover => GetDestroyedTask(mover.gameObject)));
            var canceledTask = _cancelTcs.Task;
            
            foreach (var enemyPlanet in EnemyPlanets)
            {
                enemyPlanet.GetComponent<EnemyPlanetController>().SetTarget(PlayerPlanet.transform);
            }

            await Task.WhenAny(playerDestroyed, allEnemiesDestroyed);
            
            IsActive = false;

            if (playerDestroyed.IsCompleted)
                return GameResult.Defeat;

            if (allEnemiesDestroyed.IsCompleted)
                return GameResult.Victory;

            if (canceledTask.IsCompleted)
                return GameResult.Stopped;
            
            throw new InvalidOperationException("None of tasks are finished, it's a bug.");
        }

        public void Stop()
        {
            _cancelTcs.TrySetResult(true);
            Object.Destroy(PlayerPlanet.gameObject);
            foreach (var enemyPlanet in EnemyPlanets)
            {
                Object.Destroy(enemyPlanet.gameObject);
            }
        }

        private Task GetDestroyedTask(GameObject planet)
        {
            var tcs = new TaskCompletionSource<bool>();
            planet.GetComponent<DamageablePlanet>().PlanetDestroyed += () => tcs.TrySetResult(true);
            return tcs.Task;
        }

        private void SubscribeToRocketsCreation()
        {
            var rocketLaunchers = EnemyPlanets.Append(PlayerPlanet)
                .Select(mover => mover.GetComponent<RocketLauncher>());
            foreach (var rocketLauncher in rocketLaunchers)
            {
                rocketLauncher.RocketCreated += rocket =>
                {
                    rocket.SetCelestials(_allCelestials);
                    Rockets.Add(rocket);
                };
                rocketLauncher.RocketDestroyed += rocket => Rockets.Remove(rocket);
            }
        }
    }
}