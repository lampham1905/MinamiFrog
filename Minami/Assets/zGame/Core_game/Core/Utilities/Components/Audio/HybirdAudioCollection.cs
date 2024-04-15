using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;

namespace Lam.zGame.Core_game.Core.Utilities.Components.Audio
{
    /// <summary>
    /// Hybird audio collection
    /// Require Pre-setup in Json Data, Json data must be seriealized List of SoundsCollection.Sound class
    /// It is suitable for game with many sounds and managed by Excel
    /// </summary>
    [CreateAssetMenu(fileName = "HybirdAudioCollection", menuName = "RUtilities/Hybird Audio Collection")]
    public class HybirdAudioCollection : ScriptableObject
    {
        [System.Serializable]
        public class ClipDefinition
        {
            public string[] fileName;
            public int id;
            public int limitNumber = 1;
        }
        //[System.Serializable]
        //public class Clip
        //{
        //    public string[] fileName;
        //    public int id;
        //    public int limitNumber = 1;
        //    public AudioClip[] clip;

        //    private int countPlay = 0;
        //    public bool isJustPlay = false;
        //    public int CompareTo(Clip other)
        //    {
        //        return id.CompareTo(other.id);
        //    }

        //    public AudioClip GetClipPlay()
        //    {
        //        if (clip.Length > 1)
        //        {
        //            countPlay++;
        //            if (countPlay >= clip.Length)
        //            {
        //                countPlay = 0;
        //            }
        //        }
        //        return clip[countPlay];
        //    }
        //    public AudioClip GetClipNow()
        //    {
        //        return clip[countPlay];
        //    }
        //}

        private static HybirdAudioCollection mInstance;
        public static HybirdAudioCollection Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = Resources.Load<HybirdAudioCollection>("HybirdAudioCollection");
                    mInstance.Reset();
                }
                return mInstance;
            }
        }

        public List<Clip> SFXClips;
        public List<Clip> musicClips;
        public void Reset() {
            int length = SFXClips.Count;
            for (int i = 0; i < length; i++)
            {
                SFXClips[i].Reset();
            }
            length = musicClips.Count;
            for (int i = 0; i < length; i++)
            {
                musicClips[i].Reset();
            }
        }
        public Clip GetClipById(int pId, bool pIsMusic, out int pIndex)
        {
            var list = pIsMusic ? musicClips : SFXClips;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                pIndex = i;
                if (s.id == pId)
                    return s;
            }
            pIndex = -1;
            return null;
        }

        public Clip GetClipByIndex(int pIndex, bool pIsMusic)
        {
            var list = pIsMusic ? musicClips : SFXClips;
            if (pIndex >= list.Count || pIndex < 0)
                return null;
            return list[pIndex];
        }

        public Clip GetClip(string pName, bool pIsMusic, out int pIndex)
        {
            var list = pIsMusic ? musicClips : SFXClips;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                pIndex = i;
                if (s.fileName[0].ToLower() == pName.ToLower())
                    return s;
            }
            pIndex = -1;
            return null;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(HybirdAudioCollection))]
        private class SoundsCollectionEditor : Editor
        {
            private HybirdAudioCollection mScript;

            private void OnEnable()
            {
                mScript = target as HybirdAudioCollection;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                GUILayout.Space(5);
                DrawCustom();
            }

            private void DrawCustom()
            {
                EditorHelper.BoxVertical(() =>
                {
                    string musicsSourcePath = EditorHelper.FolderSelector("Musics Sources Path", "musicsSourcePath", Application.dataPath);
                    string sfxsSourcePath = EditorHelper.FolderSelector("SFX Sources Path", "sfxsSourcePath", Application.dataPath);

                    if (EditorHelper.Button("Build"))
                    {
                        musicsSourcePath = Application.dataPath + musicsSourcePath;
                        sfxsSourcePath = Application.dataPath + sfxsSourcePath;

                        var musicFiles = EditorHelper.GetObjects<AudioClip>(musicsSourcePath, "t:AudioClip");
                        string musicsJsonData = Resources.Load<TextAsset>("Data/Musics")?.text;

                        var listMusicClip = JsonHelper.GetJsonList<ClipDefinition>(musicsJsonData);
                        mScript.musicClips = new List<Clip>();
                        int length = listMusicClip.Count;
                        for (int i = 0; i < length; i++)
                        {
                            Clip sound = new Clip();
                            sound.fileName = listMusicClip[i].fileName;
                            sound.id = listMusicClip[i].id;
                            sound.limitNumber = listMusicClip[i].limitNumber;

                            int lengthF = listMusicClip[i].fileName.Length;
                            sound.clip = new AudioClip[lengthF];
                            for (int k = 0; k < lengthF; k++)
                            {
                                bool found = false;
                                foreach (AudioClip f in musicFiles)
                                {
                                    if (sound.fileName == null) continue;
                                    if (f.name.ToLower().Equals(listMusicClip[i].fileName[k].ToLower()))
                                    {
                                        found = true;
                                        sound.clip[k] = f;
                                        break;
                                    }
                                }
                                if (!found)
                                    UnityEngine.Debug.LogError("Not found music audio file " + sound.fileName[k]);
                            }

                            mScript.musicClips.Add(sound);
                        }
                        //----
                        //mScript.musicClips = JsonHelper.GetJsonList<Clip>(musicsJsonData);
                        //if (mScript.musicClips != null)
                        //{
                        //    foreach (var sound in mScript.musicClips)
                        //    {
                        //        bool found = false;
                        //        foreach (var clip in musicFiles)
                        //        {
                        //            if (sound.fileName == null) continue;
                        //            if (sound.fileName.ToLower() == clip.name.ToLower())
                        //            {
                        //                found = true;
                        //                sound.clip = clip;
                        //                break;
                        //            }
                        //        }
                        //        if (!found)
                        //            UnityEngine.Debug.LogError("Not found music audio file " + sound.fileName);
                        //    }
                        //}
                        //--------------
                        var sfxFiles = EditorHelper.GetObjects<AudioClip>(sfxsSourcePath, "t:AudioClip");
                        string sFXsJsonData = Resources.Load<TextAsset>("Data/SFXs")?.text;

                        var listSoundClip = JsonHelper.GetJsonList<ClipDefinition>(sFXsJsonData);
                        mScript.SFXClips = new List<Clip>();
                        length = listSoundClip.Count;
                        for (int i = 0; i < length; i++)
                        {
                            Clip sound = new Clip();
                            sound.fileName = listSoundClip[i].fileName;
                            sound.id = listSoundClip[i].id;
                            sound.limitNumber = listSoundClip[i].limitNumber;

                            int lengthF = listSoundClip[i].fileName.Length;
                            sound.clip = new AudioClip[lengthF];
                            for (int k = 0; k < lengthF; k++)
                            {
                                bool found = false;
                                foreach (AudioClip f in sfxFiles)
                                {
                                    if (sound.fileName == null) continue;
                                    if (f.name.ToLower().Equals(listSoundClip[i].fileName[k].ToLower()))
                                    {
                                        found = true;
                                        sound.clip[k] = f;
                                        break;
                                    }
                                }
                                if (!found)
                                    UnityEngine.Debug.LogError("Not found music audio file " + sound.fileName[k]);
                            }

                            mScript.SFXClips.Add(sound);
                        }
                        //-----------------
                        //mScript.SFXClips = JsonHelper.GetJsonList<Clip>(sFXsJsonData);
                        //if (mScript.SFXClips != null)
                        //{
                        //    foreach (var sound in mScript.SFXClips)
                        //    {
                        //        bool found = false;
                        //        foreach (var clip in sfxFiles)
                        //        {
                        //            if (sound.fileName == null) continue;
                        //            if (sound.fileName.ToLower() == clip.name.ToLower())
                        //            {
                        //                found = true;
                        //                sound.clip = clip;
                        //                break;
                        //            }
                        //        }
                        //        if (!found)
                        //            UnityEngine.Debug.LogError("Not found sfx audio file " + sound.fileName);
                        //    }
                        //}
                        //-------------------------
                        if (GUI.changed)
                        {
                            EditorUtility.SetDirty(mScript);
                            AssetDatabase.SaveAssets();
                        }
                    }
                }, ColorHelper.DarkMagenta, true);
            }
        }
#endif
    }
}