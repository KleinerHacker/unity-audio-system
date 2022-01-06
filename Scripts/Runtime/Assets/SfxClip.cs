using UnityEngine;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Types;

namespace UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets
{
    public abstract class SfxClip : ScriptableObject
    {
        #region Properties

        public abstract AudioClipItem NextClip { get; }

        #endregion
    }
}