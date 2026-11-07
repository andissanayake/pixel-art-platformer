using PixelAdventure.Audio;
using UnityEngine;

namespace PixelAdventure.Level.Parts
{
    public sealed class AudioPart : ILevelPart
    {
        public const string PartId = "Audio";
        public string Id => PartId;

        public void Create(LevelContext context, Transform parent)
        {
            var go = LevelContext.CreateChild(parent, "Sound Player", Vector3.zero);

            var music = go.AddComponent<AudioSource>();
            music.playOnAwake = false;
            music.loop = true;
            music.volume = LevelSettings.MusicVolume;

            var effects = go.AddComponent<AudioSource>();
            effects.playOnAwake = false;
            effects.volume = LevelSettings.EffectsVolume;

            go.AddComponent<SoundPlayer>().Configure(music, effects);
        }
    }
}
