using System;
using System.Collections.Generic;
using Planetarity.Celestials;
using Planetarity.Models;
using Planetarity.Models.Interfaces;
using Planetarity.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Planetarity.Rockets
{
    [RequireComponent(typeof(Rigidbody))]
    public class Rocket : MonoBehaviour, IHittable
    {
        public event Action Destroyed;

        [SerializeField] private RocketType _rocketType;
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _cooldownTime = 10;
        [SerializeField] private float _acceleration = 0.5f;
        [SerializeField] private float _steerAngleSpeed = 30;
        [SerializeField] private float _gravityStrengthMultiplier = 2;
        [SerializeField] private float _operationalRadius = 15; // how far from sun can this rocket go
        [SerializeField] private float _rocketLifetime = 15;

        private Rigidbody _rigidbody;
        private ObjectsPool<Rocket> _pool;
        private Vector2? _steeringTarget;
        private IReadOnlyCollection<ICelestial> _celestials;
        private float _timeOfLunch;

        public RocketType RocketType => _rocketType;
        public float CooldownTime => _cooldownTime;
        public Planet HomePlanet { get; private set; }

        public RocketState GetRocketState()
        {
            return new RocketState
            {
                RocketType = _rocketType,
                PlanetID = HomePlanet.ID,
                Position = _rigidbody.position,
                Rotation = _rigidbody.rotation,
                Velocity = _rigidbody.velocity,
                AngularVelocity = _rigidbody.angularVelocity,
                LeftLifetime = _rocketLifetime - (Time.time -_timeOfLunch)
            };
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Launch(Vector2 initialVelocity, ObjectsPool<Rocket> rocketsPool, Planet planet)
        {
            _rigidbody.velocity = initialVelocity;
            _rigidbody.angularVelocity = Vector3.zero;
            _pool = rocketsPool;
            HomePlanet = planet;
            _timeOfLunch = Time.time;
            gameObject.SetActive(true);
        }

        public void Spawn(RocketState state, ObjectsPool<Rocket> rocketsPool, Planet planet)
        {
            var t = transform;
            t.position = state.Position;
            t.rotation = state.Rotation;
            _rigidbody.velocity = state.Velocity;
            _rigidbody.angularVelocity = state.AngularVelocity;
            _pool = rocketsPool;
            HomePlanet = planet;
            _timeOfLunch = Time.time - (_rocketLifetime - state.LeftLifetime);
            t.parent = planet.transform.parent;
            gameObject.SetActive(true);
        }

        public void SetCelestials(IReadOnlyCollection<ICelestial> celestials)
        {
            _celestials = celestials;
        }

        public void Reset()
        {
            _pool = null;
            _steeringTarget = null;
            _celestials = null;
            HomePlanet = null;
            Destroyed = null;
        }

        public void SetSteeringTarget(Vector2? clickPosition) => _steeringTarget = clickPosition - transform.position;

        public void DestroyRocket()
        {
            Destroyed?.Invoke();
            _pool?.Release(this);
        }

        private void Update()
        {
            if (transform.position.magnitude > _operationalRadius || _rocketLifetime + _timeOfLunch < Time.time)
                DestroyRocket();
        }

        private void FixedUpdate()
        {
            if (_steeringTarget != null)
                SteerTowards(_steeringTarget.Value);

            _rigidbody.velocity += transform.up * (Time.fixedDeltaTime * _acceleration);
            AddGravityForce();
        }

        private void OnCollisionEnter(Collision other)
        {
            var hittable = other.gameObject.GetComponent<IHittable>();
            if (hittable == null)
                return;

            hittable.Hit(_damage);
            DestroyRocket();
        }

        public void Hit(float _)
        {
            DestroyRocket();
        }

        private void SteerTowards(Vector2 directionVector)
        {
            var t = transform;
            var angleToPosition = Vector2.SignedAngle(t.up, directionVector);
            var rotationAngle = Mathf.Min(_steerAngleSpeed * Time.fixedDeltaTime, Mathf.Abs(angleToPosition)) * Mathf.Sign(angleToPosition);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(new Vector3(0, 0, rotationAngle)));
        }

        private void AddGravityForce()
        {
            foreach (var celestial in _celestials)
            {
                var toCelestialVector = celestial.Position - transform.position;
                var direction = toCelestialVector.normalized;
                var force = celestial.GravitationalParameter * _gravityStrengthMultiplier / toCelestialVector.sqrMagnitude;
                _rigidbody.AddForce(direction * force);
            }
        }
    }
}