using PixelAdventure.Level;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace PixelAdventure.EditorTools
{
    [InitializeOnLoad]
    static class LevelBuilderMenu
    {
        const string AutoRebuildKey = "PixelAdventure.LevelBuilder.AutoRebuild";

        public static bool AutoRebuild
        {
            get => EditorPrefs.GetBool(AutoRebuildKey, true);
            set => EditorPrefs.SetBool(AutoRebuildKey, value);
        }

        public static bool BuildOnPlay
        {
            get => EditorPrefs.GetBool(GameBootstrap.BuildOnPlayKey, false);
            set => EditorPrefs.SetBool(GameBootstrap.BuildOnPlayKey, value);
        }

        static LevelBuilderMenu()
        {
            // Generated objects are a preview only; the scene file must stay empty.
            EditorSceneManager.sceneSaving += (scene, _) => LevelBuilder.Clear(scene);

            // Runs after every script recompile. Only refreshes an existing preview, so a cleared level stays cleared.
            EditorApplication.delayCall += () =>
            {
                if (AutoRebuild && !EditorApplication.isPlayingOrWillChangePlaymode && LevelBuilder.IsBuilt)
                    LevelBuilder.Rebuild();
            };
        }

        [MenuItem("Tools/Pixel Adventure/Rebuild Level")]
        static void Rebuild() => LevelBuilder.Rebuild();

        [MenuItem("Tools/Pixel Adventure/Clear Level")]
        static void Clear() => LevelBuilder.Clear();
    }
}
