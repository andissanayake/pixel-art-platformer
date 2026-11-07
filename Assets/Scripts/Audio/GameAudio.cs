using UnityEngine;

namespace PixelAdventure.Audio
{
    /// <summary>
    /// Sounds from Kenney's "Digital Audio", "Music Jingles" and "Music Loops" packs (CC0), loaded from
    /// Assets/Resources/Audio.
    /// </summary>
    public static class GameAudio
    {
        public const string ResourcesFolder = "Audio";

        public static AudioClip Jump => Sfx("jump");
        public static AudioClip Coin => Sfx("coin");
        public static AudioClip GameOver => Sfx("game_over");
        public static AudioClip LevelComplete => Sfx("level_complete");
        public static AudioClip Music => Load($"{ResourcesFolder}/Music/music");

        static AudioClip Sfx(string name) => Load($"{ResourcesFolder}/Sfx/{name}");

        static AudioClip Load(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            if (clip == null)
                Debug.LogError($"Missing sound 'Assets/Resources/{path}.ogg'.");
            return clip;
        }
    }
}
