using System;
using Planetarity.Celestials;
using Planetarity.Rockets;
using UnityEngine;

namespace Planetarity.Controllers
{
    public abstract class BasePlanetController : MonoBehaviour
    {
        protected RocketLauncher _rocketLauncher;
        private Rocket _controlledRocket;

        private PlanetControllerState State
        {
            get
            {
                if (enabled == false)
                    return PlanetControllerState.Disabled;
                if (_controlledRocket != null)
                    return PlanetControllerState.ControlRocket;
                return PlanetControllerState.Idle;
            }
        }

        public void SetControlledRocket(Rocket rocket)
        {
            _controlledRocket = rocket;
            _controlledRocket.Destroyed += () => _controlledRocket = null;
        }

        protected virtual void Update()
        {
            switch (State)
            {
                case PlanetControllerState.Idle:
                    LaunchRocket();
                    break;
                case PlanetControllerState.ControlRocket:
                    SteerRocket();
                    break;
                case PlanetControllerState.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(State), $"Unknown value '{State}'.");
            }
        }

        protected abstract Vector2? GetTarget();

        private void LaunchRocket()
        {
            if (_rocketLauncher.IsInCooldown)
                return;

            var targetPosition = GetTarget();
            if (targetPosition == null)
                return;

            var launchDirection = (targetPosition.Value - (Vector2) transform.position).normalized;
            _controlledRocket = _rocketLauncher.LaunchRocket(launchDirection);
            _controlledRocket.Destroyed += () => _controlledRocket = null;
        }

        private void SteerRocket()
        {
            var targetPosition = GetTarget();
            if (targetPosition != null)
                _controlledRocket.SetSteeringTarget(targetPosition.Value);
        }
    }
}