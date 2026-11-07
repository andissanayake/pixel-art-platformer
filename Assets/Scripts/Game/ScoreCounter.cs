using System;
using UnityEngine;

namespace PixelAdventure.Game
{
    public sealed class ScoreCounter : MonoBehaviour
    {
        public event Action Changed;

        public int Score { get; private set; }
        public int CoinsCollected { get; private set; }

        void OnEnable()
        {
            GameEvents.CoinCollected += OnCoinCollected;
            GameEvents.LevelRestarted += ResetScore;
        }

        void OnDisable()
        {
            GameEvents.CoinCollected -= OnCoinCollected;
            GameEvents.LevelRestarted -= ResetScore;
        }

        public void ResetScore()
        {
            Score = 0;
            CoinsCollected = 0;
            Changed?.Invoke();
        }

        void OnCoinCollected(int value)
        {
            Score += value;
            CoinsCollected++;
            Changed?.Invoke();
        }
    }
}
