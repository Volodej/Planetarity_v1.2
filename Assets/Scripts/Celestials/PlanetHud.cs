using System;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.Celestials
{
    public class PlanetHud : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _sliderFill;
        [SerializeField] private Text _reloadText;

        [SerializeField] private Color _playerHealthColor;
        [SerializeField] private Color _enemyHealthColor;

        private void Awake()
        {
            _canvas.worldCamera = Camera.main;
        }

        public void SetupForPlayer()
        {
            _reloadText.gameObject.SetActive(true);
            _sliderFill.color = _playerHealthColor;
        }

        public void SetupForEnemy()
        {
            _reloadText.gameObject.SetActive(false);
            _sliderFill.color = _enemyHealthColor;
        }

        public void SetReloadTime(float time)
        {
            if (_reloadText.gameObject.activeSelf)
                _reloadText.text = time.ToString("0.0");
        }
        
        public void SetHealth(float healthPercentage)
        {
            _healthSlider.value = healthPercentage;
        }

        public void MarkAsDead()
        {
            _canvas.enabled = false;
        }
    }
}