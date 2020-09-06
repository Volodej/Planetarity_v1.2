using System.Collections.Generic;
using System.Linq;
using Planetarity.Rockets;
using Planetarity.Utils;
using UnityEngine;

namespace Planetarity.Systems
{
    public class RocketPoolHolder : MonoBehaviour
    {
        [SerializeField] private Transform _orphansRoot;
        [SerializeField] private List<Rocket> _rocketPrefabs;

        private readonly Dictionary<RocketType, ObjectsPool<Rocket>> RocketPools = new Dictionary<RocketType, ObjectsPool<Rocket>>(3);

        private void Awake()
        {
            foreach (var rocketPrefab in _rocketPrefabs)
            {
                var pool = new ObjectsPool<Rocket>(
                    () => Instantiate(rocketPrefab),
                    rocket => rocket.Reset(),
                    _orphansRoot);
                RocketPools.Add(rocketPrefab.RocketType, pool);
            }
        }

        public ObjectsPool<Rocket> GetPool(RocketType type) => RocketPools[type];
    }
}