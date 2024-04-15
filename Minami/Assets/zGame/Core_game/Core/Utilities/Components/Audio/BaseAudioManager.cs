//#define USE_DOTWEEN

#if USE_DOTWEEN
using DG.Tweening;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = Lam.zGame.Core_game.Core.Utilities.Common.Debug.Debug;

namespace Lam.zGame.Core_game.Core.Utilities.Components.Audio
{
    [System.Serializable]
    public class Clip
    {
        public string[] fileName;
        public int id;
        public int limitNumber = 1;
        public AudioClip[] clip;

        private int countPlay = 0;
        public bool isJustPlay = false;
        public int CompareTo(Clip other)
        {
            return id.CompareTo(other.id);
        }

        public AudioClip GetClipPlaySFX()
        {
            if (clip.Length > 1)
            {
                countPlay++;
                if (countPlay >= clip.Length)
                {
                    countPlay = 0;
                }
            }
            else
            {
                countPlay = 0;
            }
            return clip[countPlay];
        }
        public AudioClip GetClipPlayMusic()
        {
            if (clip.Length > 1)
            {
                //countPlay++;
                int indexRd = Random.Range(0, clip.Length);
                if (indexRd == countPlay)
                {
                    //nhu the nay se lam tang ti le lay music ben canh
                    indexRd++;
                }
                countPlay = indexRd;
                if (countPlay >= clip.Length)
                {
                    countPlay = 0;
                }
            }
            else
            {
                countPlay = 0;
            }
            return clip[countPlay];
        }
        public AudioClip GetClipNow()
        {
            return clip[countPlay];
        }
        //can thuc hien reset vi` nam trong scriptobject
        public void Reset()
        {
            //countPlay = 0;
            isJustPlay = false;
        }
    }
    [System.Serializable]
    public class DataAudioSources : IComparable<DataAudioSources>
    {
        public int id;
        public int indexAudioSource;
        public int CompareTo(DataAudioSources other)
        {
            return id.CompareTo(other.id);
        }
    }
    public class BaseAudioManager : MonoBehaviour
    {
        [SerializeField] protected bool mEnabledSFX = true;
        [SerializeField] protected bool mEnabledMusic = true;
        [SerializeField] protected AudioSource[] mSFXSources;
        [SerializeField] protected AudioSource mSFXSourceUnlimited;
        [SerializeField] protected AudioSource mMusicSource;

        public List<Clip> listSFXsoundJustPlay = new List<Clip>();

        private List<DataAudioSources> mSFXBattleSources = new List<DataAudioSources>();
        public bool EnabledSFX => mEnabledSFX;
        public bool EnabledMusic => mEnabledMusic;
        public float MusicVolume => mMusicSource.volume;

        private bool mStopPlaying;

        public int idMusicJustPlay;

        public virtual void Init()
        {
            int length = mSFXSources.Length;
            for (int i = 0; i < length; i++)
            {
                DataAudioSources item = new DataAudioSources()
                {
                    id = -1,
                    indexAudioSource = i
                };
                mSFXBattleSources.Add(item);
            }
            idMusicJustPlay = -1;
        }

        public void EnableMusic(bool pValue)
        {
            mEnabledMusic = pValue;
            mMusicSource.mute = !pValue;

            if (!mMusicSource.isPlaying)
                mMusicSource.Play();
        }

        public void EnableSFX(bool pValue)
        {
            mEnabledSFX = pValue;
            foreach (var s in mSFXSources)
                s.mute = !pValue;
        }

        public void SetMusicVolume(float pValue, float pFadeDuration = 0, Action pOnComplete = null)
        {
            if (mStopPlaying)
                return;

#if USE_DOTWEEN
            DOTween.Kill(mMusicSource.GetInstanceID());
            if (pFadeDuration <= 0)
            {
                mMusicSource.volume = pValue;
                pOnComplete?.Invoke();
            }
            else
                mMusicSource.DOFade(pValue, pFadeDuration)
                    .SetUpdate(true)
                    .SetId(mMusicSource.GetInstanceID())
                    .OnComplete(() => { pOnComplete?.Invoke(); });
#else
            float from = mMusicSource.volume;
            StartCoroutine(IELerp(pFadeDuration, (lerp) =>
            {
                mMusicSource.volume = from + (lerp * (pValue - from));
            }, () =>
            {
                pOnComplete?.Invoke();
            }));
#endif
        }

        public void SetSFXVolume(float pValue, float pFadeDuration = 0, Action pOnComplete = null)
        {
            if (mStopPlaying)
                return;

            for (int i = 0; i < mSFXSources.Length; i++)
            {
#if USE_DOTWEEN
                if (mSFXSources[i].isPlaying)
                {
                    DOTween.Kill(mSFXSources[i].GetInstanceID());
                    if (pFadeDuration <= 0)
                    {
                        mSFXSources[i].volume = pValue;
                        pOnComplete?.Invoke();
                    }
                    else
                        mSFXSources[i].DOFade(pValue, pFadeDuration)
                            .SetUpdate(true)
                            .SetId(mSFXSources[i].GetInstanceID())
                            .OnUpdate(() => { pOnComplete?.Invoke(); });
                }
#else
                mSFXSources[i].volume = pValue;
                pOnComplete?.Invoke();
#endif
            }
        }

        public void StopMusic(float pFadeDuration = 0, Action pOnComplete = null)
        {
            if (!mMusicSource.isPlaying)
                return;

            mStopPlaying = true;

#if USE_DOTWEEN
            DOTween.Kill(mMusicSource.GetInstanceID());
            if (pFadeDuration <= 0)
            {
                mMusicSource.Stop();
                pOnComplete?.Invoke();
            }
            else
            {
                mMusicSource.DOFade(0, pFadeDuration)
                    .OnComplete(() =>
                    {
                        mMusicSource.volume = 1;
                        mMusicSource.Stop();
                        pOnComplete?.Invoke();
                    }).SetId(mMusicSource.GetInstanceID()).SetUpdate(true);
            }
#else
            if (pFadeDuration <= 0)
                mMusicSource.Stop();
            else
            {
                StartCoroutine(IELerp(1f,
                    (lerp) =>
                    {
                        mMusicSource.volume = 1 - lerp;
                    }, () =>
                    {
                        mMusicSource.volume = 1;
                        mMusicSource.Stop();
                        pOnComplete?.Invoke();
                    }));
            }
#endif
        }

        public void StopSFX(Clip pClip)
        {
            if (pClip == null)
                return;
            for (int i = 0; i < mSFXBattleSources.Count; i++)
            {
                AudioSource audioSource = mSFXSources[mSFXBattleSources[i].indexAudioSource];
                if (audioSource.clip != null && audioSource.clip.GetInstanceID() == pClip.GetClipNow().GetInstanceID())
                {
                    mSFXSources[i].Stop();
                    mSFXSources[i].clip = null;
                    mSFXBattleSources[i].id = -1;
                }
            }
        }

        public void StopSFXs()
        {
            for (int i = 0; i < mSFXSources.Length; i++)
            {
                mSFXSources[i].Stop();
                mSFXSources[i].clip = null;
            }
        }

        protected void CreateAudioSources()
        {
            var sfxSources = new List<AudioSource>();
            var audioSources = gameObject.FindComponentsInChildren<AudioSource>();
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (i == 0)
                {
                    mMusicSource = audioSources[i];
                    mMusicSource.name = "Music";
                }
                else
                {
                    sfxSources.Add(audioSources[i]);
                    audioSources[i].name = "SFX_" + i;
                }
            }
            if (sfxSources.Count < 5)
                for (int i = sfxSources.Count; i <= 15; i++)
                {
                    var obj = new GameObject("SFX_" + i);
                    obj.transform.SetParent(transform);
                    sfxSources.Add(obj.AddComponent<AudioSource>());
                }
            mSFXSources = sfxSources.ToArray();
        }

        protected AudioSource CreateMoreSFXSource()
        {
            var obj = new GameObject("SFX_" + mSFXSources.Length);
            obj.transform.SetParent(transform);
            var newAudioSource = obj.AddComponent<AudioSource>();
            mSFXSources = mSFXSources.Add(newAudioSource);
            return newAudioSource;
        }

        protected IEnumerator IELerp(float pTime, Action<float> pOnUpdate, Action pOnFinished)
        {
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;
                if (pTime > time)
                    break;
                pOnUpdate.Raise(time / pTime);
                yield return null;
            }
            pOnFinished.Raise();
        }

        protected AudioSource GetSFXSouce(AudioClip pClip, int pLimitNumber, bool pLoop)
        {
            return GetSFXSouce(pClip.GetInstanceID(), pLimitNumber, pLoop);
        }
        protected AudioSource GetSFXSouce(int pClipID, int pLimitNumber, bool pLoop)
        {
            try
            {
                if (pLimitNumber > 0 || pLoop)
                {
                    DataAudioSources dataAudioSourceEmpty = mSFXBattleSources[0];
                    int indexFirst = -1;
                    if (!pLoop)
                    {
                        //if (pClipID == WildGunner.IDs.SFX_SHOOT_RIFLE)
                        //{
                        //    UnityEngine.Debug.Log($" firsttttttttttttttt countsame sfx : ");
                        //}
                        int countSameClips = 0;
                        for (int i = mSFXBattleSources.Count - 1; i >= 0; i--)
                        {
                            AudioSource audioSource = mSFXSources[mSFXBattleSources[i].indexAudioSource];
                            //if (pClipID == WildGunner.IDs.SFX_SHOOT_RIFLE)
                            //{
                            //    UnityEngine.Debug.Log($"{i} 222222 : {audioSource.isPlaying }  {mSFXBattleSources[i].id}");
                            //}
                            if (audioSource.isPlaying && audioSource.clip != null && mSFXBattleSources[i].id == pClipID)
                            {
                                if (indexFirst == -1)
                                {
                                    indexFirst = i;
                                }
                                countSameClips++;
                                if (countSameClips >= pLimitNumber)
                                {
                                    //if (pClipID == WildGunner.IDs.SFX_SHOOT_RIFLE) {
                                    //    UnityEngine.Debug.Log("countsame sfx : " + countSameClips + "/" + pLimitNumber);
                                    //}
                                    //var source = mSFXBattleSources[i];
                                    //mSFXBattleSources.Remove(source);
                                    //mSFXBattleSources.Add(source);
                                    return null;
                                    //return mSFXSources[indexFirst];
                                    //return audioSource;
                                    //return sources[i];
                                }
                            }
                            else if (!audioSource.isPlaying)
                            {
                                mSFXBattleSources[i].id = -1;
                                audioSource.clip = null;
                            }
                        }
                        if (countSameClips < pLimitNumber)
                        {
                            for (int i = mSFXBattleSources.Count - 1; i >= 0; i--)
                            {
                                AudioSource audioSource = mSFXSources[mSFXBattleSources[i].indexAudioSource];
                                if (audioSource.clip == null)
                                {
                                    dataAudioSourceEmpty = mSFXBattleSources[i];
                                    dataAudioSourceEmpty.id = pClipID;
                                    return audioSource;
                                }
                            }

                            //create new audiosource
                            AudioSource newAudio = CreateMoreSFXSource();
                            DataAudioSources item = new DataAudioSources()
                            {
                                id = pClipID,
                                indexAudioSource = mSFXSources.Length - 1
                            };
                            mSFXBattleSources.Add(item);
                            return newAudio;
                        }
                    }
                    else
                    {
                        for (int i = mSFXBattleSources.Count - 1; i >= 0; i--)
                        {
                            AudioSource audioSource = mSFXSources[mSFXBattleSources[i].indexAudioSource];
                            if (audioSource.clip == null || !audioSource.isPlaying
                                || mSFXBattleSources[i].id == pClipID)
                            {
                                dataAudioSourceEmpty = mSFXBattleSources[i];
                                dataAudioSourceEmpty.id = pClipID;
                                return audioSource;
                            }
                        }
                    }
                    //---------------------------
                    //if (!pLoop)
                    //{
                    //    int countSameClips = 0;
                    //    for (int i = mSFXSources.Length - 1; i >= 0; i--)
                    //    {
                    //        if (mSFXSources[i].isPlaying && mSFXSources[i].clip != null && mSFXSources[i].clip.GetInstanceID() == pClipID)
                    //            countSameClips++;
                    //        else if (!mSFXSources[i].isPlaying)
                    //            mSFXSources[i].clip = null;
                    //    }
                    //    if (countSameClips < pLimitNumber)
                    //    {
                    //        for (int i = mSFXSources.Length - 1; i >= 0; i--)
                    //            if (mSFXSources[i].clip == null)
                    //                return mSFXSources[i];

                    //        return CreateMoreSFXSource();
                    //    }
                    //}
                    //else
                    //{
                    //    for (int i = mSFXSources.Length - 1; i >= 0; i--)
                    //        if (mSFXSources[i].clip == null || !mSFXSources[i].isPlaying
                    //            || mSFXSources[i].clip.GetInstanceID() == pClipID)
                    //            return mSFXSources[i];
                    //}
                }
                else
                {
                    return mSFXSourceUnlimited;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                return null;
            }
        }

        public void PlayMusic(float pFadeDuration = 0, float pVolume = 1f)
        {
            mMusicSource.Play();
            mStopPlaying = false;
#if USE_DOTWEEN
            DOTween.Kill(mMusicSource.GetInstanceID());
            if (pFadeDuration <= 0)
                mMusicSource.volume = pVolume;
            else
            {
                mMusicSource.volume = 0;
                mMusicSource.DOFade(pVolume, pFadeDuration).SetUpdate(true)
                    .SetId(mMusicSource.GetInstanceID());
            }
#else
            if (pFadeDuration <= 0)
                mMusicSource.volume = pVolume;
            else
            {
                mMusicSource.volume = 0;
                StartCoroutine(IELerp(3f, (lerp) =>
                {
                    mMusicSource.volume = lerp * pVolume;
                }, () =>
                {
                    mMusicSource.volume = pVolume;
                }));
            }
#endif
        }
        public void BeforeViewVideo() {
            if (mEnabledMusic && mMusicSource.clip != null)
            {
                mMusicSource.volume = 0;
            }
        }
        public void AfterViewVideo() {
            if (mEnabledMusic && mMusicSource.clip != null)
            {
                mMusicSource.volume = 1;
            }
        }

        public void PauseMusic(){
            if (mEnabledMusic&& mMusicSource.clip!=null) {
                mMusicSource.Pause();
            }
        }
        public void ResumeMusic()
        {
            if (mEnabledMusic && mMusicSource.clip != null)
            {
                mMusicSource.Play();
            }
        }
        public void PlayMusic(AudioClip pClip, bool pLoop, float pFadeDuration = 0, float pVolume = 1f)
        {
            mStopPlaying = false;

            if (pClip == null || (mMusicSource.clip != null&& mMusicSource.isPlaying && pClip.GetInstanceID() == mMusicSource.clip.GetInstanceID()))
                return;

            mMusicSource.clip = pClip;
            mMusicSource.loop = pLoop;
            if (!mEnabledMusic) return;
            mMusicSource.Play();

#if USE_DOTWEEN
            DOTween.Kill(mMusicSource.GetInstanceID());
            if (pFadeDuration <= 0)
                mMusicSource.volume = pVolume;
            else
            {
                mMusicSource.volume = 0;
                mMusicSource.DOFade(pVolume, pFadeDuration).SetUpdate(true)
                    .SetId(mMusicSource.GetInstanceID());
            }
#else
            if (pFadeDuration <= 0)
                mMusicSource.volume = pVolume;
            else
            {
                mMusicSource.volume = 0;
                StartCoroutine(IELerp(3f, (lerp) =>
                {
                    mMusicSource.volume = lerp * pVolume;
                }, () =>
                {
                    mMusicSource.volume = pVolume;
                }));
            }
#endif
        }

        public void PlaySFX(Clip pClip, int limitNumber, bool pLoop, float pPitchRandomMultiplier = 1)
        {
            if (pClip == null || pClip.isJustPlay)
                return;
            AudioClip audioClipWillPlay = pClip.GetClipPlaySFX();
            var source = GetSFXSouce(pClip.id, limitNumber, pLoop);
            if (source == null)
                return;
            source.volume = 1;
            source.loop = pLoop;
            source.clip = audioClipWillPlay;
            source.pitch = 1;
            if (pPitchRandomMultiplier != 1)
            {
                if (Random.value < .5)
                    source.pitch *= Random.Range(1 / pPitchRandomMultiplier, 1);
                else
                    source.pitch *= Random.Range(1, pPitchRandomMultiplier);
            }
            if (!pLoop)
                source.PlayOneShot(audioClipWillPlay);
            else
                source.Play();

            pClip.isJustPlay = true;
            listSFXsoundJustPlay.Add(pClip);
        }
        private void LateUpdate()
        {
            int length = listSFXsoundJustPlay.Count;
            if (length > 0)
            {
                for (int i = length - 1; i >= 0; i--)
                {
                    listSFXsoundJustPlay[i].isJustPlay = false;
                    listSFXsoundJustPlay.RemoveAt(i);
                }
            }
            if (mEnabledMusic) {
                if (!mMusicSource.isPlaying) {
                    HybirdAudioManager.Instance.PlayMusicById(idMusicJustPlay, false, 3f);
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (mSFXSources == null)
                mSFXSources = new AudioSource[0];
            var audioSources = gameObject.FindComponentsInChildren<AudioSource>();
            for (int i = mSFXSources.Length - 1; i >= 0; i--)
            {
                if (audioSources.Contains(mSFXSources[i]))
                    audioSources.Remove(mSFXSources[i]);
            }
            if (mMusicSource == null && audioSources.Count > 0)
            {
                mMusicSource = audioSources[0];
                audioSources.RemoveAt(0);
                mMusicSource.name = "Music";
            }
            else if (mMusicSource == null)
            {
                var obj = new GameObject("Music");
                obj.AddComponent<AudioSource>();
                obj.transform.SetParent(transform);
                mMusicSource = obj.GetComponent<AudioSource>();
            }
            if (mSFXSourceUnlimited == null && audioSources.Count > 0)
            {
                mSFXSourceUnlimited = audioSources[0];
                audioSources.RemoveAt(0);
                mSFXSourceUnlimited.name = "SFXUnlimited";
            }
            else if (mSFXSourceUnlimited == null || mSFXSourceUnlimited == mMusicSource)
            {
                var obj = new GameObject("SFXUnlimited");
                obj.AddComponent<AudioSource>();
                obj.transform.SetParent(transform);
                mSFXSourceUnlimited = obj.GetComponent<AudioSource>();
            }
        }

        [CustomEditor(typeof(BaseAudioManager), true)]
        protected class BaseAudioManagerEditor : Editor
        {
            private BaseAudioManager mScript;

            private void OnEnable()
            {
                mScript = target as BaseAudioManager;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (EditorHelper.ButtonColor("Add Music Audio Source", mScript.mMusicSource == null ? Color.green : Color.grey))
                {
                    if (mScript.mMusicSource == null)
                    {
                        var obj = new GameObject("Music");
                        obj.transform.SetParent(mScript.transform);
                        obj.AddComponent<AudioSource>();
                        mScript.mMusicSource = obj.GetComponent<AudioSource>();
                    }
                    if (mScript.mSFXSourceUnlimited == null)
                    {
                        var obj = new GameObject("SFX_Unlimited");
                        obj.transform.SetParent(mScript.transform);
                        obj.AddComponent<AudioSource>();
                        mScript.mMusicSource = obj.GetComponent<AudioSource>();
                    }
                }
                if (EditorHelper.ButtonColor("Add SFX Audio Source"))
                    mScript.CreateMoreSFXSource();
                if (EditorHelper.ButtonColor("Create Audio Sources", Color.green))
                    mScript.CreateAudioSources();
                if (EditorHelper.Button("Stop Music"))
                    mScript.StopMusic(1f);
                if (EditorHelper.Button("Play Music"))
                    mScript.PlayMusic();

                if (GUI.changed)
                    EditorUtility.SetDirty(mScript);
            }
        }
#endif
    }
}