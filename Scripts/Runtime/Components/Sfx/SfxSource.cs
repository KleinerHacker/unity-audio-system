using System;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Utils;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityPrefsEx.Runtime.prefs_ex.Scripts.Runtime.Utils;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Sfx
{
    [AddComponentMenu(UnityAudioConstants.Menu.Component.SfxMenu + "/SFX Source")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class SfxSource : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        private string preset;

        #endregion
        
        private AudioSource _audioSource;
        private float _minAmbienceDelay;
        private float _maxAmbienceDelay;

        #region Builtin Methods

        private void Awake()
        {
            var settings = SfxSystemSettings.Singleton;
            var data = string.IsNullOrEmpty(preset) ? settings.Data : settings.Items.FirstOrThrow(x => string.Equals(x.Identifier, preset),
                () => new InvalidOperationException("Unable to find SFX system with identifier " + preset)).Data;
            
            _audioSource = GetComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = data.MixerGroup;
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            
            var volumeSaveKey = (string.IsNullOrWhiteSpace(preset) ? "default" : preset) + ".volume";
            _audioSource.volume = PlayerPrefsEx.GetFloat(volumeSaveKey, data.InitialVolume);
            
            _minAmbienceDelay = data.MinAmbientDelay;
            _maxAmbienceDelay = data.MaxAmbientDelay;
        }

        #endregion
        
        public ISfxPlayedClip PlayInLoop(SfxClip clip) => SfxUtils.PlayInLoop(this, _audioSource, clip);

        public ISfxPlayedClip PlayInLoop(SfxClip clip, float minPlayTime, float maxPlayTime) => 
            SfxUtils.PlayInLoop(this, _audioSource, clip, minPlayTime, maxPlayTime);

        public void PlayOneShot(SfxClip clip) => SfxUtils.PlayOneShot(_audioSource, clip);

        public void PlayOneShot(AudioClip clip) => SfxUtils.PlayOneShot(_audioSource, clip);

        public ISfxPlayedClip PlayAsAmbient(SfxClip clip) => SfxUtils.PlayAsAmbient(this, _audioSource, clip, _minAmbienceDelay, _maxAmbienceDelay);
    }
}