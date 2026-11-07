using PixelAdventure.Game;
using UnityEngine;
using UnityEngine.UI;

namespace PixelAdventure.UI
{
    public sealed class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] ScoreCounter counter;
        [SerializeField] Text text;
        [SerializeField] int totalCoins;

        // Configure runs after AddComponent has already called OnEnable, so it must (re)subscribe itself.
        public void Configure(ScoreCounter scoreCounter, Text label, int coinsInLevel)
        {
            Unsubscribe();
            counter = scoreCounter;
            text = label;
            totalCoins = coinsInLevel;
            if (isActiveAndEnabled) Subscribe();
            Refresh();
        }

        public void SetCoinCount(int coinsInLevel)
        {
            totalCoins = coinsInLevel;
            Refresh();
        }

        void OnEnable()
        {
            Subscribe();
            Refresh();
        }

        void OnDisable() => Unsubscribe();

        void Subscribe()
        {
            if (counter != null) counter.Changed += Refresh;
        }

        void Unsubscribe()
        {
            if (counter != null) counter.Changed -= Refresh;
        }

        void Refresh()
        {
            if (counter == null || text == null) return;
            text.text = $"Score {counter.Score}    Coins {counter.CoinsCollected}/{totalCoins}";
        }
    }
}
