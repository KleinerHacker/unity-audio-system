using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityPrefsEx.Runtime.prefs_ex.Scripts.Runtime.Utils;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Sfx
{
    public sealed class SfxController : MonoBehaviour
    {
        #region Static Area

        internal static SfxController DefaultController { get; private set; }
        internal static IReadOnlyDictionary<string, SfxController> CustomControllers => new ReadOnlyDictionary<string, SfxController>(_customControllers);

        private static readonly IDictionary<string, SfxController> _customControllers = new Dictionary<string, SfxController>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadSfxSystem()
        {
            var settings = SfxSystemSettings.Singleton;
            
            var go = new GameObject("SFX System");
            DontDestroyOnLoad(go);
            DefaultController = go.AddComponent<SfxController>(); //Default
            DefaultController.Initialize(settings.Data, null);

            foreach (var item in settings.Items)
            {
                var customGo = new GameObject("SFX Custom System " + item.Identifier);
                customGo.transform.SetParent(go.transform);
                DontDestroyOnLoad(customGo);
                
                var sfxController = customGo.AddComponent<SfxController>();
                sfxController.Key = item.Identifier;
                sfxController.Initialize(item.Data, item.Identifier);

                _customControllers.Add(item.Identifier, sfxController);
            }
        }

        #endregion

        #region Properties

        private AudioMixerGroup _audioMixerGroup;
        
        internal string Key { get; set; }

        internal float Volume
        {
            get => _audioSource.volume;
            set
            {
                _audioSource.volume = value;
                if (!string.IsNullOrWhiteSpace(_volumeSaveKey))
                {
                    PlayerPrefsEx.SetFloat(_volumeSaveKey, value, true);
                }
                else
                {
                    Debug.LogWarning("Unable to save new volume for sfx system into player prefs: Not initialized yet");
                }
            }
        }

        #endregion

        private AudioSource _audioSource;
        private float _minAmbienceDelay;
        private float _maxAmbienceDelay;

        private string _volumeSaveKey;

        #region Builtin Methods

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        } 

        #endregion

        internal void Initialize(SfxData data, string systemKey)
        {
            _audioSource.outputAudioMixerGroup = data.MixerGroup;
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;

            _volumeSaveKey = (string.IsNullOrWhiteSpace(systemKey) ? "default" : systemKey) + ".volume";
            _audioSource.volume = PlayerPrefsEx.GetFloat(_volumeSaveKey, data.InitialVolume);

            _minAmbienceDelay = data.MinAmbientDelay;
            _maxAmbienceDelay = data.MaxAmbientDelay;
        }

        internal ISfxPlayedClip PlayInLoop(SfxClip clip) => SfxUtils.PlayInLoop(this, _audioSource, clip);

        internal ISfxPlayedClip PlayInLoop(SfxClip clip, float minPlayTime, float maxPlayTime) => 
            SfxUtils.PlayInLoop(this, _audioSource, clip, minPlayTime, maxPlayTime);

        internal void PlayOneShot(SfxClip clip) => SfxUtils.PlayOneShot(_audioSource, clip);

        internal void PlayOneShot(AudioClip clip) => SfxUtils.PlayOneShot(_audioSource, clip);

        internal ISfxPlayedClip PlayAsAmbient(SfxClip clip) => SfxUtils.PlayAsAmbient(this, _audioSource, clip, _minAmbienceDelay, _maxAmbienceDelay);
    }
}