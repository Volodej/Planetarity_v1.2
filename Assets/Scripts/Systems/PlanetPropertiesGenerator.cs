using System;
using System.Collections.Generic;
using System.Linq;
using Planetarity.Models;
using Planetarity.Rockets;
using Planetarity.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Planetarity.Systems
{
    public class PlanetPropertiesGenerator : MonoBehaviour
    {
        private static List<RocketType> RocketTypes = Enum.GetValues(typeof(RocketType)).Cast<RocketType>().ToList();
        
        [SerializeField, Range(3, 30)]
        private float _minRadius = 5;

        [SerializeField, Range(3, 30)]
        private float _maxRadius = 20;

        [SerializeField, Range(3, 10)]
        private int _minPlanetsCount = 5;

        [SerializeField, Range(3, 10)]
        private int _maxPlanetsCount = 7;

        [SerializeField] private PlanetGenerationProperties _planetGenerationProperties;

        public GameState GenerateGameState()
        {
            var planetsCount = Random.Range(_minPlanetsCount, _maxPlanetsCount + 1);
            var spaceBetweenPlanets = _planetGenerationProperties.MaxSize;

            var totalAvailableSpace = _maxRadius - _minRadius;
            var spacePerPlanet = (totalAvailableSpace - spaceBetweenPlanets * (planetsCount - 1)) / planetsCount;
            if (spacePerPlanet <= 0)
            {
                throw new InvalidOperationException("Not enough space for planets, try different orbit or planet size settings.\n" +
                                                    $"Available space: {totalAvailableSpace}, planets: {planetsCount}.");
            }

            var playerIndex = Random.Range(0, planetsCount);
            var enemyPlanters = Enumerable.Range(0, planetsCount)
                .Where(index => index != playerIndex)
                .Select(index => GetRangeFromIndex(index, spacePerPlanet, spacePerPlanet))
                .Select(GeneratePlanetState)
                .ToList();
            var playerPlanet = GeneratePlanetState(GetRangeFromIndex(playerIndex, spacePerPlanet, spacePerPlanet));

            return new GameState
            {
                PlayerPlanet = playerPlanet,
                EnemyPlanets = enemyPlanters
            };
        }

        private PlanetState GeneratePlanetState(Range orbitRange)
        {
            var orbitalState = new OrbitalState
            {
                Radius = Random.Range(orbitRange.Min, orbitRange.Max),
                CurrentRotation = Random.rotation,
                RotationPerSecond = Quaternion.Euler(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-60f, 60f)),
                PhasingFactor = Random.value
            };

            var planetSize = Random.Range(_planetGenerationProperties.MinSize, _planetGenerationProperties.MaxSize);
            var planetProperties = new PlanetProperties
            {
                Size = planetSize,
                GravitationalParameter = _planetGenerationProperties.GravitationalParameterPerUnitSize * planetSize
            };

            var healthState = new HealthState
            {
                CurrentHealth = _planetGenerationProperties.PlanetHealth,
                TotalHealth = _planetGenerationProperties.PlanetHealth
            };

            var rocketLauncherState = new RocketLauncherState
            {
                LeftCooldown = 0,
                RocketType = RocketTypes.RandomElement()
            };

            return new PlanetState
            {
                ID = Guid.NewGuid().ToString(),
                RandomSeed = Random.Range(int.MinValue, int.MaxValue),
                OrbitalState = orbitalState,
                PlanetProperties = planetProperties,
                HealthState = healthState,
                RocketLauncherState = rocketLauncherState
            };
        }

        private Range GetRangeFromIndex(int index, float spacePerPlanet, float spaceBetweenPlants)
        {
            var min = _minRadius + (spacePerPlanet + spaceBetweenPlants) * index;
            var max = _minRadius + spacePerPlanet * (index + 1) + spaceBetweenPlants * index;
            return new Range(min, max);
        }
    }
}