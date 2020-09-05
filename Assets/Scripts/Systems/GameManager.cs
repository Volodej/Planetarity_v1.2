using System;
using Planetarity.Models;
using Planetarity.UI;
using Planetarity.UI.Models;
using UnityEngine;
using PauseState = UnityEditor.PauseState;

namespace Planetarity.Systems
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenu;
        [SerializeField] private HudView _hud;
        [SerializeField] private GameResultSequenceView _gameResultSequence;
        [SerializeField] private GamePlayController _gamePlayController;

        private readonly SaveSystem _saveSystem = new SaveSystem();

        private void Awake()
        {
            _hud.MainMenuRequested += HandleMainMenu;
            _hud.PauseStateChanged += _gamePlayController.ApplyPauseState;
        }

        private void Start()
        {
            HandleMainMenu();
        }

        private async void HandleMainMenu()
        {
            if (_gamePlayController.IsPlaying)
            {
                _hud.Hide();
                _gamePlayController.ApplyPauseState(PauseState.Paused);
            }

            var menuAction = await _mainMenu.Show(new MainMenuState
            {
                HasLoadAction = _saveSystem.HasSave,
                HasSaveAction = _gamePlayController.IsPlaying,
                IsAvailableBackToGame = _gamePlayController.IsPlaying
            });

            switch (menuAction)
            {
                case MainMenuAction.NewGame:
                    PlayGame();
                    break;
                case MainMenuAction.SaveGame:
                    _saveSystem.SaveGameState(_gamePlayController.GetGameState());
                    _gamePlayController.ApplyPauseState(PauseState.Unpaused);
                    _hud.Show(PauseState.Unpaused);
                    break;
                case MainMenuAction.LoadGame:
                    PlayGame(_saveSystem.LoadGameState());
                    break;
                case MainMenuAction.Exit:
                    Application.Quit();
                    break;
                case MainMenuAction.BackToGame:
                    _gamePlayController.ApplyPauseState(PauseState.Unpaused);
                    _hud.Show(PauseState.Unpaused);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void PlayGame(GameState gameState = null)
        {
            _hud.Show(PauseState.Unpaused);
            _gamePlayController.ApplyPauseState(PauseState.Unpaused);

            var gameTask = gameState != null
                ? _gamePlayController.StartGame(gameState)
                : _gamePlayController.StartNewGame();
            var gameResult = await gameTask;

            switch (gameResult)
            {
                case GameResult.Victory:
                    await _gameResultSequence.Show("VICTORY!");
                    break;
                case GameResult.Defeat:
                    await _gameResultSequence.Show("DEFEAT");
                    break;
                case GameResult.Stopped:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            HandleMainMenu();
        }
    }
}