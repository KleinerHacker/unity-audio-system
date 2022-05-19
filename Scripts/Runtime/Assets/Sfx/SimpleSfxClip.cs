using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx
{
    [CreateAssetMenu(menuName = UnityAudioConstants.Menu.Asset.SfxMenu + "/Simple SFX Clip")]
    public sealed class SimpleSfxClip : SfxClip
    {
        #region Inspector Data

        [SerializeField]
        private AudioClipItem clipItem;

        #endregion

        #region Properties

        public AudioClipItem ClipItem => clipItem;

        public override AudioClipItem NextClip => clipItem;

        #endregion
    }
}