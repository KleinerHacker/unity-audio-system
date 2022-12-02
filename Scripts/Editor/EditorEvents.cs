using UnityAudio.Editor.audio_system.Scripts.Editor.Components.Music;
using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music;
using UnityEditor;
using UnityEditor.Callbacks;

namespace UnityAudio.Editor.audio_system.Scripts.Editor
{
    internal static class EditorEvents
    {
        [OnOpenAsset]
        public static bool OnOpenLayeredMusicClip(int instanceID, int line)
        {
            var layeredMusicClip = EditorUtility.InstanceIDToObject(instanceID) as MusicClip;
            if (layeredMusicClip == null)
                return false;
            
            LayeredMusicClipEditorWindow.ShowWindow();
            return true;
        }
    }
}