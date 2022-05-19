using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
#if !UNITY_EDITOR
using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
#endif

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx
{
    public sealed class SfxSystemSettings : ScriptableObject
    {
        #region Static Area

#if UNITY_EDITOR
        private const string Path = "Assets/Resources/sfx-system.asset";
#endif

        public static SfxSystemSettings Singleton
        {
            get
            {
#if UNITY_EDITOR
                var settings = AssetDatabase.LoadAssetAtPath<SfxSystemSettings>(Path);
                if (settings == null)
                {
                    Debug.Log("Unable to find game settings, create new");

                    settings = new SfxSystemSettings();
                    AssetDatabase.CreateAsset(settings, Path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return settings;
#else
                return AssetResourcesLoader.Instance.GetAsset<SfxSystemSettings>();
#endif
            }
        }

#if UNITY_EDITOR
        public static SerializedObject SerializedSingleton => new SerializedObject(Singleton);
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