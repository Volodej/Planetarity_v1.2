using System;
using Planetarity.Rockets;
using UnityEngine;

namespace Planetarity.Models
{
    [Serializable]
    public class RocketState
    {
        public RocketType RocketType;
        public string PlanetID;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public Vector3 Position;
        public Quaternion Rotation;
        public float LeftLifetime;
    }
}