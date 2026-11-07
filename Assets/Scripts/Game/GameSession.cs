using PixelAdventure.Level;
using UnityEngine;

namespace PixelAdventure.Game
{
    /// <summary>
    /// Play-session flow. Restarting replaces the level content and tells the HUD and audio, which stay alive,
    /// to reset themselves.
    /// </summary>
    public static class GameSession
    {
        public static void RestartLevel()
        {
            Time.timeScale = 1f;
            LevelBuilder.RebuildLevel();
            GameEvents.RaiseLevelRestarted();
        }
    }
}
