using System;
using Planetarity.Models;
using UnityEngine;

namespace Planetarity.Celestials
{
    public class Sun : MonoBehaviour
    {
        [SerializeField] private float _gravitationalParameter ;
        
        public float GravitationalParameter => _gravitationalParameter;
    }
}