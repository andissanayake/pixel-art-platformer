using PixelAdventure.Audio;
using UnityEditor;
using UnityEngine;

namespace PixelAdventure.EditorTools
{
    /// <summary>
    /// Import settings for everything under <see cref="AudioRoot"/>, defined in code instead of per-file in the
    /// Inspector: short effects are decompressed on load so they start instantly, music is streamed.
    /// </summary>
    sealed class SoundImporter : AssetPostprocessor
    {
        const string AudioRoot = "Assets/Resources/" + GameAudio.ResourcesFolder + "/";
        const string MusicFolder = AudioRoot + "Music/";

        // Bump to reimport all audio after changing the settings below.
        public override uint GetVersion() => 1;

        void OnPreprocessAudio()
        {
            if (!assetPath.StartsWith(AudioRoot)) return;

            var importer = (AudioImporter)assetImporter;
            var isMusic = assetPath.StartsWith(MusicFolder);

            var settings = importer.defaultSampleSettings;
            settings.loadType = isMusic ? AudioClipLoadType.Streaming : AudioClipLoadType.DecompressOnLoad;
            settings.compressionFormat = AudioCompressionFormat.Vorbis;
            settings.quality = isMusic ? 0.7f : 1f;
            importer.defaultSampleSettings = settings;
            importer.forceToMono = !isMusic;
            importer.loadInBackground = isMusic;
        }

        [MenuItem("Tools/Pixel Adventure/Reimport Audio")]
        static void ReimportAudio()
        {
            AssetDatabase.ImportAsset(AudioRoot.TrimEnd('/'), ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
        }
    }
}
