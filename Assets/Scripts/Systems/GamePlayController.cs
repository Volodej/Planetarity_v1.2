using System.Linq;
using System.Threading.Tasks;
using Planetarity.Models;
using UnityEditor;
using UnityEngine;

namespace Planetarity.Systems
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private PlanetFactory _planetFactory;
        [SerializeField] private PlanetPropertiesGenerator _planetPropertiesGenerator;

        private readonly GameStateGrabber _gameStateGrabber = new GameStateGrabber();

        private GameSession _gameSession;

        public bool IsPlaying => _gameSession != null && _gameSession.IsActive;

        public void ApplyPauseState(PauseState state)
        {
            Time.timeScale = state == PauseState.Paused ? 0 : 1;
        }

        public Task<GameResult> StartNewGame()
        {
            var gameState = _planetPropertiesGenerator.GenerateGameState();
            return StartGame(gameState);
        }

        public Task<GameResult> StartGame(GameState gameState)
        {
            StopGame();
            var playerPlanet = _planetFactory.SpawnPlayerPlanet(gameState.PlayerPlanet);
            var enemyPlanets = gameState.EnemyPlanets
                .Select(_planetFactory.SpawnEnemyPlanet)
                .ToList();

            _gameSession = new GameSession(playerPlanet, enemyPlanets);
            return _gameSession.Play();
        }

        public GameState GetGameState()
        {
            return _gameStateGrabber.GetFromSession(_gameSession);
        }

        private void StopGame()
        {
            _gameSession?.Stop();
            _gameSession = null;
        }
    }
}