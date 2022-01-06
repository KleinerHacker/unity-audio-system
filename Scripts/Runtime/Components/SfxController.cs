using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityAnimation.Runtime.animation.Scripts.Runtime.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityPrefsEx.Runtime.prefs_ex.Scripts.Runtime.Utils;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Types;
using Random = UnityEngine.Random;

namespace UnitySfx.Runtime.sfx_system.Scripts.Runtime.Components
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
            DefaultController.Initialize(settings.Data);

            foreach (var item in settings.Items)
            {
                var customGo = new GameObject("SFX Custom System " + item.Identifier);
                customGo.transform.SetParent(go.transform);
                DontDestroyOnLoad(customGo);
                
                var sfxController = customGo.AddComponent<SfxController>();
                sfxController.Key = item.Identifier;
                sfxController.Initialize(item.Data);

                _customControllers.Add(item.Identifier, sfxController);
            }
        }

        #endregion

        #region Properties

        private float _volume;
        private AudioMixerGroup _audioMixerGroup;
        
        internal string Key { get; set; }
        private string PrefKey => "sfx-system." + Key;

        internal float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _audioSource.volume = value;
                PlayerPrefsEx.SetFloat(PrefKey, value, true);
            }
        }

        #endregion

        private AudioSource _audioSource;
        private float _minAmbienceDelay;
        private float _maxAmbienceDelay;

        #region Builtin Methods

        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion

        internal void Initialize(SfxData data)
        {
            Volume = PlayerPrefsEx.GetFloat(PrefKey, data.InitialValue);
            _audioSource.outputAudioMixerGroup = data.MixerGroup;
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }

        internal ISfxPlayedClip PlayInLoop(SfxClip clip)
        {
            var playedClip = new SfxLoopPlayedClip();
            PlayLooped(clip, playedClip);

            return playedClip;
        }
        
        private void PlayLooped(SfxClip clip, SfxLoopPlayedClip playedClip)
        {
            if (playedClip.IsStop)
                return;
                
            var item = clip.NextClip;
            _audioSource.PlayOneShot(item.AudioClip, item.Volume);

            AnimationBuilder.Create(this)
                .Wait(item.AudioClip.length, () => PlayLooped(clip, playedClip))
                .Start();
        }

        internal void PlayOneShot(SfxClip clip)
        {
            var item = clip.NextClip;
            _audioSource.PlayOneShot(item.AudioClip, item.Volume);
        }

        internal void PlayOneShot(AudioClip clip) => _audioSource.PlayOneShot(clip);

        internal ISfxPlayedClip PlayAsAmbient(SfxClip clip)
        {
            var playedClip = new SfxAmbiencePlayedClip();
            PlayAmbient(clip, playedClip);

            return playedClip;
        }

        private void PlayAmbient(SfxClip clip, SfxAmbiencePlayedClip playedClip)
        {
            AnimationBuilder.Create(this)
                .Wait(Random.Range(_minAmbienceDelay, _maxAmbienceDelay), () =>
                {
                    if (playedClip.IsStop)
                        return;
                    
                    PlayOneShot(clip);
                    PlayAmbient(clip, playedClip);
                })
                .Start();
        }

        private sealed class SfxLoopPlayedClip : ISfxPlayedClip
        {
            internal bool IsStop { get; private set; }
            
            public void Stop()
            {
                IsStop = true;
            }
        }

        private sealed class SfxAmbiencePlayedClip : ISfxPlayedClip
        {
            internal bool IsStop { get; private set; }
            
            public void Stop()
            {
                IsStop = true;
            }
        }
    }

    public interface ISfxPlayedClip
    {
        void Stop();
    }
}