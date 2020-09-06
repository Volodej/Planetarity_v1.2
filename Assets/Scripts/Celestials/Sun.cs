using System;
using Planetarity.Models;
using Planetarity.Models.Interfaces;
using UnityEngine;

namespace Planetarity.Celestials
{
    public class Sun : MonoBehaviour, ICelestial, IHittable
    {
        [SerializeField] private float _gravitationalParameter;

        public float GravitationalParameter => _gravitationalParameter;
        public Vector3 Position => transform.position;

        public void Hit(float damage)
        {
            // Ignore damage, Sun don't care about rockets 
        }
    }
}