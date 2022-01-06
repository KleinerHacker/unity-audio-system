using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime.Loader;
using UnityEngine;
using UnitySfx.Runtime.sfx_system.Scripts.Runtime.Assets;

namespace UnitySfx.Runtime.sfx_system.Scripts.Runtime
{
    public static class UnitySfxStartupEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            Debug.Log("Load SFX settings");
            AssetResourcesLoader.Instance.LoadAssets<SfxSystemSettings>("");
        }
    }
}