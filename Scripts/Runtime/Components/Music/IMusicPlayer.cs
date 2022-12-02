using UnityAudio.Runtime.audio_system.Scripts.Runtime.Assets.Music;

namespace UnityAudio.Runtime.audio_system.Scripts.Runtime.Components.Music
{
    public interface IMusicPlayer
    {
        public void Play(MusicClip clip, bool playExtroFromCurrent = false, bool playIntroFromNew = false);
        public void Stop();
        public void FinishAndStop(bool finishImmediately = false);
        public void ChangeLayer(params string[] activeLayers);
        public void ChangeVariant(string variant);
        public void ResetLayersAndVariant();
    }
}