using System;
using System.Collections.Generic;
using System.Linq;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Music;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Components.Music
{
    public sealed class LayeredMusicClipEditorWindow : EditorWindow
    {
        #region Static Area

        [MenuItem("Window/Audio System/Layered Music Clip Editor")]
        public static void ShowWindow()
        {
            var window = CreateInstance<LayeredMusicClipEditorWindow>();
            window.Show();
        }

        #endregion

        private SerializedObject _serializedObject;
        private SerializedProperty _layersProperty;

        private LayerReorderableList _layerList;
        private IDictionary<string, bool> _selectedLayerList = new Dictionary<string, bool>();

        private readonly MusicPlayer _musicPlayer = new MusicPlayer();

        public LayeredMusicClipEditorWindow()
        {
            _musicPlayer.PlayAudio += (_, args) =>
            {
                var sample = args.StayInTime ? EditorUtilityEx.GetAudioSamplePosition() : 0;
                EditorUtilityEx.StopAudio();
                EditorUtilityEx.PlayAudio(args.Clip, sample, args.Loop);
            };
            _musicPlayer.StopAllAudio += (_, _) => EditorUtilityEx.StopAudio();
            _musicPlayer.StopAudio += (_, args) => EditorUtilityEx.StopAudio();
            _musicPlayer.MarkedForFinishingAudio += (_, args) => EditorUtilityEx.SetLoop(false);
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Layered Music Clip");
            EditorUtilityEx.ResetAllAudioClipPlayCountsOnPlay = false;

            SelectionChanged();
            Selection.selectionChanged += SelectionChanged;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        private void OnFocus()
        {
            SelectionChanged();
        }

        private void Update()
        {
            _musicPlayer.OnUpdateAudioState(() => !EditorUtilityEx.IsAudioPlaying());
        }

        private void OnGUI()
        {
            if (_serializedObject == null)
            {
                EditorGUILayout.HelpBox("Select a layered music clip", MessageType.Warning);
                return;
            }

            _serializedObject.Update();

            _layerList.DoLayoutList();

            GUILayout.Space(50f);

            GUILayout.Label("Demo Player", EditorStyles.boldLabel);
            GUILayout.Space(20f);

            GUILayout.Label("Layers to play");
            for (var i = 0; i < _layersProperty.arraySize; i++)
            {
                var property = _layersProperty.GetArrayElementAtIndex(i);
                var nameProperty = property.FindPropertyRelative("name");
                var defaultProperty = property.FindPropertyRelative("activatedByDefault");

                var selected = _selectedLayerList.GetOrDefault(nameProperty.stringValue, defaultProperty.boolValue);
                var newSelected = GUILayout.Toggle(selected, nameProperty.stringValue);
                _selectedLayerList.AddOrOverwrite(nameProperty.stringValue, newSelected);
            }

            GUILayout.Space(20f);

            GUILayout.Label("Player");
            GUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(!_selectedLayerList.Values.Any(x => x));
                {
                    if (GUILayout.Button("Play"))
                    {
                        EditorUtilityEx.StopAudio();
                        _musicPlayer.Play((MusicClip)Selection.activeObject, false, true);
                    }
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(!EditorUtilityEx.IsAudioPlaying());
                {
                    if (GUILayout.Button("Stop"))
                    {
                        _musicPlayer.Stop();
                    }

                    if (GUILayout.Button("Finish"))
                    {
                        _musicPlayer.FinishAndStop();
                    }
                    
                    if (GUILayout.Button("Finish Immediately"))
                    {
                        _musicPlayer.FinishAndStop(true);
                    }

                    if (GUILayout.Button("Update Layers"))
                    {
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndHorizontal();

            _serializedObject.ApplyModifiedProperties();
        }

        private void SelectionChanged()
        {
            var clip = Selection.activeObject as MusicClip;
            if (clip == null)
            {
                _serializedObject = null;
                _layerList = null;
                return;
            }

            _serializedObject = new SerializedObject(clip);
            _layersProperty = _serializedObject.FindProperty("layers");

            _layerList = new LayerReorderableList(_serializedObject, _layersProperty);
        }
    }
}