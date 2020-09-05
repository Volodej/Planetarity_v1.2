using System;
using System.Threading.Tasks;
using Planetarity.UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _backButton;

        private TaskCompletionSource<MainMenuAction> _taskCompletionSource;

        public async Task<MainMenuAction> Show(MainMenuState state)
        {
            _saveGameButton.interactable = state.HasSaveAction;
            _loadGameButton.interactable = state.HasLoadAction;
            _backButton.gameObject.SetActive(state.IsAvailableBackToGame);

            gameObject.SetActive(true);

            _taskCompletionSource = new TaskCompletionSource<MainMenuAction>();
            await _taskCompletionSource.Task;
            gameObject.SetActive(false);
            return await _taskCompletionSource.Task;
        }

        private void Awake()
        {
            _newGameButton.onClick.AddListener(() => _taskCompletionSource?.TrySetResult(MainMenuAction.NewGame));
            _saveGameButton.onClick.AddListener(() => _taskCompletionSource?.TrySetResult(MainMenuAction.SaveGame));
            _loadGameButton.onClick.AddListener(() => _taskCompletionSource?.TrySetResult(MainMenuAction.LoadGame));
            _exitGameButton.onClick.AddListener(() => _taskCompletionSource?.TrySetResult(MainMenuAction.Exit));
            _backButton.onClick.AddListener(() => _taskCompletionSource?.TrySetResult(MainMenuAction.BackToGame));
        }
    }
}