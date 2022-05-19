using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music
{
    [CreateAssetMenu(menuName = UnityAudioConstants.Menu.Asset.SfxMenu + "/Simple Music Clip")]
    public sealed class SimpleMusicClip : MusicClip
    {
        #region Inspector Data

        [SerializeField]
        private AudioClipItemEx clipItem;

        #endregion

        #region Properties

        public AudioClipItemEx ClipItem => clipItem;

        #endregion
    }
}