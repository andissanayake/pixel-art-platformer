using System;

namespace PixelAdventure.Game
{
    /// <summary>
    /// Game-wide events, so whatever causes them doesn't need references to whatever reacts,
    /// and either side can be rebuilt on its own.
    /// The first of <see cref="PlayerDied"/> and <see cref="LevelCompleted"/> ends the round; the other is ignored.
    /// </summary>
    public static class GameEvents
    {
        public static bool HasEnded { get; private set; }

        public static event Action Jumped;
        public static event Action<int> CoinCollected;
        public static event Action PlayerDied;
        public static event Action LevelCompleted;

        /// <summary>The level content was replaced. Session systems (HUD, audio) reset; their objects stay.</summary>
        public static event Action LevelRestarted;

        public static void RaiseJumped()
        {
            if (HasEnded) return;
            Jumped?.Invoke();
        }

        public static void RaiseCoinCollected(int value)
        {
            if (HasEnded) return;
            CoinCollected?.Invoke(value);
        }

        public static void RaisePlayerDied() => EndRound(PlayerDied);

        public static void RaiseLevelCompleted() => EndRound(LevelCompleted);

        public static void RaiseLevelRestarted()
        {
            BeginRound();
            LevelRestarted?.Invoke();
        }

        /// <summary>A new round is starting, so death and completion can be raised again.</summary>
        public static void BeginRound() => HasEnded = false;

        static void EndRound(Action outcome)
        {
            if (HasEnded) return;
            HasEnded = true;
            outcome?.Invoke();
        }
    }
}
