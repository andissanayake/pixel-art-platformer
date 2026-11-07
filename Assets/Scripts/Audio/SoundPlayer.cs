using PixelAdventure.Game;
using UnityEngine;

namespace PixelAdventure.Audio
{
    /// <summary>
    /// Plays the background music and reacts to game events with sound effects. When the game ends the music
    /// stops and a game over or level complete sound plays instead.
    /// Mute is <see cref="GameInput.MutePressed"/>; the setting is saved between sessions.
    /// </summary>
    public sealed class SoundPlayer : MonoBehaviour
    {
        const string MutedKey = "PixelAdventure.Muted";

        [SerializeField] AudioSource music;
        [SerializeField] AudioSource effects;
        [SerializeField] AudioClip jumpClip;
        [SerializeField] AudioClip coinClip;
        [SerializeField] AudioClip gameOverClip;
        [SerializeField] AudioClip levelCompleteClip;

        public static bool Muted
        {
            get => PlayerPrefs.GetInt(MutedKey, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(MutedKey, value ? 1 : 0);
                AudioListener.volume = value ? 0f : 1f;
            }
        }

        public void Configure(AudioSource musicSource, AudioSource effectsSource)
        {
            music = musicSource;
            effects = effectsSource;
            music.clip = GameAudio.Music;
            jumpClip = GameAudio.Jump;
            coinClip = GameAudio.Coin;
            gameOverClip = GameAudio.GameOver;
            levelCompleteClip = GameAudio.LevelComplete;
        }

        void OnEnable()
        {
            GameEvents.Jumped += OnJumped;
            GameEvents.CoinCollected += OnCoinCollected;
            GameEvents.PlayerDied += OnPlayerDied;
            GameEvents.LevelCompleted += OnLevelCompleted;
            GameEvents.LevelRestarted += OnLevelRestarted;
            GameInput.MutePressed += OnMutePressed;
        }

        void OnDisable()
        {
            GameEvents.Jumped -= OnJumped;
            GameEvents.CoinCollected -= OnCoinCollected;
            GameEvents.PlayerDied -= OnPlayerDied;
            GameEvents.LevelCompleted -= OnLevelCompleted;
            GameEvents.LevelRestarted -= OnLevelRestarted;
            GameInput.MutePressed -= OnMutePressed;
        }

        void Start()
        {
            AudioListener.volume = Muted ? 0f : 1f;
            PlayMusic();
        }

        void OnMutePressed() => Muted = !Muted;

        void OnJumped() => PlayEffect(jumpClip);

        void OnCoinCollected(int _) => PlayEffect(coinClip);

        void OnPlayerDied() => Finish(gameOverClip);

        void OnLevelCompleted() => Finish(levelCompleteClip);

        // The sound player survives a level restart, so the finished round has to be cleared by hand.
        void OnLevelRestarted()
        {
            if (!Application.isPlaying) return;

            if (effects != null) effects.Stop();
            PlayMusic();
        }

        void PlayMusic()
        {
            if (music == null || music.clip == null) return;
            music.Stop();
            music.Play();
        }

        // Reached only for the outcome that ended the round, so the sting plays even though the round has ended.
        void Finish(AudioClip clip)
        {
            if (clip != null) effects.PlayOneShot(clip);
            music.Stop();
        }

        void PlayEffect(AudioClip clip)
        {
            if (GameEvents.HasEnded || clip == null) return;
            effects.PlayOneShot(clip);
        }
    }
}
