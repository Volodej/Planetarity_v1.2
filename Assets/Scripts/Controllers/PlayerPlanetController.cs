using Planetarity.Celestials;
using UnityEngine;

namespace Planetarity.Controllers
{
    public class PlayerPlanetController : BasePlanetController
    {
        private Camera _camera;

        private void Awake()
        {
        }

        public static PlayerPlanetController AttachTo(RocketLauncher rocketLauncher)
        {
            var controller = rocketLauncher.gameObject.AddComponent<PlayerPlanetController>();
            controller._rocketLauncher = rocketLauncher;
            controller._camera = Camera.main;
            return controller;
        }
        
        protected override Vector2? GetTarget()
        {
            if (Input.GetMouseButton(0) == false)
                return null;

            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }

    }
}