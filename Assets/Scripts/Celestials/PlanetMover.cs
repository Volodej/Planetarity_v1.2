using Planetarity.Models;
using UnityEngine;

namespace Planetarity.Celestials
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetMover : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private float _orbitalPeriod;

        public float Radius { get; private set; }
        public float PhasingFactor { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Construct(OrbitalState orbitalState, Sun sun)
        {
            Radius = orbitalState.Radius;
            PhasingFactor = orbitalState.PhasingFactor;
            _orbitalPeriod = CalculateOrbitalPeriod(Radius, sun);
            _rigidbody.position = GetOrbitalPosition(PhasingFactor);
        }

        private void FixedUpdate()
        {
            PhasingFactor = GetNewOrbitalPhasing(Time.fixedDeltaTime);
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

        private float CalculateOrbitalPeriod(float radius, Sun sun)
        {
            // T = 2 * PI * sqrt (r^3 / GM)
            return 2 * Mathf.PI * Mathf.Sqrt(radius * radius * radius / sun.GravitationalParameter);
        }
    }
}