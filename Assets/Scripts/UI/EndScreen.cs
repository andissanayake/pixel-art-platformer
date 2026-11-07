using PixelAdventure.Game;
using UnityEngine;
using UnityEngine.UI;

namespace PixelAdventure.UI
{
    /// <summary>
    /// Shows the game over or level complete overlay and freezes the game.
    /// Restarting rebuilds the level content; this HUD stays up and dismisses the overlay.
    /// Restart is <see cref="GameInput.RestartPressed"/>.
    /// </summary>
    public sealed class EndScreen : MonoBehaviour
    {
        // Ignore input briefly so a jump press at the moment the game ends doesn't restart straight away.
        const float RestartDelay = 0.5f;

        static readonly Color GameOverTint = new Color(0.25f, 0f, 0f, 0.7f);
        static readonly Color LevelCompleteTint = new Color(0f, 0.2f, 0.05f, 0.7f);

        [SerializeField] GameObject overlay;
        [SerializeField] Image background;
        [SerializeField] Text message;
        [SerializeField] ScoreCounter counter;
        [SerializeField] int totalCoins;

        bool m_HasEnded;
        float m_EndedAt;

        public void Configure(GameObject overlayRoot, Image overlayBackground, Text messageLabel, ScoreCounter scoreCounter, int coinsInLevel)
        {
            overlay = overlayRoot;
            background = overlayBackground;
            message = messageLabel;
            counter = scoreCounter;
            totalCoins = coinsInLevel;
            overlay.SetActive(false);
        }

        public void SetCoinCount(int coinsInLevel) => totalCoins = coinsInLevel;

        void OnEnable()
        {
            GameEvents.PlayerDied += OnPlayerDied;
            GameEvents.LevelCompleted += OnLevelCompleted;
            GameEvents.LevelRestarted += OnLevelRestarted;
            GameInput.RestartPressed += OnRestartPressed;
        }

        void OnDisable()
        {
            GameEvents.PlayerDied -= OnPlayerDied;
            GameEvents.LevelCompleted -= OnLevelCompleted;
            GameEvents.LevelRestarted -= OnLevelRestarted;
            GameInput.RestartPressed -= OnRestartPressed;
        }

        void OnDestroy()
        {
            if (m_HasEnded) Time.timeScale = 1f;
        }

        void OnPlayerDied() => Show("Game Over", GameOverTint);

        void OnLevelCompleted() => Show("Level Complete!", LevelCompleteTint);

        void Show(string title, Color tint)
        {
            if (m_HasEnded) return;

            m_HasEnded = true;
            m_EndedAt = Time.unscaledTime;
            Time.timeScale = 0f;

            var score = counter != null ? counter.Score : 0;
            var coins = counter != null ? counter.CoinsCollected : 0;
            message.text = $"{title}\n\n<size=64>Score {score}\nCoins {coins}/{totalCoins}</size>\n\n<size=36>Press R or tap to play again</size>";
            background.color = tint;
            overlay.SetActive(true);
        }

        void OnRestartPressed()
        {
            if (!m_HasEnded || Time.unscaledTime - m_EndedAt < RestartDelay) return;
            GameSession.RestartLevel();
        }

        void OnLevelRestarted()
        {
            m_HasEnded = false;
            Time.timeScale = 1f;
            if (overlay != null) overlay.SetActive(false);
        }
    }
}
