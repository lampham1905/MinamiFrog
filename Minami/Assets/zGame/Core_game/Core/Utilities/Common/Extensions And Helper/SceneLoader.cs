﻿using System;
using System.Collections;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper
{
    public class SceneLoader
    {
        public static AsyncOperation LoadScene(string pScene, bool pIsAdditive, bool pAutoActive, Action<float> pOnProgress, Action pOnCompleted, float pFixedLoadTime = 0)
        {
            var scene = SceneManager.GetSceneByName(pScene);
            if (scene.isLoaded&& pIsAdditive)
            {
                pOnProgress.Raise(1);
                pOnCompleted.Raise();
                return null;
            }

            var sceneOperator = SceneManager.LoadSceneAsync(pScene, pIsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            sceneOperator.allowSceneActivation = false;
            CoroutineUtil.StartCoroutine(IEProcessOperation(sceneOperator, pAutoActive, pOnProgress, pOnCompleted, pFixedLoadTime));
            return sceneOperator;
        }

        public static void LoadScene(string pScene, bool pIsAdditive)
        {
            var scene = SceneManager.GetSceneByName(pScene);
            if (scene.isLoaded)
                return;

            SceneManager.LoadScene(pScene, pIsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }

        private static IEnumerator IEProcessOperation(AsyncOperation sceneOperator, bool pAutoActive, Action<float> pOnProgress, Action pOnCompleted, float pFixedLoadTime = 0)
        {
            pOnProgress.Raise(0f);

            float startTime = Time.unscaledTime;
            float fakeProgress = 0.2f;
            float offsetProgress = pFixedLoadTime > 0 ? fakeProgress : 0;

            while (true)
            {
                float progress = Mathf.Clamp01(sceneOperator.progress / 0.9f);
                pOnProgress.Raise(progress - offsetProgress);
                yield return null;

                if (sceneOperator.isDone || progress >= 1)
                    break;
            }

            float loadTime = Time.unscaledTime - startTime;
            float additionalTime = pFixedLoadTime - loadTime;
            if (additionalTime <= 0)
                pOnProgress.Raise(1);
            else
            {
                float time = 0;
                while (true)
                {
                    time += Time.deltaTime;
                    if (time > additionalTime)
                        break;

                    float progress = (1 - fakeProgress) + time / additionalTime * fakeProgress;
                    pOnProgress.Raise(progress);
                    yield return null;
                }
                pOnProgress.Raise(1);
            }

            pOnCompleted.Raise();

            if (pAutoActive)
                sceneOperator.allowSceneActivation = true;
        }

        public static AsyncOperation UnloadScene(string pScene, Action<float> pOnProgress, Action pOnComplted)
        {
            var scene = SceneManager.GetSceneByName(pScene);
            if (!scene.isLoaded)
            {
                pOnProgress(1f);
                return null;
            }

            var sceneOperator = SceneManager.UnloadSceneAsync(pScene);
            CoroutineUtil.StartCoroutine(IEProcessOperation(sceneOperator, false, pOnProgress, pOnComplted));
            return sceneOperator;
        }

        public static AsyncOperation UnloadScene(Scene pScene, Action<float> pOnProgress, Action pOnComplted)
        {
            if (!pScene.isLoaded)
            {
                pOnProgress(1f);
                return null;
            }

            var sceneOperator = SceneManager.UnloadSceneAsync(pScene);
            CoroutineUtil.StartCoroutine(IEProcessOperation(sceneOperator, false, pOnProgress, pOnComplted));
            return sceneOperator;
        }
    }
}