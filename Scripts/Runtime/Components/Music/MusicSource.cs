using System;
using System.Collections.Generic;
using System.Linq;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Music
{
    [AddComponentMenu(UnityAudioConstants.Menu.Component.MusicMenu + "/Music Source")]
    [DisallowMultipleComponent]
    public sealed class MusicSource : MonoBehaviour, IMusicPlayer
    {
        #region Inspector Data

        [Header("Data")]
        [SerializeField]
        private AudioMixerGroup mixerGroup;

        [Header("Fading")]
        [SerializeField]
        private AnimationCurve fadingCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField]
        private float fadingSpeed = 1f;

        #endregion

        #region Properties

        public MusicPlayer.MusicPlayerState State => _musicPlayer.State;

        public bool IsPlaying => _musicPlayer.IsPlaying;

        #endregion

        private readonly IDictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();
        private readonly MusicPlayer _musicPlayer = new MusicPlayer();

        public MusicSource()
        {
            _musicPlayer.PrepareAudio += (_, args) =>
            {
                foreach (var audioSource in _audioSources.Values)
                {
                    Destroy(audioSource);
                }

                _audioSources.Clear();

                foreach (var layer in args.LayerNames)
                {
                    _audioSources.Add(layer, CreateAudioSource());
                }
            };
            _musicPlayer.PlayAudio += (_, args) =>
            {
                var audioSource = _audioSources[args.LayerName];
                audioSource.loop = args.Loop;
                audioSource.clip = args.Clip;
                if (args.StayInTime && _audioSources.Values.Any(x => x.isPlaying))
                {
                    audioSource.time = _audioSources.Values.First(x => x.isPlaying).time;
                }

                audioSource.Play();
            };
            _musicPlayer.StopAllAudio += (_, _) =>
            {
                foreach (var audioSource in _audioSources.Values)
                {
                    audioSource.Stop();
                }
            };
            _musicPlayer.StopAudio += (_, args) =>
            {
                _audioSources[args.LayerName].Stop();
            };
            _musicPlayer.MarkedForFinishingAudio += (_, args) =>
            {
                _audioSources[args.LayerName].loop = false;
            };
        }

        #region Builtin Methods

        private void FixedUpdate()
        {
            _musicPlayer.OnUpdateAudioState(() => _audioSources.Values.Any(x => !x.isPlaying));
        }

        #endregion

        public void Play(MusicClip clip, bool playExtroFromCurrent = false, bool playIntroFromNew = false)
        {
            _musicPlayer.Play(clip, playExtroFromCurrent, playIntroFromNew);
        }

        public void Stop()
        {
            _musicPlayer.Stop();
        }

        public void FinishAndStop(bool finishImmediately = false)
        {
            _musicPlayer.FinishAndStop(finishImmediately);
        }

        public void ChangeLayer(params string[] activeLayers)
        {
            _musicPlayer.ChangeLayer(activeLayers);
        }

        public void ChangeVariant(string variant)
        {
            _musicPlayer.ChangeVariant(variant);
        }

        public void ResetLayersAndVariant()
        {
            _musicPlayer.ResetLayersAndVariant();
        }

        private AudioSource CreateAudioSource()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = mixerGroup;

            return audioSource;
        }
    }
}