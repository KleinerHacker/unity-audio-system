using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets;

namespace UnitySfx.Editor.sfx_system.Scripts.Editor.Provider
{
    public sealed class SfxSystemProvider : SettingsProvider
    {
        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SfxSystemProvider();
        }

        #endregion

        private SerializedObject _settings;
        private SerializedProperty _itemsProperty;
        private SerializedProperty _mixerGroupProperty;
        private SerializedProperty _initialVolumeProperty;
        private SerializedProperty _ambienceMinDelayProperty;
        private SerializedProperty _ambienceMaxDelayProperty;

        private SfxList _sfxList;

        private bool _ambienceFold;

        public SfxSystemProvider() : base("Project/Audio/SFX System", SettingsScope.Project, new[] { "tooling", "audio", "SFX" })
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = SfxSystemSettings.SerializedSingleton;
            if (_settings == null)
                return;

            _itemsProperty = _settings.FindProperty("items");
            var dataProperty = _settings.FindProperty("data");
            _mixerGroupProperty = dataProperty.FindPropertyRelative("mixerGroup");
            _initialVolumeProperty = dataProperty.FindPropertyRelative("initialVolume");
            _ambienceMinDelayProperty = dataProperty.FindPropertyRelative("minAmbientDelay");
            _ambienceMaxDelayProperty = dataProperty.FindPropertyRelative("maxAmbientDelay");

            _sfxList = new SfxList(_settings, _itemsProperty);
        }

        public override void OnGUI(string searchContext)
        {
            _settings.Update();

            EditorGUILayout.LabelField("Default SFX System", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_mixerGroupProperty, new GUIContent("Mixer Group"));
            EditorGUILayout.PropertyField(_initialVolumeProperty, new GUIContent("Initial Volume"));
            _ambienceFold = EditorGUILayout.BeginFoldoutHeaderGroup(_ambienceFold, "Ambience Settings");
            if (_ambienceFold)
            {
                if (_ambienceMinDelayProperty.floatValue > _ambienceMaxDelayProperty.floatValue)
                {
                    EditorGUILayout.HelpBox("Min delay is greater than max delay!", MessageType.Error);
                }
                EditorGUILayout.PropertyField(_ambienceMinDelayProperty, new GUIContent("Min Delay"));
                EditorGUILayout.PropertyField(_ambienceMaxDelayProperty, new GUIContent("Max Delay"));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Additional SFX Systems", EditorStyles.boldLabel);

            if (SfxSystemSettings.Singleton.Items.Any(x => string.IsNullOrEmpty(x.Identifier)))
            {
                EditorGUILayout.HelpBox("Some keys are empty!", MessageType.Warning);
            }

            if (SfxSystemSettings.Singleton.Items.HasDoublets(x => x.Identifier))
            {
                EditorGUILayout.HelpBox("Some keys exists more than one times!", MessageType.Warning);
            }

            _sfxList.DoLayoutList();

            _settings.ApplyModifiedProperties();
        }
    }
}