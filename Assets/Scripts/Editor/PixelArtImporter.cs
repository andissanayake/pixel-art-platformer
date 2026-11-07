using System.IO;
using PixelAdventure.Art;
using UnityEditor;
using UnityEngine;

namespace PixelAdventure.EditorTools
{
    /// <summary>
    /// Import settings for everything under <see cref="ArtRoot"/>, defined in code instead of per-file in the
    /// Inspector: sprites at <see cref="GameArt.PixelsPerUnit"/>, point filtering, no compression or mipmaps.
    /// Characters are pivoted at their feet so they can be placed on the bottom edge of their collider.
    /// The checkpoint flag is pivoted on its pole, at the bottom of the frame.
    /// </summary>
    sealed class PixelArtImporter : AssetPostprocessor
    {
        const string ArtRoot = "Assets/Resources/" + GameArt.ResourcesFolder + "/";
        const string CharactersFolder = ArtRoot + "Characters/";

        // Horizontal center of the pole in the 64px checkpoint frame.
        const float FlagPolePivotX = 23f / 64f;

        // Bump to reimport all art after changing the settings below.
        public override uint GetVersion() => 2;

        void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith(ArtRoot)) return;

            var importer = (TextureImporter)assetImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = GameArt.PixelsPerUnit;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;

            var fileName = Path.GetFileName(assetPath);
            var settings = new TextureImporterSettings();
            importer.ReadTextureSettings(settings);
            // FullRect is required for tiled and sliced draw modes.
            settings.spriteMeshType = SpriteMeshType.FullRect;
            if (assetPath.StartsWith(CharactersFolder))
            {
                settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;
            }
            else if (fileName.StartsWith("flag_"))
            {
                settings.spriteAlignment = (int)SpriteAlignment.Custom;
                settings.spritePivot = new Vector2(FlagPolePivotX, 0f);
            }
            else
            {
                settings.spriteAlignment = (int)SpriteAlignment.Center;
            }

            importer.SetTextureSettings(settings);
        }

        [MenuItem("Tools/Pixel Adventure/Reimport Art")]
        static void ReimportArt()
        {
            AssetDatabase.ImportAsset(ArtRoot.TrimEnd('/'), ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
        }
    }
}
