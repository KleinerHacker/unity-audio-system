using System;
using UnityEditor;
using UnityEditorEx.Runtime.editor_ex.Scripts.Runtime.Assets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx
{
    public sealed class SfxSystemSettings : ProviderAsset<SfxSystemSettings>
    {
        #region Static Area

        public static SfxSystemSettings Singleton => GetSingleton("SFX System", "sfx-system.asset");

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => GetSerializedSingleton("SFX System", "sfx-system.asset");
#endif

        #endregion

        #region Inspector Data

        [SerializeField]
        private SfxData data;

        [SerializeField]
        private SfxSystemItem[] items;

        #endregion

        #region Properties

        public SfxData Data => data;

        public SfxSystemItem[] Items => items;

        #endregion
    }

    [Serializable]
    public sealed class SfxSystemItem
    {
        #region Inspector Data

        [SerializeField]
        private string identifier;

        [SerializeField]
        private SfxData data;

        #endregion

        #region Properties

        public string Identifier => identifier;

        public SfxData Data => data;

        #endregion
    }

    [Serializable]
    public sealed class SfxData
    {
        #region Inspector Data

        [SerializeField]
        private float minAmbientDelay = 1f;

        [SerializeField]
        private float maxAmbientDelay = 2f;

        [SerializeField]
        private AudioMixerGroup mixerGroup;

        [SerializeField]
        [FormerlySerializedAs("initialValue")]
        [Range(0f, 1f)]
        private float initialVolume = 1f;

        #endregion

        #region Properties

        public float MinAmbientDelay => minAmbientDelay;

        public float MaxAmbientDelay => maxAmbientDelay;

        public AudioMixerGroup MixerGroup => mixerGroup;

        public float InitialVolume => initialVolume;

        #endregion
    }
}