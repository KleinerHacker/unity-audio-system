using System.Linq;
using UnityAudio.Editor.audio_system.Scripts.Editor.Utils;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Commons;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Assets.Sfx
{
    [CustomEditor(typeof(RandomSfxClip))]
    public sealed class RandomSfxClipEditor : AutoEditor
    {
        [SerializedPropertyReference("items")]
        [SerializedPropertyDefaultRepresentation]
        private SerializedProperty _itemsProperty;

        public RandomSfxClipEditor() : base(CustomGUIPosition.Top)
        {
        }

        protected override void DoInspectorGUI()
        {
            var rect = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
            GUI.Box(rect, "Drag and drop audio clips here");
            EditorGUILayout.Space(20f);

            var evt = Event.current;
            if (evt.type is EventType.DragUpdated or EventType.DragPerform)
            {
                if (!rect.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    var audioClips = DragAndDrop.objectReferences
                        .Select(AssetDatabase.GetAssetPath)
                        .Select(AssetDatabase.LoadAssetAtPath<AudioClip>)
                        .Where(x => x != null)
                        .ToArray();

                    foreach (var audioClip in audioClips)
                    {
                        _itemsProperty.InsertArrayElementAtIndex(_itemsProperty.arraySize);
                        var property = _itemsProperty.GetArrayElementAtIndex(_itemsProperty.arraySize - 1);
                        property.FindPropertyRelative("audioClip").objectReferenceValue = audioClip;
                        property.FindPropertyRelative("volume").floatValue = 1f;
                    }
                }
            }

            if (GUILayout.Button("Calculate Volumes"))
            {
                var properties = serializedObject.FindProperties("items");
                
                var volumes = VolumeUtils.CalculateVolumes(
                    properties
                        .Select(x => (AudioClip) x.FindPropertyRelative("audioClip").objectReferenceValue)
                        .ToArray()
                );
                for (var i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    property.FindPropertyRelative("volume").floatValue = volumes[i];
                }
            }
        }
    }
}