using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Components.Music
{
    public sealed class ClipReorderableList : ReorderableList
    {
        public ClipReorderableList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(rect, "Clips", EditorStyles.boldLabel);
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            EditorGUI.PropertyField(rect, serializedProperty.GetArrayElementAtIndex(i), GUIContent.none);
        }
    }
}