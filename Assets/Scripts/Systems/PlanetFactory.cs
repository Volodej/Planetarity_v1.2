using System;
using System.Collections.Generic;
using System.Linq;
using Planetarity.Celestials;
using Planetarity.Models;
using Planetarity.Utils;
using UnityEngine;

namespace Planetarity.Systems
{
    public class PlanetFactory : MonoBehaviour
    {
        [SerializeField] private Transform _solarSystemRoot;
        [SerializeField] private Sun _sun;
        [SerializeField] private List<PlanetMover> _planetPrefabs;
        [SerializeField] private List<Palette> _palettes;

        public PlanetMover SpawnPlayerPlanet(PlanetState planetState)
        {
            var planetInstance = CreatePlanet(planetState);
            SetupPlayerPlanet(planetInstance);
            planetInstance.gameObject.name += "_Player";
            return planetInstance;
        }

        public PlanetMover SpawnEnemyPlanet(PlanetState planetState)
        {
            var planetInstance = CreatePlanet(planetState);
            SetupEnemyPlanet(planetInstance);
            return planetInstance;
        }

        private PlanetMover CreatePlanet(PlanetState planetState)
        {
            var random = new System.Random(planetState.RandomSeed);
            var planetPrefab = _planetPrefabs.RandomElement(random);
            var instance = Instantiate(planetPrefab, _solarSystemRoot);

            instance.Construct(planetState.OrbitalState, _sun);
            var planet = instance.GetComponent<Planet>();
            planet.Construct(planetState);
            
            var planetVisuals = instance.GetComponent<PlanetVisuals>();
            planetVisuals.SetColors(GetPlanetPalette(random));
            
            var planetHud = instance.GetComponent<PlanetHud>();
            planetHud.SetHealth(planetState.HealthState.CurrentHealth / planetState.HealthState.TotalHealth);

            var damageablePlanet = instance.GetComponent<DamageablePlanet>();
            damageablePlanet.Construct(planetState.HealthState);
            damageablePlanet.PlanetDestroyed += planetVisuals.MarkAsDead;
            damageablePlanet.PlanetDestroyed += planetHud.MarkAsDead;
            damageablePlanet.HealthPercentageChanged += planetHud.SetHealth;

            return instance;
        }

        private IReadOnlyList<Color> GetPlanetPalette(System.Random random)
        {
            var paletteCopy = _palettes.RandomElement(random).Colors.ToList();
            paletteCopy.Shuffle(random);
            return paletteCopy;
        }

        private void SetupEnemyPlanet(PlanetMover planetInstance)
        {
        }

        private void SetupPlayerPlanet(PlanetMover planetInstance)
        {
        }

        [Serializable]
        public class Palette
        {
            public List<Color> Colors;
        }
    }
}