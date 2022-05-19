using System;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Types
{
    [Serializable]
    public sealed class AudioClipItem
    {
        #region Inspector Data

        [SerializeField]
        private AudioClip audioClip;

        [SerializeField]
        private float volume = 1f;

        #endregion

        #region Properties

        public AudioClip AudioClip => audioClip;

        public float Volume => volume;

        #endregion
    }
}