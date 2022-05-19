using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Provider
{
    public sealed class SfxList : ReorderableList
    {
        private const float LeftMargin = 15f;
        private const float BottomMargin = 2f;
        private const float ColumnSpace = 5f;

        private const float AmbienceMinDelayWidth = 100f;
        private const float AmbienceMaxDelayWidth = 100f;
        private const float MixerGroupWidth = 250f;
        private const float ValueWidth = 300f;
        private const float CommonWidth = MixerGroupWidth + ValueWidth + AmbienceMinDelayWidth + AmbienceMaxDelayWidth;

        public SfxList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            var commonWidth = rect.width - (CommonWidth + LeftMargin);
            var pos = new Rect(rect.x + LeftMargin, rect.y, commonWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Key"));

            pos = new Rect(rect.x + LeftMargin + commonWidth, rect.y, AmbienceMinDelayWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Amb. Min Delay"));

            pos = new Rect(rect.x + LeftMargin + commonWidth + AmbienceMinDelayWidth, rect.y, AmbienceMaxDelayWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Amb. Max Delay"));

            pos = new Rect(rect.x + LeftMargin + commonWidth + AmbienceMinDelayWidth + AmbienceMaxDelayWidth, rect.y, MixerGroupWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Mixer Group"));

            pos = new Rect(rect.x + LeftMargin + AmbienceMinDelayWidth + AmbienceMaxDelayWidth + commonWidth + MixerGroupWidth, rect.y, ValueWidth, rect.height);
            EditorGUI.LabelField(pos, new GUIContent("Initial Volume"));
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var identifierProperty = property.FindPropertyRelative("identifier");
            var subProperty = property.FindPropertyRelative("data");
            var ambienceMinDelayProperty = subProperty.FindPropertyRelative("minAmbientDelay");
            var ambienceMaxDelayProperty = subProperty.FindPropertyRelative("maxAmbientDelay");
            var mixerGroupProperty = subProperty.FindPropertyRelative("mixerGroup");
            var initialVolumeProperty = subProperty.FindPropertyRelative("initialVolume");

            var commonWidth = rect.width - CommonWidth;
            var pos = new Rect(rect.x, rect.y, commonWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, identifierProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth, rect.y, AmbienceMinDelayWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, ambienceMinDelayProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth + AmbienceMinDelayWidth, rect.y, AmbienceMaxDelayWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, ambienceMaxDelayProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth + AmbienceMinDelayWidth + AmbienceMaxDelayWidth, rect.y, MixerGroupWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, mixerGroupProperty, GUIContent.none);

            pos = new Rect(rect.x + commonWidth + AmbienceMinDelayWidth + AmbienceMaxDelayWidth + MixerGroupWidth, rect.y, ValueWidth - ColumnSpace, rect.height - BottomMargin);
            EditorGUI.PropertyField(pos, initialVolumeProperty, GUIContent.none);
        }
    }
}