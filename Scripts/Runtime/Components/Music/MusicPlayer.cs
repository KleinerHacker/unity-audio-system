using System;
using System.Linq;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Music
{
    public sealed class MusicPlayer : IMusicPlayer
    {
        #region Properties

        public MusicClip Clip { get; private set; }
        public string CurrentVariant { get; private set; }
        public string[] CurrentLayers => _currentLayers.ToArray(); //Copy
        
        public MusicPlayerState State { get; private set; } = MusicPlayerState.Stopped;

        public bool IsPlaying => State != MusicPlayerState.Stopped;

        #endregion

        #region Events

        public event EventHandler<AudioPrepareEventArgs> PrepareAudio; 
        public event EventHandler<AudioPlayEventArgs> PlayAudio;
        public event EventHandler StopAllAudio;
        public event EventHandler<AudioStopEventArgs> StopAudio; 
        public event EventHandler<AudioStopEventArgs> MarkedForFinishingAudio;

        #endregion
        
        private string[] _currentLayers;
        private bool _markedForFinishing = false;

        public void Play(MusicClip clip, bool playExtroFromCurrent = false, bool playIntroFromNew = false)
        {
            Clip = clip;
            
            UpdateLayersAndVariants();
            PrepareAudio?.Invoke(this, new AudioPrepareEventArgs(clip.Layers.Select(x => x.Name).ToArray()));
            
            if (playIntroFromNew)
            {
                PlayIntro(false);
                return;
            }

            PlayLoop(false);
        }
        
        public void Stop()
        {
            StopAllAudio?.Invoke(this, EventArgs.Empty);
            State = MusicPlayer.MusicPlayerState.Stopped;
        }
        
        public void FinishAndStop(bool finishImmediately = false)
        {
            if (finishImmediately)
            {
                PlayExtro(false);
                return;
            }

            _markedForFinishing = true;
            foreach (var layer in Clip.Layers)
            {
                MarkedForFinishingAudio?.Invoke(this, new AudioStopEventArgs(layer.Name));
            }
        }
        
        public void ChangeLayer(params string[] activeLayers)
        {
            var layersToStop = _currentLayers.Where(x => !activeLayers.Contains(x));
            var layersToStart = activeLayers.Where(x => !_currentLayers.Contains(x)).ToArray();

            foreach (var layer in layersToStop)
            {
                StopAudio?.Invoke(this, new AudioStopEventArgs(layer));
            }

            switch (State)
            {
                case MusicPlayer.MusicPlayerState.Stopped:
                    break;
                case MusicPlayer.MusicPlayerState.PlayIntro:
                    PlayVariant(x => x.IntroVariants, x => x.DefaultIntroVariantIndex, false, true, layersToStart);
                    break;
                case MusicPlayer.MusicPlayerState.PlayLoop:
                    PlayVariant(x => x.LoopVariants, x => x.DefaultLoopVariantIndex, !_markedForFinishing, true, layersToStart);
                    break;
                case MusicPlayer.MusicPlayerState.PlayExtro:
                    PlayVariant(x => x.ExtroVariants, x => x.DefaultExtroVariantIndex, false, true, layersToStart);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentLayers = activeLayers;
        }
        
        public void ChangeVariant(string variant)
        {
            CurrentVariant = variant;

            switch (State)
            {
                case MusicPlayerState.Stopped:
                    break;
                case MusicPlayerState.PlayIntro:
                    PlayVariant(x => x.IntroVariants, x => x.DefaultIntroVariantIndex, false, false);
                    break;
                case MusicPlayerState.PlayLoop:
                    PlayVariant(x => x.LoopVariants, x => x.DefaultLoopVariantIndex, !_markedForFinishing, false);
                    break;
                case MusicPlayerState.PlayExtro:
                    PlayVariant(x => x.ExtroVariants, x => x.DefaultExtroVariantIndex, false, false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void ResetLayersAndVariant()
        {
            ChangeLayer(Clip.Layers
                .Where(x => x.ActivatedByDefault)
                .Select(x => x.Name)
                .ToArray());
            ChangeVariant(null);
        }

        public void OnUpdateAudioState(bool isAnyAudioNotPlaying)
        {
            OnUpdateAudioState(() => isAnyAudioNotPlaying);
        }

        public void OnUpdateAudioState(Func<bool> isAnyAudioNotPlaying)
        {
            if (State == MusicPlayerState.Stopped)
                return;

            if (isAnyAudioNotPlaying.Invoke())
            {
                switch (State)
                {
                    case MusicPlayerState.PlayIntro:
                        PlayLoop(false);
                        break;
                    case MusicPlayerState.PlayLoop:
                        if (_markedForFinishing)
                        {
                            PlayExtro(false);
                            _markedForFinishing = false;
                        }

                        break;
                    case MusicPlayerState.PlayExtro:
                        State = MusicPlayerState.Stopped;
                        break;
                    case MusicPlayerState.Stopped:
                    default:
                        throw new NotImplementedException(State.ToString());
                }
            }
        }
        
        private void UpdateLayersAndVariants()
        {
            CurrentVariant = null;
            _currentLayers = Clip.Layers
                .Where(x => x.ActivatedByDefault)
                .Select(x => x.Name)
                .ToArray();
        }
        
        private void PlayIntro(bool stayInTime)
        {
            PlayVariant(x => x.IntroVariants, x => x.DefaultIntroVariantIndex, false, stayInTime);
            State = MusicPlayerState.PlayIntro;
        }

        private void PlayLoop(bool stayInTime)
        {
            PlayVariant(x => x.LoopVariants, x => x.DefaultLoopVariantIndex, true, stayInTime);
            State = MusicPlayerState.PlayLoop;
        }

        private void PlayExtro(bool stayInTime)
        {
            PlayVariant(x => x.ExtroVariants, x => x.DefaultExtroVariantIndex, false, stayInTime);
            State = MusicPlayerState.PlayLoop;
        }
        
        private void PlayVariant(Func<MusicLayer, MusicVariant[]> variantsExtractor, Func<MusicLayer, int> variantIndexExtractor, bool loop, bool stayInTime, string[] layers = null)
        {
            var filteredLayers = layers == null ? 
                Clip.Layers.Where(x => _currentLayers.Contains(x.Name)) : 
                Clip.Layers.Where(x => layers.Contains(x.Name));
            foreach (var layer in filteredLayers)
            {
                var variants = variantsExtractor(layer);
                var variantIndex = variantIndexExtractor(layer);
                
                var variant = string.IsNullOrEmpty(CurrentVariant) ? variants[variantIndex] : variants.First(x => x.Name == CurrentVariant);
                PlayAudio?.Invoke(this, new AudioPlayEventArgs(layer.Name, variant.Clips.GetRandom(), loop, stayInTime));
            }
        }

        public class AudioPrepareEventArgs : EventArgs
        {
            public string[] LayerNames { get; }

            public AudioPrepareEventArgs(string[] layerNames)
            {
                LayerNames = layerNames;
            }
        }

        public class AudioPlayEventArgs : EventArgs
        {
            public string LayerName { get; }
            public AudioClip Clip { get; }
            public bool Loop { get; }
            public bool StayInTime { get; }

            public AudioPlayEventArgs(string layerName, AudioClip clip, bool loop, bool stayInTime)
            {
                LayerName = layerName;
                Clip = clip;
                Loop = loop;
                StayInTime = stayInTime;
            }
        }

        public class AudioStopEventArgs : EventArgs
        {
            public string LayerName { get; }

            public AudioStopEventArgs(string layerName)
            {
                LayerName = layerName;
            }
        }
        
        public enum MusicPlayerState
        {
            Stopped,
            PlayIntro,
            PlayLoop,
            PlayExtro,
        }
    }
}