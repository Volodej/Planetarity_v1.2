using Cinemachine;
using UnityEngine;

namespace Planetarity.Cameras
{
    public class CamerasController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _planetCamera;
        [SerializeField] private CinemachineVirtualCamera _rocketCamera;
        
        //public void 
        public void SwitchToPlanet(GameObject playerPlanet)
        {
            _rocketCamera.Follow = null;
            _planetCamera.Follow = playerPlanet.transform;
            _planetCamera.gameObject.SetActive(true);
            _rocketCamera.gameObject.SetActive(false);
        }

        public void SwitchToRocket(GameObject rocket)
        {
            _rocketCamera.Follow = rocket.transform;
            _rocketCamera.gameObject.SetActive(true);
            _planetCamera.gameObject.SetActive(false);
        }
    }
}