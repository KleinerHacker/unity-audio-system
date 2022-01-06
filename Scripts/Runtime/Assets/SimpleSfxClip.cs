using UnityEngine;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Types;

namespace UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets
{
    [CreateAssetMenu(menuName = UnitySfxConstants.Menu.Asset.Root + "/Simple SFX Clip")]
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