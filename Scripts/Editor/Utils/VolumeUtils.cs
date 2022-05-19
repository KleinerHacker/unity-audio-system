using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityAudio.Editor.audio_system.Scripts.Editor.Utils
{
    internal static class VolumeUtils
    {
        public static float CalculateVolume(AudioClip clip)
        {
            var myVol = CalculateVolumeOf(clip);
            var otherVol = CalculateVolumeExcept(new []{clip});

            return otherVol / myVol;
        }
        
        public static float[] CalculateVolumes(AudioClip[] clips)
        {
            try
            {
                var otherVol = CalculateVolumeExcept(clips);
                return clips
                    .Select((x, index) =>
                    {
                        EditorUtility.DisplayProgressBar("Calculate Volumes", x.name, (float)index / (float) clips.Length);
                        return CalculateVolumeOf(x);
                    })
                    .Select(x => otherVol / x)
                    .ToArray();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static float CalculateVolumeOf(AudioClip clip)
        {
            if (!clip.LoadAudioData())
                throw new InvalidOperationException("Unable to load audio data from given clip");

            var data = new float[clip.channels * clip.samples];
            if (!clip.GetData(data, 0))
                throw new InvalidOperationException("Unable to read audio data from given clip");

            var result = data.Average();

            clip.UnloadAudioData();

            return result;
        }

        private static float CalculateVolumeExcept(AudioClip[] clips)
        {
            try
            {
                var assets = AssetDatabase.FindAssets("t:" + nameof(AudioClip));
                return assets
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<AudioClip>)
                    .Where(x => !clips.Contains(x))
                    .Select((audioClip, index) =>
                    {
                        EditorUtility.DisplayProgressBar("Calculate Volumes", audioClip.name, (float)index / (float) assets.Length);
                        return CalculateVolumeOf(audioClip);
                    })
                    .Average();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}