using UnityAnimation.Runtime.animation.Scripts.Runtime.Utils;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Utils
{
    internal static class SfxUtils
    {
        public static ISfxPlayedClip PlayInLoop(MonoBehaviour behaviour, AudioSource audioSource, SfxClip clip)
        {
            var playedClip = new SfxLoopPlayedClip();
            PlayLooped(behaviour, audioSource, clip, playedClip);

            return playedClip;
        }

        private static void PlayLooped(MonoBehaviour behaviour, AudioSource audioSource, SfxClip clip, SfxLoopPlayedClip playedClip)
        {
            if (playedClip.IsStop)
                return;

            var item = clip.NextClip;
            audioSource.PlayOneShot(item.AudioClip, item.Volume);

            AnimationBuilder.Create(behaviour)
                .Wait(item.AudioClip.length, () => PlayLooped(behaviour, audioSource, clip, playedClip))
                .Start();
        }

        public static ISfxPlayedClip PlayInLoop(MonoBehaviour behaviour, AudioSource audioSource, SfxClip clip, float minPlayTime, float maxPlayTime)
        {
            var playedClip = new SfxLoopPlayedClip();
            PlayLooped(behaviour, audioSource, clip, playedClip, minPlayTime, maxPlayTime);

            return playedClip;
        }

        private static void PlayLooped(MonoBehaviour behaviour, AudioSource audioSource, SfxClip clip, SfxLoopPlayedClip playedClip, float minPlayTime, float maxPlayTime)
        {
            if (playedClip.IsStop)
            {
                audioSource.Stop();
                audioSource.volume = 1f;
                audioSource.clip = null;

                return;
            }

            var item = clip.NextClip;
            audioSource.clip = item.AudioClip;
            audioSource.volume = item.Volume;
            audioSource.Play();

            AnimationBuilder.Create(behaviour)
                .Wait(Random.Range(minPlayTime, maxPlayTime), () => PlayLooped(behaviour, audioSource, clip, playedClip, minPlayTime, maxPlayTime))
                .Start();
        }

        public static void PlayOneShot(AudioSource audioSource, SfxClip clip)
        {
            var item = clip.NextClip;
            audioSource.PlayOneShot(item.AudioClip, item.Volume);
        }

        public static void PlayOneShot(AudioSource audioSource, AudioClip clip) => audioSource.PlayOneShot(clip);

        public static ISfxPlayedClip PlayAsAmbient(MonoBehaviour behaviour, AudioSource audioSource, SfxClip clip, float minAmbienceDelay, float maxAmbienceDelay)
        {
            var playedClip = new SfxAmbiencePlayedClip();
            PlayAmbient(behaviour, audioSource, clip, playedClip, minAmbienceDelay, maxAmbienceDelay);

            return playedClip;
        }

        private static void PlayAmbient(MonoBehaviour behaviour, AudioSource audioSource, SfxClip clip, SfxAmbiencePlayedClip playedClip, 
            float minAmbienceDelay, float maxAmbienceDelay)
        {
            AnimationBuilder.Create(behaviour)
                .Wait(Random.Range(minAmbienceDelay, maxAmbienceDelay), () =>
                {
                    if (playedClip.IsStop)
                        return;

                    PlayOneShot(audioSource, clip);
                    PlayAmbient(behaviour, audioSource, clip, playedClip, minAmbienceDelay, maxAmbienceDelay);
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
}