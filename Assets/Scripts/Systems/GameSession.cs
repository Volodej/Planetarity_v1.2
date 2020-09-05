using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Planetarity.Celestials;
using Planetarity.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Planetarity.Systems
{
    public class GameSession
    {
        private readonly TaskCompletionSource<bool> _cancelTcs;

        public GameSession(PlanetMover playerPlanet, List<PlanetMover> enemyPlanets)
        {
            PlayerPlanet = playerPlanet;
            EnemyPlanets = enemyPlanets;
            _cancelTcs = new TaskCompletionSource<bool>();
        }

        public bool IsActive { get; private set; }
        public PlanetMover PlayerPlanet { get; }
        public List<PlanetMover> EnemyPlanets { get;  }

        public async Task<GameResult> Play()
        {
            IsActive = true;
            var playerDestroyed = GetDestroyedTask(PlayerPlanet.gameObject);
            var allEnemiesDestroyed = Task.WhenAll(EnemyPlanets.Select(mover => GetDestroyedTask(mover.gameObject)));
            var canceledTask = _cancelTcs.Task;

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
    }
}