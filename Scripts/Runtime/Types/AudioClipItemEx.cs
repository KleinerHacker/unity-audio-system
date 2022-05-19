using System;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Types
{
    [Serializable]
    public class AudioClipItemEx : AudioClipItem
    {
        #region Inspector Data

        [SerializeField]
        private AudioClip inClip;
        
        [SerializeField]
        private AudioClip outClip;

        #endregion

        #region Properties

        public AudioClip InClip => inClip;

        public AudioClip OutClip => outClip;

        #endregion
    }
}