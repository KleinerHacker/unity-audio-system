using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEngine;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Types;

namespace UnitySfx.Editor.sfx_system.Scripts.Editor.Assets
{
    [CustomPropertyDrawer(typeof(AudioClipItem))]
    public sealed class AudioClipItemDrawer : ExtendedDrawer
    {
        private const float VolumeWidth = 70f;
        private const float BottomMargin = 2f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(
                new Rect(position.x, position.y, position.width - VolumeWidth - 5f, position.height),
                property.FindPropertyRelative("audioClip"),
                GUIContent.none
            );
            EditorGUI.PropertyField(
                new Rect(position.x + position.width - VolumeWidth, position.y, VolumeWidth, position.height),
                property.FindPropertyRelative("volume"),
                GUIContent.none
            );
        }
    }
}