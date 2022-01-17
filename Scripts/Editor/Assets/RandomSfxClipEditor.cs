using System;
using System.Linq;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Commons;
using UnityEngine;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Types;

namespace UnitySfx.Editor.sfx_system.Scripts.Editor.Assets
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
        }
    }
}