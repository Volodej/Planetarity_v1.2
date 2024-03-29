using System;
using Planetarity.Models;
using Planetarity.Models.Interfaces;
using UnityEngine;

namespace Planetarity.Celestials
{
    public class DamageablePlanet : MonoBehaviour, IHittable
    {
        public event Action<float> HealthPercentageChanged;
        public event Action PlanetDestroyed;

        [SerializeField] private float _totalHealth;
        [SerializeField] private float _currentHealth;

        private PlanetVisuals _planetVisuals;

        public float TotalHealth => _totalHealth;
        public float CurrentHealth => _currentHealth;

        public void Construct(HealthState healthState)
        {
            _totalHealth = healthState.TotalHealth;
            _currentHealth = healthState.CurrentHealth;
        }

        private void Start()
        {
            if (_currentHealth <= 0)
                KillPlanet();
        }

        public void Hit(float damage)
        {
            _currentHealth -= damage;
            HealthPercentageChanged?.Invoke(_currentHealth / _totalHealth);
            if (_currentHealth <= 0)
                KillPlanet();
        }

        private void KillPlanet()
        {
            PlanetDestroyed?.Invoke();
        }
    }
}