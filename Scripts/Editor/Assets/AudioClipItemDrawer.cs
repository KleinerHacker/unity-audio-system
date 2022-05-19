using UnityAudio.Editor.audio_system.Scripts.Editor.Utils;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Assets
{
    [CustomPropertyDrawer(typeof(AudioClipItem))]
    public sealed class AudioClipItemDrawer : ExtendedDrawer
    {
        private const float VolumeWidth = 35f;
        private const float VolumeCalcWidth = 70f;
        private const float BottomMargin = 2f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(
                new Rect(position.x, position.y, position.width - VolumeWidth - VolumeCalcWidth - 10f, position.height),
                property.FindPropertyRelative("audioClip"),
                GUIContent.none
            );
            EditorGUI.PropertyField(
                new Rect(position.x + position.width - VolumeWidth - VolumeCalcWidth - 5f, position.y, VolumeWidth, position.height),
                property.FindPropertyRelative("volume"),
                GUIContent.none
            );
            if (GUI.Button(
                    new Rect(position.x + position.width - VolumeCalcWidth, position.y, VolumeCalcWidth, position.height),
                    new GUIContent("Calc Vol", "Recalculate Volume on all existing clips")
                ))
            {
                var volume = VolumeUtils.CalculateVolume((AudioClip)property.FindPropertyRelative("audioClip").objectReferenceValue);
                property.FindPropertyRelative("volume").floatValue = volume;
            }
        }
    }
}