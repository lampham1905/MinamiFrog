#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.Audio
{
    public class AudioMenuTools : Editor
    {
        [MenuItem("RUtilities/Audio/Add Audio Manager")]
        private static void AddAudioManager()
        {
            var manager = GameObject.FindObjectOfType<AudioManager>();
            if (manager == null)
            {
                var obj = new GameObject("AudioManager");
                obj.AddComponent<AudioManager>();
            }
        }

        [MenuItem("RUtilities/Audio/Add Hybird Audio Manager")]
        private static void AddHybirdAudioManager()
        {
            var manager = GameObject.FindObjectOfType<HybirdAudioManager>();
            if (manager == null)
            {
                var obj = new GameObject("HybirdAudioManager");
                obj.AddComponent<HybirdAudioManager>();
            }
        }

        [MenuItem("RUtilities/Audio/Open Audio Collection")]
        private static void OpenAudioCollection()
        {
            Selection.activeObject = AudioCollection.Instance;
        }

        [MenuItem("RUtilities/Audio/Open Hybird Audio Collection")]
        private static void OpenHybirdAudioCollection()
        {
            Selection.activeObject = HybirdAudioCollection.Instance;
        }
    }
}
#endif