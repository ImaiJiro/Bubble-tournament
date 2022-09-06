#if UNITY_EDITOR
using Etourney.Enums;
using Etourney.Settings;
using UnityEditor;
using UnityEngine;

namespace Etourney.Editor
{
    public sealed class EtourneySettingsWindow : EditorWindow
    {
        private GUIStyle HorizontalLine
        {
            get
            {
                if (_horizontalLine != null)
                {
                    return _horizontalLine;
                }

                _horizontalLine = new GUIStyle();
                _horizontalLine.normal.background = Texture2D.whiteTexture;
                _horizontalLine.margin = new RectOffset(0, 0, 4, 4);
                _horizontalLine.fixedHeight = 1;

                return _horizontalLine;
            }
        }

        private GUIStyle _horizontalLine;

        [MenuItem("Etourney/Settings")]
        private static void Open()
        {
            var window = GetWindow<EtourneySettingsWindow>();
            window.titleContent.text = "Etourney settings";
            window.minSize = new Vector2(300, 400);

            window.Show();
        }

        private void OnGUI()
        {
            DrawGameSettingsPane();
            DrawHorizontalLine();

            ApplyChanges();
        }

        private void ApplyChanges()
        {
            if (!GUI.changed)
            {
                return;
            }

            EditorUtility.SetDirty(EtourneySettings.Instance);
            AssetDatabase.SaveAssets();
        }

        private void DrawGameSettingsPane()
        {
            EditorGUILayout.Foldout(true, "Etourney game Settings");

            EditorGUI.indentLevel++;

            EditorGUILayout.Space();

            EtourneySettings.Instance.GameKey = EditorGUILayout.TextField("Game key:", EtourneySettings.Instance.GameKey);
            EtourneySettings.Instance.LocalGameKey = EditorGUILayout.TextField("Local game key:", EtourneySettings.Instance.LocalGameKey);
            EtourneySettings.Instance.ServerLocation = (ServerLocation) EditorGUILayout.EnumPopup("Server location:", EtourneySettings.Instance.ServerLocation);
            EtourneySettings.Instance.Environment = (Environment) EditorGUILayout.EnumPopup("Environment:", EtourneySettings.Instance.Environment);
            EtourneySettings.Instance.Orientation = (Orientation) EditorGUILayout.EnumPopup("Orientation:", EtourneySettings.Instance.Orientation);
        }

        private void DrawHorizontalLine()
        {
            var prevColor = GUI.color;
            GUI.color = Color.grey;

            GUILayout.Box(GUIContent.none, HorizontalLine);

            GUI.color = prevColor;
        }
    }
}
#endif