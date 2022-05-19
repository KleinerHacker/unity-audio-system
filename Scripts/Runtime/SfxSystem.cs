using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Components;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Sfx;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime
{
    public static class SfxSystem
    {
        public static SfxSystemInstance Default => new SfxSystemInstance(SfxController.DefaultController);

        public static SfxSystemInstance Get(string identifier) =>
            SfxController.CustomControllers.ContainsKey(identifier) ? new SfxSystemInstance(SfxController.CustomControllers[identifier]) : null;
    }

    public sealed class SfxSystemInstance
    {
        private readonly SfxController _controller;

        internal SfxSystemInstance(SfxController controller)
        {
            _controller = controller;
        }

        public string Key => _controller.Key;

        public float Volume
        {
            get => _controller.Volume;
            set => _controller.Volume = value;
        }

        public ISfxPlayedClip PlayInLoop(SfxClip clip) => _controller.PlayInLoop(clip);
        
        public ISfxPlayedClip PlayInLoop(SfxClip clip, float minPlayTime, float maxPlayTime) => _controller.PlayInLoop(clip, minPlayTime, maxPlayTime);

        public void PlayOneShot(SfxClip clip) => _controller.PlayOneShot(clip);

        public void PlayOneShot(AudioClip clip) => _controller.PlayOneShot(clip);

        public ISfxPlayedClip PlayAsAmbient(SfxClip clip) => _controller.PlayAsAmbient(clip);
    }
}