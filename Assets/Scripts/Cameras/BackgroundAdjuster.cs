using System;
using UnityEngine;

namespace Planetarity.Cameras
{
    public class BackgroundAdjuster : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _moveFactor = 0.1f;

        private void LateUpdate()
        {
            var position = -_camera.transform.position * _moveFactor;
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
        }
    }
}