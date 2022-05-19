using System.Linq;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Sfx;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Components.Sfx
{
    [CustomEditor(typeof(SfxSource))]
    public sealed class SfxSourceEditor : ExtendedEditor
    {
        private SerializedProperty _presetProperty;
        private string[] _presets;

        private void OnEnable()
        {
            _presetProperty = serializedObject.FindProperty("preset");
            _presets = new[] { "<default>" }.Concat(SfxSystemSettings.Singleton.Items.Select(x => x.Identifier)).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var index = _presets.IndexOf(x => string.Equals(x, _presetProperty.stringValue)) + 1;
            var newIndex = EditorGUILayout.Popup(new GUIContent("Preset"), index, _presets);
            if (newIndex != index)
            {
                _presetProperty.stringValue = newIndex <= 0 ? null : _presets[newIndex - 1];
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}