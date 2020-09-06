using System;
using System.Collections.Generic;
using System.Linq;
using Planetarity.Celestials;
using Planetarity.Controllers;
using Planetarity.Models;
using Planetarity.Utils;
using UnityEngine;

namespace Planetarity.Systems
{
    public class PlanetFactory : MonoBehaviour
    {
        [SerializeField] private Transform _solarSystemRoot;
        [SerializeField] private Sun _sun;
        [SerializeField] private RocketPoolHolder _rocketPoolHolder;
        [SerializeField] private List<PlanetMover> _planetPrefabs;
        [SerializeField] private List<Palette> _palettes;

        [SerializeField] private EnemyControllerSettings _enemyControllerSettings;

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

            var healthState = planetState.HealthState;
            var planetHud = instance.GetComponent<PlanetHud>();
            planetHud.SetHealth(healthState.CurrentHealth / healthState.TotalHealth);

            var damageablePlanet = instance.GetComponent<DamageablePlanet>();
            damageablePlanet.Construct(healthState);
            damageablePlanet.PlanetDestroyed += planetVisuals.MarkAsDead;
            damageablePlanet.PlanetDestroyed += planetHud.MarkAsDead;
            damageablePlanet.HealthPercentageChanged += planetHud.SetHealth;

            var launcherState = planetState.RocketLauncherState;
            var rocketLauncher = instance.GetComponent<RocketLauncher>();
            rocketLauncher.Construct(planet, _rocketPoolHolder.GetPool(launcherState.RocketType), launcherState.LeftCooldown);
            rocketLauncher.CooldownTimeUpdated += planetHud.SetReloadTime;

            return instance;
        }

        private IReadOnlyList<Color> GetPlanetPalette(System.Random random)
        {
            var paletteCopy = _palettes.RandomElement(random).Colors.ToList();
            paletteCopy.Shuffle(random);
            return paletteCopy;
        }

        private void SetupPlayerPlanet(PlanetMover planetInstance)
        {
            planetInstance.GetComponent<PlanetHud>().SetupForPlayer();
            var rocketLauncher = planetInstance.GetComponent<RocketLauncher>();
            var damageablePlanet = planetInstance.GetComponent<DamageablePlanet>();
            var controller = PlayerPlanetController.AttachTo(rocketLauncher);
            damageablePlanet.PlanetDestroyed += () => controller.enabled = false;
        }

        private void SetupEnemyPlanet(PlanetMover planetInstance)
        {
            planetInstance.GetComponent<PlanetHud>().SetupForEnemy();
            var rocketLauncher = planetInstance.GetComponent<RocketLauncher>();
            var damageablePlanet = planetInstance.GetComponent<DamageablePlanet>();
            var controller = EnemyPlanetController.AttachTo(rocketLauncher, _enemyControllerSettings);
            damageablePlanet.PlanetDestroyed += () => controller.enabled = false;
        }

        [Serializable]
        public class Palette
        {
            public List<Color> Colors;
        }
    }
}