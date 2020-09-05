using Planetarity.Models;
using UnityEngine;

namespace Planetarity.Celestials
{
    public class Planet : MonoBehaviour
    {
        [SerializeField] private Transform _planetModel;
        [SerializeField] private float _size;
        [SerializeField] private float _gravitationalParameter;

        public Quaternion CurrentRotation => _planetModel.localRotation;
        public Quaternion RotationPerSecond { get; private set; }

        public float Size => _size;
        public float GravitationalParameter => _gravitationalParameter;
        public int RandomSeed { get; private set; }

        public void Construct(PlanetState planetState)
        {
            _size = planetState.PlanetProperties.Size;
            _gravitationalParameter = planetState.PlanetProperties.GravitationalParameter;
            transform.localScale = Vector3.one * _size;

            _planetModel.localRotation = planetState.OrbitalState.CurrentRotation;
            RotationPerSecond = planetState.OrbitalState.RotationPerSecond;

            RandomSeed = planetState.RandomSeed;
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            var frameRotation = Quaternion.Lerp(Quaternion.identity, RotationPerSecond, Time.deltaTime);
            _planetModel.localRotation *= frameRotation;
        }
    }
}