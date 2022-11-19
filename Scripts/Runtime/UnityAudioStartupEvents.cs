using UnityAssetLoader.Runtime.asset_loader.Scripts.Runtime;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Sfx;
using UnityEngine;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime
{
    public static class UnityAudioStartupEvents
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize()
        {
            Debug.Log("Load SFX settings");
            AssetResourcesLoader.LoadFromResources<SfxSystemSettings>("");
        }
    }
}