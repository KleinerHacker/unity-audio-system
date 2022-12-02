using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Components.Music
{
    public sealed class LayerReorderableList : ReorderableList
    {
        private VariantReorderableList[] _introVariantLists = Array.Empty<VariantReorderableList>();
        private VariantReorderableList[] _extroVariantLists = Array.Empty<VariantReorderableList>();
        private VariantReorderableList[] _loopVariantLists = Array.Empty<VariantReorderableList>();

        public LayerReorderableList(SerializedObject serializedObject, SerializedProperty elements) : base(serializedObject, elements)
        {
            drawHeaderCallback += DrawHeaderCallback;
            drawElementCallback += DrawElementCallback;
            elementHeightCallback += ElementHeightCallback;

            OnChangedCallback(this);
            onChangedCallback += OnChangedCallback;
        }

        private void DrawHeaderCallback(Rect rect)
        {
            GUI.Label(rect, "Layer List");
            GUI.Label(rect, "Layers: " + serializedProperty.arraySize, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight });
        }

        private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused)
        {
            var property = serializedProperty.GetArrayElementAtIndex(i);

            var nameProperty = property.FindPropertyRelative("name");
            var defaultProperty = property.FindPropertyRelative("activatedByDefault");
            
            var introVariantsProperty = property.FindPropertyRelative("introVariants");
            var introVariantIndexProperty = property.FindPropertyRelative("defaultIntroVariantIndex");
            
            var extroVariantsProperty = property.FindPropertyRelative("extroVariants");
            var extroVariantIndexProperty = property.FindPropertyRelative("defaultExtroVariantIndex");
            
            var loopVariantsProperty = property.FindPropertyRelative("loopVariants");
            var loopVariantIndexProperty = property.FindPropertyRelative("defaultLoopVariantIndex");

            nameProperty.stringValue = GUI.TextField(new Rect(rect.x + rect.width - 250f, rect.y + 1f, 250f, 20f), nameProperty.stringValue, new GUIStyle(EditorStyles.textField) { alignment = TextAnchor.MiddleRight });
            defaultProperty.boolValue = GUI.Toggle(new Rect(rect.x, rect.y, 250f, 25f), defaultProperty.boolValue, "Activate on startup");
            
            FragmentBox(new Rect(rect.x, rect.y + 25f, rect.width / 4f - 5f, 25f), "Intro", introVariantsProperty, introVariantIndexProperty);
            FragmentBox(new Rect(rect.x + rect.width - rect.width / 4f + 5f, rect.y + 25f, rect.width / 4f - 5f, 25f), "Extro", extroVariantsProperty, extroVariantIndexProperty);
            FragmentBox(new Rect(rect.x + rect.width / 4f + 5f, rect.y + 25f, rect.width / 2f - 10f, 25f), "Loop", loopVariantsProperty, loopVariantIndexProperty);

            _introVariantLists[i].DoList(new Rect(rect.x, rect.y + 55f, rect.width / 4f - 5f, 50f));
            _extroVariantLists[i].DoList(new Rect(rect.x + rect.width - rect.width / 4f + 5f, rect.y + 55f, rect.width / 4f - 5f, 50f));
            _loopVariantLists[i].DoList(new Rect(rect.x + rect.width / 4f + 5f, rect.y + 55f, rect.width / 2f - 10f, 50f));

            void FragmentBox(Rect boxRect, string name, SerializedProperty variantsProperty, SerializedProperty variantIndexProperty)
            {
                GUI.Box(boxRect, name, EditorStyles.helpBox);
                GUI.Label(new Rect(boxRect.x + 100f, boxRect.y+1, 50f, 20f), "Default");
                var variantNames = ExtractVariantNames(variantsProperty);
                variantIndexProperty.intValue = EditorGUI.Popup(new Rect(boxRect.x + 150f, boxRect.y + 3f, boxRect.width - 153f, 20f), variantIndexProperty.intValue, variantNames);
            }

            string[] ExtractVariantNames(SerializedProperty variantsProperty)
            {
                var result = new string[variantsProperty.arraySize];
                for (var i = 0; i < variantsProperty.arraySize; i++)
                {
                    result[i] = variantsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue;
                }

                return result;
            }
        }

        private float ElementHeightCallback(int i)
        {
            return 75f + Mathf.Max(_introVariantLists[i].GetHeight(), _extroVariantLists[i].GetHeight(), _loopVariantLists[i].GetHeight());
        }

        private void OnChangedCallback(ReorderableList reorderableList)
        {
            _introVariantLists = new VariantReorderableList[serializedProperty.arraySize];
            _loopVariantLists = new VariantReorderableList[serializedProperty.arraySize];
            _extroVariantLists = new VariantReorderableList[serializedProperty.arraySize];
            
            for (var i = 0; i < serializedProperty.arraySize; i++)
            {
                var property = serializedProperty.GetArrayElementAtIndex(i);
                
                var introProperty = property.FindPropertyRelative("introVariants");
                var loopProperty = property.FindPropertyRelative("loopVariants");
                var extroProperty = property.FindPropertyRelative("extroVariants");
                
                _introVariantLists[i] = new VariantReorderableList(serializedProperty.serializedObject, introProperty);
                _loopVariantLists[i] = new VariantReorderableList(serializedProperty.serializedObject, loopProperty);
                _extroVariantLists[i] = new VariantReorderableList(serializedProperty.serializedObject, extroProperty);
            }
        }
    }
}