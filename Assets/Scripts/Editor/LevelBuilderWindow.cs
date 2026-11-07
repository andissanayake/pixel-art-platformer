using System.Collections.Generic;
using PixelAdventure.Game;
using PixelAdventure.Level;
using UnityEditor;
using UnityEngine;

namespace PixelAdventure.EditorTools
{
    sealed class LevelBuilderWindow : EditorWindow
    {
        [MenuItem("Tools/Pixel Adventure/Level Builder Window")]
        static void Open() => GetWindow<LevelBuilderWindow>("Level Builder");

        void OnHierarchyChange() => Repaint();

        void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Rebuild All")) LevelBuilder.Rebuild();
                if (GUILayout.Button("Rebuild Level")) GameSession.RestartLevel();
                if (GUILayout.Button("Clear All")) LevelBuilder.Clear();
            }

            LevelBuilderMenu.AutoRebuild = EditorGUILayout.ToggleLeft("Rebuild preview after code changes", LevelBuilderMenu.AutoRebuild);
            LevelBuilderMenu.BuildOnPlay = EditorGUILayout.ToggleLeft("Build level when entering Play mode", LevelBuilderMenu.BuildOnPlay);

            EditorGUILayout.Space();
            DrawParts("Session", LevelBuilder.SessionParts);
            EditorGUILayout.Space();
            DrawParts("Level", LevelBuilder.LevelParts);
        }

        static void DrawParts(string label, IReadOnlyList<ILevelPart> parts)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            foreach (var part in parts)
            {
                var built = LevelBuilder.HasPart(part.Id);
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(part.Id, built ? "built" : "not built");
                    if (GUILayout.Button("Rebuild", GUILayout.Width(70))) LevelBuilder.Rebuild(part.Id);
                    using (new EditorGUI.DisabledScope(!built))
                    {
                        if (GUILayout.Button("Clear", GUILayout.Width(60))) LevelBuilder.Clear(part.Id);
                    }
                }
            }
        }
    }
}
