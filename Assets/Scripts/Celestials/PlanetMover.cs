using Planetarity.Models;
using UnityEngine;

namespace Planetarity.Celestials
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetMover : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField] private float _orbitalPeriod;
        [SerializeField] private Sun _sun;
        [SerializeField] private float _radius;
        [SerializeField] private float _phasingFactor;

        public float Radius => _radius;

        public float PhasingFactor => _phasingFactor;

        public Vector2 Velocity
        {
            get
            {
                var speed = Mathf.Sqrt(_sun.GravitationalParameter / Radius);
                var angle = 2 * Mathf.PI * PhasingFactor;
                var x = speed * Mathf.Sin(angle);
                var y = speed * Mathf.Cos(angle);
                return new Vector2(x, y);
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Construct(OrbitalState orbitalState, Sun sun)
        {
            _sun = sun;
            _radius = orbitalState.Radius;
            _phasingFactor = orbitalState.PhasingFactor;
            _orbitalPeriod = CalculateOrbitalPeriod(Radius);
            _rigidbody.position = GetOrbitalPosition(PhasingFactor);
        }

        private void FixedUpdate()
        {
            _phasingFactor = GetNewOrbitalPhasing(Time.fixedDeltaTime);
            var newPosition = GetOrbitalPosition(PhasingFactor);
            _rigidbody.MovePosition(newPosition);
        }

        private float GetNewOrbitalPhasing(float deltaTime)
        {
            var phasingFactor = PhasingFactor + deltaTime / _orbitalPeriod;
            return phasingFactor > 1 ? phasingFactor - Mathf.Floor(phasingFactor) : phasingFactor;
        }

        private Vector3 GetOrbitalPosition(float phasingFactor)
        {
            var angle = 2 * Mathf.PI * phasingFactor;
            var x = Radius * Mathf.Cos(angle);
            var y = Radius * Mathf.Sin(angle);
            return new Vector3(x, y, 0);
        }

        private float CalculateOrbitalPeriod(float radius)
        {
            // T = 2 * PI * sqrt (r^3 / GM)
            return 2 * Mathf.PI * Mathf.Sqrt(radius * radius * radius / _sun.GravitationalParameter);
        }
    }
}