using System;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx
{
    [CreateAssetMenu(menuName = UnityAudioConstants.Menu.Asset.SfxMenu + "/Random SFX Clip")]
    public sealed class RandomSfxClip : SfxClip
    {
        #region Inspector Data

        [SerializeField]
        private AudioClipItem[] items = Array.Empty<AudioClipItem>();

        #endregion

        #region Properties

        public AudioClipItem[] Items => items;

        public override AudioClipItem NextClip => items.GetRandom();

        #endregion
    }
}