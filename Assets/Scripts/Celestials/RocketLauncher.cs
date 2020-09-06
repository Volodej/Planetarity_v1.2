using System;
using Planetarity.Rockets;
using Planetarity.Utils;
using UnityEngine;

namespace Planetarity.Celestials
{
    public class RocketLauncher : MonoBehaviour
    {
        public event Action<float> CooldownTimeUpdated;
        public event Action<Rocket> RocketCreated;
        public event Action<Rocket> RocketDestroyed;

        [SerializeField] private float _startSpeed;
        [SerializeField] private float _cooldownTime;
        [SerializeField] private float _cooldownStart;

        private PlanetMover _planetMover;
        private ObjectsPool<Rocket> _rocketsPool;
        private Planet _planet;
        
        public RocketType RocketType { get; private set; }

        public float LeftCooldown => Mathf.Max(0, _cooldownStart + _cooldownTime - Time.time);
        public bool IsInCooldown => LeftCooldown > 0;

        public void Construct(Planet planet, ObjectsPool<Rocket> rocketsPool, RocketType rocketType, float leftCooldown)
        {
            _rocketsPool = rocketsPool;
            _cooldownStart = Time.time;
            _cooldownTime = leftCooldown;
            _planet = planet;
            RocketType = rocketType;
        }

        private void Start()
        {
            _planetMover = GetComponent<PlanetMover>();
        }

        public Rocket LaunchRocket(Vector2 direction)
        {
            if (IsInCooldown)
                throw new InvalidOperationException($"Can't launch rocket, system is in cooldown: {LeftCooldown}");

            var rocket = _rocketsPool.Borrow();
            PositionRocket(rocket, direction);
            var initialVelocity = _planetMover.Velocity + direction * _startSpeed;
            rocket.Launch(initialVelocity, _rocketsPool, _planet);
            _cooldownTime = rocket.CooldownTime;
            _cooldownStart = Time.time;

            rocket.Destroyed += () => RocketDestroyed?.Invoke(rocket);

            RocketCreated?.Invoke(rocket);

            return rocket;
        }

        private void PositionRocket(Rocket rocket, Vector2 direction)
        {
            var rocketTransform = rocket.transform;
            var planetTransform = transform;

            rocketTransform.parent = planetTransform.parent;
            rocketTransform.position = planetTransform.position + (Vector3) (direction * (_planet.Size + 0.5f));
            rocketTransform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        }

        private void Update()
        {
            CooldownTimeUpdated?.Invoke(LeftCooldown);
        }
    }
}