using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Lam.zGame.Core_game.Core.Utilities.Components.AddressablesManager
{
    /// <summary>
    /// Handles all loading, unloading, instantiating, and destroying of AssetReferences and their associated Objects.
    /// </summary>
    public static class AssetManager
    {
        private const string ERROR = "<color=#ffa500>" + nameof(AssetManager) + " Error:</color> ";

        public delegate void DelegateAssetLoaded(object key, AsyncOperationHandle handle);
        public static event DelegateAssetLoaded OnAssetLoaded;

        public delegate void DelegateAssetUnloaded(object runtimeKey);
        public static event DelegateAssetUnloaded OnAssetUnloaded;

        private static readonly Dictionary<object, AsyncOperationHandle> m_LoadingAssets = new Dictionary<object, AsyncOperationHandle>(20);
        private static readonly Dictionary<object, AsyncOperationHandle> m_LoadedAssets = new Dictionary<object, AsyncOperationHandle>(100);
        private static readonly Dictionary<object, List<GameObject>> m_InstantiatedObjects = new Dictionary<object, List<GameObject>>(10);

        public static IReadOnlyList<object> LoadedAssets => m_LoadedAssets.Values.Select(x => x.Result).ToList();
        public static int loadedAssetsCount => m_LoadedAssets.Count;
        public static int loadingAssetsCount => m_LoadingAssets.Count;
        public static int instantiatedAssetsCount => m_InstantiatedObjects.Values.SelectMany(x => x).Count();

        #region Get
        public static bool IsLoaded(AssetReference pRef)
        {
            return m_LoadedAssets.ContainsKey(pRef.RuntimeKey);
        }
        static bool IsLoaded(object key)
        {
            return m_LoadedAssets.ContainsKey(key);
        }
        public static bool IsLoading(AssetReference pRef)
        {
            return m_LoadingAssets.ContainsKey(pRef.RuntimeKey);
        }
        static bool IsLoading(object key)
        {
            return m_LoadingAssets.ContainsKey(key);
        }
        public static bool IsInstantiated(AssetReference pRef)
        {
            return m_InstantiatedObjects.ContainsKey(pRef.RuntimeKey);
        }
        public static bool IsInstantiated(object key)
        {
            return m_InstantiatedObjects.ContainsKey(key);
        }
        public static int InstantiatedCount(AssetReference pRef)
        {
            return !IsInstantiated(pRef) ? 0 : m_InstantiatedObjects[pRef.RuntimeKey].Count;
        }
        #endregion

        #region Load/Unload
        /// <summary>
        /// DO NOT USE FOR <see cref="Component"/>s. Call <see cref="TryGetOrLoadComponentAsync{TComponentType}(UnityEngine.AddressableAssets.AssetReference,out UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle{TComponentType})"/>
        ///
        /// Tries to get an already loaded <see cref="UnityEngine.Object"/> of type <see cref="TObjectType"/>.
        /// Returns <value>true</value> if the object was loaded and sets <paramref name="handle"/> to the completed <see cref="AsyncOperationHandle{TObject}"/>
        /// If the object was not loaded returns <value>false</value>, loads the object and sets <paramref name="handle"/> to the un-completed <see cref="AsyncOperationHandle{TObject}"/>
        /// </summary>
        /// <param name="pRef">The <see cref="AssetReference"/> to load.</param>
        /// <param name="handle">The loading or completed <see cref="AsyncOperationHandle{TObject}"/></param>
        /// <typeparam name="TObjectType">The type of NON-COMPONENT object to load.</typeparam>
        /// <returns><value>true</value> if the object has already been loaded, false otherwise.</returns>
        public static bool TryGetOrLoadObjectAsync<TObjectType>(AssetReference pRef, out AsyncOperationHandle<TObjectType> handle) where TObjectType : Object
        {
            CheckRuntimeKey(pRef);

            var key = pRef.RuntimeKey;

            if (m_LoadedAssets.ContainsKey(key))
            {
                try
                {
                    handle = m_LoadedAssets[key].Convert<TObjectType>();
                }
                catch
                {
                    handle = Addressables.ResourceManager.CreateCompletedOperation(m_LoadedAssets[key].Result as TObjectType, string.Empty);
                }

                return true;
            }

            if (m_LoadingAssets.ContainsKey(key))
            {
                try
                {
                    handle = m_LoadingAssets[key].Convert<TObjectType>();
                }
                catch
                {
                    handle = Addressables.ResourceManager.CreateChainOperation(m_LoadingAssets[key], chainOp => Addressables.ResourceManager.CreateCompletedOperation(chainOp.Result as TObjectType, string.Empty));
                }
                return false;
            }


            handle = Addressables.LoadAssetAsync<TObjectType>(pRef);

            m_LoadingAssets.Add(key, handle);

            handle.Completed += op2 =>
            {
                m_LoadedAssets.Add(key, op2);
                m_LoadingAssets.Remove(key);

                OnAssetLoaded?.Invoke(key, op2);
            };

            return false;
        }
        /// <summary>
        /// DO NOT USE FOR <see cref="Component"/>s. Call <see cref="TryGetOrLoadComponentAsync{TComponentType}(UnityEngine.AddressableAssets.AssetReference,out UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle{TComponentType})"/>
        ///
        /// Tries to get an already loaded <see cref="UnityEngine.Object"/> of type <see cref="TObjectType"/>.
        /// Returns <value>true</value> if the object was loaded and sets <paramref name="handle"/> to the completed <see cref="AsyncOperationHandle{TObject}"/>
        /// If the object was not loaded returns <value>false</value>, loads the object and sets <paramref name="handle"/> to the un-completed <see cref="AsyncOperationHandle{TObject}"/>
        /// </summary>
        /// <param name="pRef">The <see cref="AssetReferenceT{TObject}"/> to load.</param>
        /// <param name="handle">The loading or completed <see cref="AsyncOperationHandle{TObject}"/></param>
        /// <typeparam name="TObjectType">The type of NON-COMPONENT object to load.</typeparam>
        /// <returns><value>true</value> if the object has already been loaded, false otherwise.</returns>
        public static bool TryGetOrLoadObjectAsync<TObjectType>(AssetReferenceT<TObjectType> pRef, out AsyncOperationHandle<TObjectType> handle) where TObjectType : Object
        {
            return TryGetOrLoadObjectAsync(pRef as AssetReference, out handle);
        }

        /// <summary>
        /// DO NOT USE FOR <see cref="UnityEngine.Object"/>s. Call <see cref="TryGetOrLoadObjectAsync{TObjectType}(UnityEngine.AddressableAssets.AssetReference,out UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle{TObjectType})"/>
        ///
        /// Tries to get an already loaded <see cref="Component"/> of type <see cref="TComponentType"/>.
        /// Returns <value>true</value> if the object was loaded and sets <paramref name="handle"/> to the completed <see cref="AsyncOperationHandle{TObject}"/>
        /// If the object was not loaded returns <value>false</value>, loads the object and sets <paramref name="handle"/> to the un-completed <see cref="AsyncOperationHandle{TObject}"/>
        /// </summary>
        /// <param name="pRef">The <see cref="AssetReference"/> to load.</param>
        /// <param name="handle">The loading or completed <see cref="AsyncOperationHandle{TObject}"/></param>
        /// <typeparam name="TComponentType">The type of Component to load.</typeparam>
        /// <returns><value>true</value> if the object has already been loaded, false otherwise.</returns>
        public static bool TryGetOrLoadComponentAsync<TComponentType>(AssetReference pRef, out AsyncOperationHandle<TComponentType> handle) where TComponentType : Component
        {
            CheckRuntimeKey(pRef);

            var key = pRef.RuntimeKey;

            if (m_LoadedAssets.ContainsKey(key))
            {
                handle = ConvertHandleToComponent<TComponentType>(m_LoadedAssets[key]);
                return true;
            }


            if (m_LoadingAssets.ContainsKey(key))
            {
                handle = Addressables.ResourceManager.CreateChainOperation(m_LoadingAssets[key], ConvertHandleToComponent<TComponentType>);
                return false;
            }


            var op = Addressables.LoadAssetAsync<GameObject>(pRef);

            m_LoadingAssets.Add(key, op);

            op.Completed += op2 =>
            {
                m_LoadedAssets.Add(key, op2);
                m_LoadingAssets.Remove(key);

                OnAssetLoaded?.Invoke(key, op2);
            };

            handle = Addressables.ResourceManager.CreateChainOperation<TComponentType, GameObject>(op, chainOp =>
            {
                var go = chainOp.Result;
                go.TryGetComponent(out TComponentType comp);
                return Addressables.ResourceManager.CreateCompletedOperation(comp, string.Empty);
            });
            return false;
        }
        /// <summary>
        /// DO NOT USE FOR <see cref="UnityEngine.Object"/>s. Call <see cref="TryGetOrLoadObjectAsync{TObjectType}(UnityEngine.AddressableAssets.AssetReference,out UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle{TObjectType})"/>
        ///
        /// Tries to get an already loaded <see cref="Component"/> of type <see cref="TComponentType"/>.
        /// Returns <value>true</value> if the object was loaded and sets <paramref name="handle"/> to the completed <see cref="AsyncOperationHandle{TObject}"/>
        /// If the object was not loaded returns <value>false</value>, loads the object and sets <paramref name="handle"/> to the un-completed <see cref="AsyncOperationHandle{TObject}"/>
        /// </summary>
        /// <param name="pRef">The <see cref="AssetReferenceT{TObject}"/> to load.</param>
        /// <param name="handle">The loading or completed <see cref="AsyncOperationHandle{TObject}"/></param>
        /// <typeparam name="TComponentType">The type of Component to load.</typeparam>
        /// <returns><value>true</value> if the object has already been loaded, false otherwise.</returns>
        public static bool TryGetOrLoadComponentAsync<TComponentType>(AssetReferenceT<TComponentType> pRef, out AsyncOperationHandle<TComponentType> handle) where TComponentType : Component
        {
            return TryGetOrLoadComponentAsync(pRef as AssetReference, out handle);
        }

        public static bool TryGetObjectSync<TObjectType>(AssetReference pRef, out TObjectType result) where TObjectType : Object
        {
            CheckRuntimeKey(pRef);
            var key = pRef.RuntimeKey;

            if (m_LoadedAssets.ContainsKey(key))
            {
                result = m_LoadedAssets[key].Convert<TObjectType>().Result;
                return true;
            }

            result = null;
            return false;
        }
        public static bool TryGetObjectSync<TObjectType>(AssetReferenceT<TObjectType> pRef, out TObjectType result) where TObjectType : Object
        {
            return TryGetObjectSync(pRef as AssetReference, out result);
        }

        public static bool TryGetComponentSync<TComponentType>(AssetReference pRef, out TComponentType result) where TComponentType : Component
        {
            CheckRuntimeKey(pRef);
            var key = pRef.RuntimeKey;

            if (m_LoadedAssets.ContainsKey(key))
            {
                var handle = m_LoadedAssets[key];
                result = null;
                var go = handle.Result as GameObject;
                if (!go)
                    throw new Exception($"Cannot convert {nameof(handle.Result)} to {nameof(GameObject)}.");
                result = go.GetComponent<TComponentType>();
                if (!result)
                    throw new Exception($"Cannot {nameof(go.GetComponent)} of Type {typeof(TComponentType)}.");
                return true;
            }

            result = null;
            return false;
        }
        public static bool TryGetComponentSync<TComponentType>(AssetReferenceT<TComponentType> pRef, out TComponentType result) where TComponentType : Component
        {
            return TryGetComponentSync(pRef as AssetReference, out result);
        }

        public static AsyncOperationHandle<List<AsyncOperationHandle<Object>>> LoadAssetsByLabelAsync(string label)
        {
            var handle = Addressables.ResourceManager.StartOperation(new LoadAssetsByLabelOperation(m_LoadedAssets, m_LoadingAssets, label, AssetLoadedCallback), default);
            return handle;
        }
        static void AssetLoadedCallback(object key, AsyncOperationHandle handle)
        {
            OnAssetLoaded?.Invoke(key, handle);
        }

        /// <summary>
        /// Unloads the given <paramref name="pRef"/> and calls <see cref="DestroyAllInstances"/> if it was Instantiated.
        /// </summary>
        /// <param name="pRef"></param>
        /// <returns></returns>
        public static void Unload(AssetReference pRef)
        {
            CheckRuntimeKey(pRef);

            var key = pRef.RuntimeKey;

            Unload(key);
        }
        static void Unload(object key)
        {
            CheckRuntimeKey(key);

            AsyncOperationHandle handle;
            if (m_LoadingAssets.ContainsKey(key))
            {
                handle = m_LoadingAssets[key];
                m_LoadingAssets.Remove(key);
            }
            else if (m_LoadedAssets.ContainsKey(key))
            {
                handle = m_LoadedAssets[key];
                m_LoadedAssets.Remove(key);
            }
            else
            {
                Debug.LogWarning($"{ERROR}Cannot {nameof(Unload)} RuntimeKey '{key}': It is not loading or loaded.");
                return;
            }

            if (IsInstantiated(key))
                DestroyAllInstances(key);

            Addressables.Release(handle);

            OnAssetUnloaded?.Invoke(key);
        }

        public static void UnloadByLabel(string label)
        {
            if (string.IsNullOrEmpty(label) || string.IsNullOrWhiteSpace(label))
            {
                Debug.LogError("Label cannot be empty.");
                return;
            }

            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            locationsHandle.Completed += op =>
            {
                if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Cannot Unload by label '{label}'");
                    return;
                }
                var keys = GetKeysFromLocations(op.Result);
                foreach (var key in keys)
                {
                    if (IsLoaded(key) || IsLoading(key))
                        Unload(key);
                }
            };

        }

        #endregion

        #region Instantiation

        public static bool TryInstantiateOrLoadAsync(AssetReference pRef, Vector3 position, Quaternion rotation, Transform parent,
            out AsyncOperationHandle<GameObject> handle)
        {
            if (TryGetOrLoadObjectAsync(pRef, out AsyncOperationHandle<GameObject> loadHandle))
            {
                var instance = InstantiateInternal(pRef, loadHandle.Result, position, rotation, parent);
                handle = Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<GameObject>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var instance = InstantiateInternal(pRef, chainOp.Result, position, rotation, parent);
                return Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
            });
            return false;
        }
        //Returns an AsyncOperationHandle<TComponentType> with the result set to an instantiated Component.
        public static bool TryInstantiateOrLoadAsync<TComponentType>(AssetReference pRef, Vector3 position, Quaternion rotation, Transform parent,
            out AsyncOperationHandle<TComponentType> handle) where TComponentType : Component
        {
            if (TryGetOrLoadComponentAsync(pRef, out AsyncOperationHandle<TComponentType> loadHandle))
            {
                var instance = InstantiateInternal(pRef, loadHandle.Result, position, rotation, parent);
                handle = Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<TComponentType>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            //Create a chain that waits for loadHandle to finish, then instantiates and returns the instance GO.
            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var instance = InstantiateInternal(pRef, chainOp.Result, position, rotation, parent);
                return Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
            });
            return false;
        }
        public static bool TryInstantiateOrLoadAsync<TComponentType>(AssetReferenceT<TComponentType> pRef, Vector3 position, Quaternion rotation, Transform parent,
            out AsyncOperationHandle<TComponentType> handle) where TComponentType : Component
        {
            return TryInstantiateOrLoadAsync(pRef as AssetReference, position, rotation, parent, out handle);
        }

        public static bool TryInstantiateMultiOrLoadAsync(AssetReference pRef, int count, Vector3 position, Quaternion rotation, Transform parent,
            out AsyncOperationHandle<List<GameObject>> handle)
        {
            if (TryGetOrLoadObjectAsync(pRef, out AsyncOperationHandle<GameObject> loadHandle))
            {
                var list = new List<GameObject>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(pRef, loadHandle.Result, position, rotation, parent);
                    list.Add(instance);
                }

                handle = Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<List<GameObject>>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var list = new List<GameObject>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(pRef, chainOp.Result, position, rotation, parent);
                    list.Add(instance);
                }

                return Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
            });
            return false;
        }
        public static bool TryInstantiateMultiOrLoadAsync<TComponentType>(AssetReference pRef, int count, Vector3 position, Quaternion rotation, Transform parent,
            out AsyncOperationHandle<List<TComponentType>> handle) where TComponentType : Component
        {
            if (TryGetOrLoadComponentAsync(pRef, out AsyncOperationHandle<TComponentType> loadHandle))
            {
                var list = new List<TComponentType>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(pRef, loadHandle.Result, position, rotation, parent);
                    list.Add(instance);
                }

                handle = Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<List<TComponentType>>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var list = new List<TComponentType>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(pRef, chainOp.Result, position, rotation, parent);
                    list.Add(instance);
                }

                return Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
            });
            return false;
        }
        public static bool TryInstantiateMultiOrLoadAsync<TComponentType>(AssetReferenceT<TComponentType> pRef, int count, Vector3 position, Quaternion rotation,
            Transform parent, out AsyncOperationHandle<List<TComponentType>> handle) where TComponentType : Component
        {
            return TryInstantiateMultiOrLoadAsync(pRef as AssetReference, count, position, rotation, parent, out handle);
        }

        public static bool TryInstantiateSync(AssetReference pRef, Vector3 position, Quaternion rotation, Transform parent, out GameObject result)
        {
            if (!TryGetObjectSync(pRef, out GameObject loadResult))
            {
                result = null;
                return false;
            }

            result = InstantiateInternal(pRef, loadResult, position, rotation, parent);
            return true;
        }
        public static bool TryInstantiateSync<TComponentType>(AssetReference pRef, Vector3 position, Quaternion rotation, Transform parent,
            out TComponentType result) where TComponentType : Component
        {
            if (!TryGetComponentSync(pRef, out TComponentType loadResult))
            {
                result = null;
                return false;
            }

            result = InstantiateInternal(pRef, loadResult, position, rotation, parent);
            return true;
        }
        public static bool TryInstantiateSync<TComponentType>(AssetReferenceT<TComponentType> pRef, Vector3 position, Quaternion rotation, Transform parent,
            out TComponentType result) where TComponentType : Component
        {
            return TryInstantiateSync(pRef as AssetReference, position, rotation, parent, out result);
        }

        public static bool TryInstantiateMultiSync(AssetReference pRef, int count, Vector3 position, Quaternion rotation, Transform parent,
            out List<GameObject> result)
        {
            if (!TryGetObjectSync(pRef, out GameObject loadResult))
            {
                result = null;
                return false;
            }

            var list = new List<GameObject>(count);
            for (int i = 0; i < count; i++)
            {
                var instance = InstantiateInternal(pRef, loadResult, position, rotation, parent);
                list.Add(instance);
            }

            result = list;
            return true;
        }
        public static bool TryInstantiateMultiSync<TComponentType>(AssetReference pRef, int count, Vector3 position, Quaternion rotation, Transform parent,
            out List<TComponentType> result) where TComponentType : Component
        {
            if (!TryGetComponentSync(pRef, out TComponentType loadResult))
            {
                result = null;
                return false;
            }

            var list = new List<TComponentType>(count);
            for (int i = 0; i < count; i++)
            {
                var instance = InstantiateInternal(pRef, loadResult, position, rotation, parent);
                list.Add(instance);
            }

            result = list;
            return true;
        }
        public static bool TryInstantiateMultiSync<TComponentType>(AssetReferenceT<TComponentType> pRef, int count, Vector3 position, Quaternion rotation, Transform parent,
            out List<TComponentType> result) where TComponentType : Component
        {
            return TryInstantiateMultiSync(pRef as AssetReference, count, position, rotation, parent, out result);
        }

        static TComponentType InstantiateInternal<TComponentType>(AssetReference pRef, TComponentType loadedAsset, Vector3 position, Quaternion rotation, Transform parent)
            where TComponentType : Component
        {
            var key = pRef.RuntimeKey;

            var instance = Object.Instantiate(loadedAsset, position, rotation, parent);
            if (!instance)
                throw new NullReferenceException($"Instantiated Object of type '{typeof(TComponentType)}' is null.");

            var monoTracker = instance.gameObject.AddComponent<MonoTracker>();
            monoTracker.key = key;
            monoTracker.onDestroyed += TrackerDestroyed;

            if (!m_InstantiatedObjects.ContainsKey(key))
                m_InstantiatedObjects.Add(key, new List<GameObject>(20));
            m_InstantiatedObjects[key].Add(instance.gameObject);
            return instance;
        }
        static GameObject InstantiateInternal(AssetReference pRef, GameObject loadedAsset, Vector3 position, Quaternion rotation, Transform parent)
        {
            var key = pRef.RuntimeKey;

            var instance = Object.Instantiate(loadedAsset, position, rotation, parent);
            if (!instance)
                throw new NullReferenceException($"Instantiated Object of type '{typeof(GameObject)}' is null.");

            var monoTracker = instance.gameObject.AddComponent<MonoTracker>();
            monoTracker.key = key;
            monoTracker.onDestroyed += TrackerDestroyed;

            if (!m_InstantiatedObjects.ContainsKey(key))
                m_InstantiatedObjects.Add(key, new List<GameObject>(20));
            m_InstantiatedObjects[key].Add(instance);
            return instance;
        }

        static void TrackerDestroyed(MonoTracker tracker)
        {
            if (m_InstantiatedObjects.TryGetValue(tracker.key, out var list))
                list.Remove(tracker.gameObject);
        }

        /// <summary>
        /// Destroys all instantiated instances of <paramref name="pRef"/>
        /// </summary>
        public static void DestroyAllInstances(AssetReference pRef)
        {
            CheckRuntimeKey(pRef);

            if (!m_InstantiatedObjects.ContainsKey(pRef.RuntimeKey))
            {
                Debug.LogWarning($"{nameof(AssetReference)} '{pRef}' has not been instantiated. 0 Instances destroyed.");
                return;
            }

            var key = pRef.RuntimeKey;

            DestroyAllInstances(key);
        }
        static void DestroyAllInstances(object key)
        {
            var instanceList = m_InstantiatedObjects[key];
            for (int i = instanceList.Count - 1; i >= 0; i--)
            {
                DestroyInternal(instanceList[i]);
            }
            m_InstantiatedObjects[key].Clear();
            m_InstantiatedObjects.Remove(key);
        }
        static void DestroyInternal(Object obj)
        {
            var c = obj as Component;
            if (c)
                Object.Destroy(c.gameObject);
            else
            {
                var go = obj as GameObject;
                if (go)
                    Object.Destroy(go);
            }
        }
        #endregion

        #region Utilities
        static void CheckRuntimeKey(AssetReference pRef)
        {
            if (!pRef.RuntimeKeyIsValid())
            {
                //Debug.Log(pRef.RuntimeKey);
                throw new InvalidKeyException($"{ERROR}{nameof(pRef.RuntimeKey)} is not valid for '{pRef}'.");
            }
        }
        static bool CheckRuntimeKey(object key)
        {
            return Guid.TryParse(key.ToString(), out var result);
        }

        static AsyncOperationHandle<TComponentType> ConvertHandleToComponent<TComponentType>(AsyncOperationHandle handle) where TComponentType : Component
        {
            GameObject go = handle.Result as GameObject;
            if (!go)
                throw new Exception($"Cannot convert {nameof(handle.Result)} to {nameof(GameObject)}.");
            TComponentType comp = go.GetComponent<TComponentType>();
            if (!comp)
                throw new Exception($"Cannot {nameof(go.GetComponent)} of Type {typeof(TComponentType)}.");
            var result = Addressables.ResourceManager.CreateCompletedOperation(comp, string.Empty);

            return result;
        }

        static List<object> GetKeysFromLocations(IList<IResourceLocation> locations)
        {
            List<object> keys = new List<object>(locations.Count);

            foreach (var locator in Addressables.ResourceLocators)
            {
                foreach (var key in locator.Keys)
                {
                    bool isGUID = Guid.TryParse(key.ToString(), out var guid);
                    if (!isGUID)
                        continue;

                    if (!TryGetKeyLocationID(locator, key, out var keyLocationID))
                        continue;

                    var locationMatched = locations.Select(x => x.InternalId).ToList().Exists(x => x == keyLocationID);
                    if (!locationMatched)
                        continue;
                    keys.Add(key);
                }
            }

            return keys;
        }
        static bool TryGetKeyLocationID(IResourceLocator locator, object key, out string internalID)
        {
            internalID = string.Empty;
            var hasLocation = locator.Locate(key, typeof(Object), out var keyLocations);
            if (!hasLocation)
                return false;
            if (keyLocations.Count == 0)
                return false;
            if (keyLocations.Count > 1)
                return false;

            internalID = keyLocations[0].InternalId;
            return true;
        }
        #endregion
    }
}

