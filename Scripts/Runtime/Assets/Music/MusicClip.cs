using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music
{
    [CreateAssetMenu(menuName = UnityAudioConstants.Menu.Asset.MusicMenu + "/Layered Music Clip")]
    public sealed class MusicClip : ScriptableObject
    {
        #region Inspector Data

        [SerializeField]
        private MusicLayer[] layers = Array.Empty<MusicLayer>();

        #endregion

        #region Properties

        public MusicLayer[] Layers => layers;

        #endregion
    }

    [Serializable]
    public sealed class MusicLayer
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private bool activatedByDefault;

        [Space]
        [SerializeField]
        private MusicVariant[] introVariants = Array.Empty<MusicVariant>();

        [SerializeField]
        private MusicVariant[] loopVariants = Array.Empty<MusicVariant>();

        [SerializeField]
        private MusicVariant[] extroVariants = Array.Empty<MusicVariant>();

        [Space]
        [SerializeField]
        private int defaultIntroVariantIndex = -1;
        
        [SerializeField]
        private int defaultLoopVariantIndex = -1;
        
        [SerializeField]
        private int defaultExtroVariantIndex = -1;

        #endregion

        #region Properties

        public string Name => name;

        public bool ActivatedByDefault => activatedByDefault;

        public MusicVariant[] IntroVariants => introVariants;

        public MusicVariant[] LoopVariants => loopVariants;

        public MusicVariant[] ExtroVariants => extroVariants;

        public int DefaultIntroVariantIndex => defaultIntroVariantIndex;

        public int DefaultLoopVariantIndex => defaultLoopVariantIndex;

        public int DefaultExtroVariantIndex => defaultExtroVariantIndex;

        #endregion
    }

    [Serializable]
    public sealed class MusicVariant
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private AudioClip[] clips = Array.Empty<AudioClip>();

        #endregion

        #region Properties

        public string Name => name;

        public AudioClip[] Clips => clips;

        #endregion
    }
}