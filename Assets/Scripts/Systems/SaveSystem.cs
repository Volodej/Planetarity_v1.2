using System;
using Planetarity.Models;
using UnityEngine;

namespace Planetarity.Systems
{
    public class SaveSystem
    {
        private const string SAVE_KEY = "GameSaveState";

        public bool HasSave => PlayerPrefs.HasKey(SAVE_KEY);

        public void SaveGameState(GameState gameState)
        {
            var json = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }

        public GameState LoadGameState()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY) == false)
            {
                throw new InvalidOperationException("No save was stored in PlayerPrefs");
            }

            var json = PlayerPrefs.GetString(SAVE_KEY);
            var state = JsonUtility.FromJson<GameState>(json);
            return state;
        }
    }
}