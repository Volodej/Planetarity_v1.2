using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Image _pauseButtonIcon;

        [SerializeField] private Sprite _pauseIcon;
        [SerializeField] private Sprite _playIcon;

        private PauseState _pauseState;

        public Action MainMenuRequested;
        public Action<PauseState> PauseStateChanged;

        public void Show(PauseState pauseState)
        {
            ChangePauseState(pauseState);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(() => MainMenuRequested?.Invoke());
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnPauseButtonClicked()
        {
            ChangePauseState(_pauseState == PauseState.Paused ? PauseState.Unpaused : PauseState.Paused);
            PauseStateChanged(_pauseState);
        }

        private void ChangePauseState(PauseState pauseState)
        {
            _pauseState = pauseState;
            _pauseButtonIcon.sprite = _pauseState == PauseState.Paused ? _pauseIcon : _playIcon;
        }
    }
}