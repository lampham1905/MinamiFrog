﻿#define ADDRESSABLES

#if UNITY_EDITOR
#endif
#if ADDRESSABLES
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lam.zGame.Core_game.Core.Utilities.Common.Other;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using static UnityEngine.AddressableAssets.Addressables;
using Object = UnityEngine.Object;

namespace Lam.zGame.Core_game.Core.Utilities.Common.Extensions_And_Helper
{
#if ADDRESSABLES

    public class ComponentRef<T> : AssetReference where T : Component
    {
        public ComponentRef(string guid) : base(guid) { }
        public void LoadAsset()
        {
            LoadAssetAsync<T>();
        }
        public override bool ValidateAsset(Object obj)
        {
            var go = obj as GameObject;
            if (go == null)
                return false;
            else
            {
                T component;
                go.TryGetComponent(out component);
                return component != null;
            }
        }
        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            //this load can be expensive!
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go == null)
                return false;
            else
            {
                T component;
                go.TryGetComponent(out component);
                return component != null;
            }
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// Example generic asset reference
    /// </summary>
    [Serializable]
    public class ComponentRef_SpriteRenderer : ComponentRef<SpriteRenderer>
    {
        public ComponentRef_SpriteRenderer(string guid) : base(guid) { }
    }

    /// <summary>
    /// Example generic asset reference
    /// </summary>
    [Serializable]
    public class AssetRef_SpriteAtlas : AssetReferenceT<SpriteAtlas>
    {
        public AssetRef_SpriteAtlas(string guid) : base(guid) { }
    }

    //================================================================================

    public static class AddressableUtil
    {
        //================ Basic

        public static AsyncOperationHandle<long> GetDownloadSizeAsync(object key, Action<long> pOnComplete)
        {
            var operation = Addressables.GetDownloadSizeAsync(key);
            WaitLoadTask(operation, null, () =>
            {
                pOnComplete?.Invoke(operation.Result);
            });
            return operation;
        }

        public static AsyncOperationHandle DownloadDependenciesAsync(object pKey, bool pAutoRelease, Action<float> pOnDownload = null, Action pOnComplete = null)
        {
            var operation = Addressables.DownloadDependenciesAsync(pKey, pAutoRelease);
            WaitLoadTask(operation, pOnDownload, pOnComplete);
            return operation;
        }

        public static AsyncOperationHandle DownloadDependenciesAsync(IList<IResourceLocation> locations, bool pAutoRelease, Action<float> pOnDownload = null, Action pOnComplete = null)
        {
            var operation = Addressables.DownloadDependenciesAsync(locations, pAutoRelease);
            WaitLoadTask(operation, pOnDownload, pOnComplete);
            return operation;
        }

        public static AsyncOperationHandle DownloadDependenciesAsync(IList<object> pKeys, MergeMode mode, bool pAutoRelease, Action<float> pOnDownload = null, Action pOnComplete = null)
        {
            var operation = Addressables.DownloadDependenciesAsync(pKeys, mode, pAutoRelease);
            WaitLoadTask(operation, pOnDownload, pOnComplete);
            return operation;
        }

        public static AsyncOperationHandle DownloadDependenciesAsync(string pAddress, bool pAutoRelease, Action<float> pOnDownload = null, Action pOnComplete = null)
        {
            var operation = Addressables.DownloadDependenciesAsync(pAddress, pAutoRelease);
            WaitLoadTask(operation, pOnDownload, pOnComplete);
            return operation;
        }

        public static AsyncOperationHandle DownloadDependenciesAsync(AssetReference pReference, bool pAutoRelease, Action<float> pOnDownload = null, Action pOnComplete = null)
        {
            var operation = Addressables.DownloadDependenciesAsync(pReference, pAutoRelease);
            WaitLoadTask(operation, pOnDownload, pOnComplete);
            return operation;
        }

        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(string pAddress, LoadSceneMode pMode, Action<float> pProgress = null, Action<SceneInstance> pOnCompleted = null)
        {
            var operation = Addressables.LoadSceneAsync(pAddress, pMode);
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance pScene, Action<float> pProgress = null, Action<bool> pOnCompleted = null)
        {
            var operation = Addressables.UnloadSceneAsync(pScene);
            WaitUnloadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<TextAsset> LoadTextAssetAsync(string pAddress, Action<float> pProgress = null, Action<TextAsset> pOnCompleted = null)
        {
            var operation = Addressables.LoadAssetAsync<TextAsset>(pAddress);
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(string pAddress, Action<float> pProgress = null, Action<TObject> pOnCompleted = null) where TObject : Object
        {
            var operation = Addressables.LoadAssetAsync<TObject>(pAddress);
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<GameObject> InstantiateAsync(string pAddress, Action<float> pProgress = null, Action<GameObject> pOnCompleted = null)
        {
            var operation = Addressables.InstantiateAsync(pAddress);
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<GameObject> InstantiateAsync<TReference>(TReference pReference, Action<float> pProgress = null, Action<GameObject> pOnCompleted = null) where TReference : AssetReference
        {
            var operation = pReference.InstantiateAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<GameObject> InstantiateAsync<TComponent, TReference>(TReference pReference, Action<float> pProgress = null, Action<TComponent> pOnCompleted = null) where TComponent : Component where TReference : AssetReference
        {
            var operation = pReference.InstantiateAsync();
            WaitLoadTask(operation, pProgress, (result) =>
            {
                result.TryGetComponent(out TComponent component);
                pOnCompleted?.Invoke(component);
            });
            return operation;
        }

        //================ Asset Reference

        public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject, TReference>(TReference pAsset, Action<float> pProgress = null, Action<TObject> pOnCompleted = null) where TObject : Object where TReference : AssetReference
        {
            var operation = pAsset.LoadAssetAsync<TObject>();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(AssetReferenceT<TObject> pReference, Action<float> pProgress = null, Action<TObject> pOnCompleted = null) where TObject : Object
        {
            var operation = pReference.LoadAssetAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static void LoadPrefabAsync<TComponent>(AssetReference pReference, Action<float> pProgress = null, Action<TComponent> pOnCompleted = null) where TComponent : Component
        {
            var operation = pReference.LoadAssetAsync<GameObject>();
            WaitLoadTask(operation, pProgress, (result) =>
            {
                result.TryGetComponent(out TComponent component);
                pOnCompleted?.Invoke(component);
            });
        }

        public static AsyncOperationHandle<Sprite> LoadSpriteAsync(AssetReferenceSprite pReference, Action<float> pProgress = null, Action<Sprite> pOnCompleted = null)
        {
            var operation = pReference.LoadAssetAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<IList<Sprite>> LoadSpritesAsync(AssetReference pReference, Action<float> pProgress = null, Action<IList<Sprite>> pOnCompleted = null)
        {
            var operation = pReference.LoadAssetAsync<IList<Sprite>>();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<GameObject> LoadGameObjectAsync(AssetReferenceGameObject pAsset, Action<float> pProgress = null, Action<GameObject> pOnCompleted = null)
        {
            var operation = pAsset.LoadAssetAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<Texture> LoadTextureAsync(AssetReferenceTexture pReference, Action<float> pProgress = null, Action<Texture> pOnCompleted = null)
        {
            var operation = pReference.LoadAssetAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<Texture2D> LoadTexture2DAsync(AssetReferenceTexture2D pReference, Action<float> pProgress = null, Action<Texture2D> pOnCompleted = null)
        {
            var operation = pReference.LoadAssetAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        public static AsyncOperationHandle<Texture3D> LoadTexture3DAsync(AssetReferenceTexture3D pReference, Action<float> pProgress = null, Action<Texture3D> pOnCompleted = null)
        {
            var operation = pReference.LoadAssetAsync();
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        //================ Variation

        public static AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(string pAddress, string pLabel, Action<float> pProgress = null, Action<IList<TObject>> pOnCompleted = null)
        {
            var operation = Addressables.LoadAssetsAsync<TObject>(new List<object> { pAddress, pLabel }, null, Addressables.MergeMode.Intersection);
            WaitLoadTask(operation, pProgress, pOnCompleted);
            return operation;
        }

        //=========================================================================================================================================

        private static void WaitLoadTask(AsyncOperationHandle operation, Action<float> pProgress = null, Action pOnCompleted = null)
        {
            WaitUtil.Start(new WaitUtil.ConditionEvent()
            {
                triggerCondition = () => operation.IsDone,
                onUpdate = () =>
                {
                    if (pProgress != null)
                        pProgress(operation.PercentComplete);
                },
                onTrigger = () =>
                {
                    if (pOnCompleted != null)
                        pOnCompleted();

                    if (operation.Status == AsyncOperationStatus.Failed)
                        Debug.Debug.LogError("Failed to load asset: " + operation.OperationException.ToString());
                },
            });
        }

        private static void WaitLoadTask<T>(AsyncOperationHandle<T> operation, Action<float> pProgress = null, Action<T> pOnCompleted = null)
        {
            WaitUtil.Start(new WaitUtil.ConditionEvent()
            {
                triggerCondition = () => operation.IsDone,
                onUpdate = () =>
                {
                    if (pProgress != null)
                        pProgress(operation.PercentComplete);
                },
                onTrigger = () =>
                {
                    if (pOnCompleted != null)
                        pOnCompleted(operation.Result);

                    if (operation.Status == AsyncOperationStatus.Failed)
                        Debug.Debug.LogError("Failed to load asset: " + operation.OperationException.ToString());
                },
            });
        }

        private static void WaitUnloadTask<T>(AsyncOperationHandle<T> operation, Action<float> pProgress = null, Action<bool> pOnCompleted = null)
        {
            WaitUtil.Start(new WaitUtil.ConditionEvent()
            {
                triggerCondition = () => operation.IsDone,
                onUpdate = () =>
                {
                    if (pProgress != null)
                        pProgress(operation.PercentComplete);
                },
                onTrigger = () =>
                {
                    if (pOnCompleted != null)
                        pOnCompleted(true);

                    if (operation.Status == AsyncOperationStatus.Failed)
                        Debug.Debug.LogError("Failed to unload asset: " + operation.OperationException.ToString());
                },
            });
        }

        //================ Wait Async

        public static async Task<long> GetDownloadSizeAsync(object key)
        {
            //Clear all cached AssetBundles
            Addressables.ClearDependencyCacheAsync(key);

            //Check the download size
            var operation = Addressables.GetDownloadSizeAsync(key);
            await operation.Task;
            return operation.Result;
        }

        public static async Task<AsyncOperationHandle<TObject>> WaitLoadAssetAsync<TObject>(string pAddress) where TObject : Object
        {
            var operation = Addressables.LoadAssetAsync<TObject>(pAddress);
            await operation.Task;
            return operation;
        }

        public static async Task<List<AsyncOperationHandle<TObject>>> WaitLoadAssetAsync<TObject>(List<string> pAddresses) where TObject : Object
        {
            var list = new List<AsyncOperationHandle<TObject>>();
            foreach (var address in pAddresses)
            {
                var operation = Addressables.LoadAssetAsync<TObject>(address);
                await operation.Task;
                list.Add(operation);
            }
            return list;
        }

        public static async Task<AsyncOperationHandle<GameObject>> WaitInstantiateAsync(string pAddress)
        {
            var operation = Addressables.InstantiateAsync(pAddress);
            await operation.Task;
            return operation;
        }

        public static async Task<AsyncOperationHandle<GameObject>> WaitInstantiateAsync(AssetReference pReference)
        {
            var operation = pReference.InstantiateAsync();
            await operation.Task;
            return operation;
        }
        public static async Task<AsyncOperationHandle<GameObject>> WaitInstantiateAsync<M>(M pReference) where M : AssetReference
        {
            var operation = pReference.InstantiateAsync();
            await operation.Task;
            return operation;
        }

        public static async Task<List<AsyncOperationHandle<GameObject>>> WaitInstantiateAsync(List<AssetReference> pReferences)
        {
            var list = new List<AsyncOperationHandle<GameObject>>();
            foreach (var preference in pReferences)
            {
                var operation = preference.InstantiateAsync();
                await operation.Task;
                list.Add(operation);
            }
            return list;
        }
        public static async Task<List<AsyncOperationHandle<GameObject>>> WaitInstantiateAsync<M>(List<M> pReferences) where M : AssetReference
        {
            var list = new List<AsyncOperationHandle<GameObject>>();
            foreach (var preference in pReferences)
            {
                var operation = preference.InstantiateAsync();
                await operation.Task;
                list.Add(operation);
            }
            return list;
        }

        public static async Task<List<WrapPrefab<TComponent>>> WaitInstantiateAsync<TComponent>(List<AssetReference> pReferences) where TComponent : Component
        {
            var listWraps = new List<WrapPrefab<TComponent>>();
            foreach (var preference in pReferences)
            {
                var operation = preference.InstantiateAsync();
                await operation.Task;
                operation.Result.TryGetComponent(out TComponent component);
                listWraps.Add(new WrapPrefab<TComponent>()
                {
                    operation = operation,
                    prefab = component
                });
            }
            return listWraps;
        }
        public static async Task<List<WrapPrefab<TComponent>>> WaitInstantiateAsync<TComponent, M>(List<M> pReferences) where TComponent : Component where M : AssetReference
        {
            var listWraps = new List<WrapPrefab<TComponent>>();
            foreach (var preference in pReferences)
            {
                var operation = preference.InstantiateAsync();
                await operation.Task;
                operation.Result.TryGetComponent(out TComponent component);
                listWraps.Add(new WrapPrefab<TComponent>()
                {
                    operation = operation,
                    prefab = component
                });
            }
            return listWraps;
        }

        public static async Task<WrapPrefab<TComponent>> WaitInstantiateAsync<TComponent>(AssetReference pReference) where TComponent : Component
        {
            var wrap = new WrapPrefab<TComponent>();
            var operation = pReference.InstantiateAsync();
            await operation.Task;
            operation.Result.TryGetComponent(out TComponent component);
            wrap.operation = operation;
            wrap.prefab = component;
            return wrap;
        }
        public static async Task<WrapPrefab<TComponent>> WaitInstantiateAsync<TComponent, M>(M pReference) where TComponent : Component where M : AssetReference
        {
            var wrap = new WrapPrefab<TComponent>();
            var operation = pReference.InstantiateAsync();
            await operation.Task;
            operation.Result.TryGetComponent(out TComponent component);
            wrap.operation = operation;
            wrap.prefab = component;
            return wrap;
        }

        public static async Task<AsyncOperationHandle<TObject>> WaitLoadAssetAsync<TObject>(AssetReference pReference) where TObject : Object
        {
            var operation = pReference.LoadAssetAsync<TObject>();
            await operation.Task;
            return operation;
        }
        public static async Task<AsyncOperationHandle<TObject>> WaitLoadAssetAsync<TObject, M>(M pReference) where TObject : Object where M : AssetReference
        {
            var operation = pReference.LoadAssetAsync<TObject>();
            await operation.Task;
            return operation;
        }

        public static async Task<List<AsyncOperationHandle<TObject>>> WaitLoadAssetsAsync<TObject>(List<AssetReference> pReferences) where TObject : Object
        {
            var list = new List<AsyncOperationHandle<TObject>>();
            foreach (var r in pReferences)
            {
                var operation = r.LoadAssetAsync<TObject>();
                await operation.Task;
                list.Add(operation);
            }
            return list;
        }
        public static async Task<List<AsyncOperationHandle<TObject>>> WaitLoadAssetsAsync<TObject, M>(List<M> pReferences) where TObject : Object where M : AssetReference
        {
            var list = new List<AsyncOperationHandle<TObject>>();
            foreach (var r in pReferences)
            {
                var operation = r.LoadAssetAsync<TObject>();
                await operation.Task;
                list.Add(operation);
            }
            return list;
        }

        public static async Task<WrapPrefab<TComponent>> WaitLoadPrefabAsync<TComponent>(AssetReference pReference) where TComponent : Component
        {
            var wrap = new WrapPrefab<TComponent>();
            var operation = pReference.LoadAssetAsync<GameObject>();
            await operation.Task;
            operation.Result.TryGetComponent(out TComponent component);
            wrap.operation = operation;
            wrap.prefab = component;
            return wrap;
        }
        public static async Task<WrapPrefab<TComponent>> WaitLoadPrefabAsync<TComponent, TReference>(TReference pReference) where TComponent : Component where TReference : AssetReference
        {
            var wrap = new WrapPrefab<TComponent>();
            var operation = pReference.LoadAssetAsync<GameObject>();
            await operation.Task;
            operation.Result.TryGetComponent(out TComponent component);
            wrap.operation = operation;
            wrap.prefab = component;
            return wrap;
        }

        public static async Task<List<WrapPrefab<TComponent>>> WaitLoadPrefabsAsync<TComponent>(List<AssetReference> pReferences) where TComponent : Component
        {
            var list = new List<WrapPrefab<TComponent>>();
            foreach (var r in pReferences)
            {
                var operation = r.LoadAssetAsync<GameObject>();
                await operation.Task;
                operation.Result.TryGetComponent(out TComponent component);
                list.Add(new WrapPrefab<TComponent>()
                {
                    operation = operation,
                    prefab = component
                });
            }
            return list;
        }
        public static async Task<List<WrapPrefab<TComponent>>> WaitLoadPrefabsAsync<TComponent, TReference>(List<TReference> pReferences) where TComponent : Component where TReference : AssetReference
        {
            var list = new List<WrapPrefab<TComponent>>();
            foreach (var r in pReferences)
            {
                var operation = r.LoadAssetAsync<GameObject>();
                await operation.Task;
                operation.Result.TryGetComponent(out TComponent component);
                list.Add(new WrapPrefab<TComponent>()
                {
                    operation = operation,
                    prefab = component
                });
            }
            return list;
        }

        public static async Task<List<IResourceLocation>> WaitLoadResouceLocationAsync(string pLabel)
        {
            var localtions = new List<IResourceLocation>();
            var operation = Addressables.LoadResourceLocationsAsync(pLabel);
            await operation.Task;
            foreach (var localtion in operation.Result)
                localtions.Add(localtion);
            return localtions;
        }

        public static async Task<List<AsyncOperationHandle<GameObject>>> WaitInstantiateAsync(IList<IResourceLocation> pLocations)
        {
            var list = new List<AsyncOperationHandle<GameObject>>();
            foreach (var location in pLocations)
            {
                var operation = Addressables.InstantiateAsync(location);
                await operation.Task;
                list.Add(operation);
            }
            return list;
        }

        public static async Task<List<WrapPrefab<TComponent>>> WaitInstantiateAsync<TComponent>(IList<IResourceLocation> pLocations) where TComponent : Component
        {
            var list = new List<WrapPrefab<TComponent>>();
            foreach (var location in pLocations)
            {
                var operation = Addressables.InstantiateAsync(location);
                await operation.Task;
                operation.Result.TryGetComponent(out TComponent component);
                list.Add(new WrapPrefab<TComponent>()
                {
                    operation = operation,
                    prefab = component
                });
            }
            return list;
        }

        public static List<TComponent> GetPrefabs<TComponent>(this List<WrapPrefab<TComponent>> wraps) where TComponent : Component
        {
            var list = new List<TComponent>();
            foreach (var wrap in wraps)
                list.Add(wrap.prefab);
            return list;
        }

        public static List<TObject> GetObjects<TObject>(this List<AsyncOperationHandle<TObject>> operations) where TObject : Object
        {
            var list = new List<TObject>();
            foreach (var operation in operations)
                list.Add(operation.Result);
            return list;
        }
    }

    public class WrapPrefab<TComponent> where TComponent : Component
    {
        public TComponent prefab;
        public AsyncOperationHandle operation;
        public void Release()
        {
            Addressables.Release(operation);
        }
    }
#endif
}