using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx
{
    public abstract class SfxClip : ScriptableObject
    {
        #region Properties

        public abstract AudioClipItem NextClip { get; }

        #endregion
    }
}