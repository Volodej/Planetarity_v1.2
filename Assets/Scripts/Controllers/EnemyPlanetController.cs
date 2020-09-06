using Planetarity.Celestials;
using Planetarity.Models;
using UnityEngine;

namespace Planetarity.Controllers
{
    public class EnemyPlanetController : BasePlanetController
    {
        private Vector2 _targetPosition;
        private Transform _target;
        private float _lastRandomizationTime;
        private EnemyControllerSettings _enemyControllerSettings;

        public static EnemyPlanetController AttachTo(RocketLauncher rocketLauncher, EnemyControllerSettings enemyControllerSettings)
        {
            var controller = rocketLauncher.gameObject.AddComponent<EnemyPlanetController>();
            controller._rocketLauncher = rocketLauncher;
            controller._enemyControllerSettings = enemyControllerSettings;
            return controller;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        protected override Vector2? GetTarget()
        {
            return _target != null ? _targetPosition : (Vector2?) null;
        }

        protected override void Update()
        {
            base.Update();

            if (_lastRandomizationTime + _enemyControllerSettings.TargetSelectionFrequency < Time.time)
            {
                _targetPosition = new Vector2(RandomShiftValue(), RandomShiftValue()) + (Vector2) _target.position;
                _lastRandomizationTime = Time.time;
            }
        }

        private float RandomShiftValue() =>
            Random.Range(-_enemyControllerSettings.EnemyTargetShift, _enemyControllerSettings.EnemyTargetShift);
    }
}