using System;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Types;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music
{
    [CreateAssetMenu(menuName = UnityAudioConstants.Menu.Asset.SfxMenu + "/Combined SFX Clip")]
    public sealed class CombinedMusicClip : MusicClip
    {
        #region Inspector Data

        [SerializeField]
        private CombinedMusicClipItem[] items = Array.Empty<CombinedMusicClipItem>();

        [SerializeField]
        private CombinedMusicClipPreset[] presets = Array.Empty<CombinedMusicClipPreset>();

        #endregion

        #region Properties

        public CombinedMusicClipItem[] Items => items;

        public CombinedMusicClipPreset[] Presets => presets;

        #endregion
    }

    [Serializable]
    public sealed class CombinedMusicClipItem
    {
        #region Inspector Data

        [SerializeField]
        private string identifier;

        [SerializeField]
        private AudioClipItemEx clipItem;

        #endregion

        #region Properties

        public string Identifier => identifier;

        public AudioClipItemEx ClipItem => clipItem;

        #endregion
    }

    [Serializable]
    public sealed class CombinedMusicClipPreset
    {
        #region Inspector Data

        [SerializeField]
        private string identifier;

        [SerializeField]
        private string[] clipItemReferences = Array.Empty<string>();

        #endregion

        #region Properties

        public string Identifier => identifier;

        public string[] ClipItemReferences => clipItemReferences;

        #endregion
    }
}