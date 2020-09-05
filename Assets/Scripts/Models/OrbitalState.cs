using System;
using UnityEngine;

namespace Planetarity.Models
{
    [Serializable]
    public class OrbitalState
    {
        public float Radius;
        public float PhasingFactor;
        public Quaternion CurrentRotation;
        public Quaternion RotationPerSecond;
    }
}