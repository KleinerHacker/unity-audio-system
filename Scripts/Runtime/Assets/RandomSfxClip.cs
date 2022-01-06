using System;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Types;

namespace UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets
{
    [CreateAssetMenu(menuName = UnitySfxConstants.Menu.Asset.Root + "/Random SFX Clip")]
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