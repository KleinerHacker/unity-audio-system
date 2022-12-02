using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Components.Music
{
    public sealed class VariantReorderableList : ReorderableList
    {
        private ClipReorderableList[] _clipLists = Array.Empty<ClipReorderableList>();
        
        public VariantReorderableList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
            elementHeightCallback += ElementHeightCallback;
            
            OnChangedCallback(this);
            onChangedCallback += OnChangedCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(rect, "Variants");
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);

            var nameProperty = property.FindPropertyRelative("name");

            nameProperty.stringValue = GUI.TextField(new Rect(rect.x, rect.y + 1f, rect.width - 5f, 20f), nameProperty.stringValue);
            _clipLists[i].DoList(new Rect(rect.x, rect.y + 25f, rect.width, rect.height - 25f));
        }

        private float ElementHeightCallback(int i)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);
            var clipsProperty = property.FindPropertyRelative("clips");

            return 100f + Mathf.Max(0, clipsProperty.arraySize - 1) * 20f;
        }

        private void OnChangedCallback(ReorderableList reorderableList)
        {
            _clipLists = new ClipReorderableList[serializedProperty.arraySize];
            for (var i = 0; i < serializedProperty.arraySize; i++)
            {
                _clipLists[i] = new ClipReorderableList(serializedProperty.serializedObject, serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("clips"));
            }
        }
    }
}