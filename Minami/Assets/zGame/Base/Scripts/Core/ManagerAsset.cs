using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// by nt.Dev93
namespace ntDev
{
    public static class ManagerAsset
    {
        async public static Task<object> PreLoadAsset(string str, Action<long, long, int> actDownloading = null)
        {
            try
            {
                AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(str);
                AsyncOperationHandle<long> handleSize = Addressables.GetDownloadSizeAsync(str);
                long downloadSize = await handleSize.Task;
                if (downloadSize > 0)
                {
                    Ez.Log("Downloading: " + str + " " + downloadSize + " bytes");
                    CoreGame.Instance.StartCoroutine(StartDownload(handle, actDownloading));
                }
                else Ez.Log("Already had " + str);
                object o = await handle.Task;
                return handle.Task;
            }
            catch (Exception e)
            {
                Ez.Log(e.ToString());
                return null;
            }
        }

        async public static Task<T> LoadAssetAsync<T>(string str, Action<long, long, int> actDownloading = null) where T : UnityEngine.Object
        {
            try
            {
                AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(str);
                AsyncOperationHandle<long> handleSize = Addressables.GetDownloadSizeAsync(str);
                long downloadSize = await handleSize.Task;
                if (downloadSize > 0)
                {
                    Ez.Log("Downloading: " + str + " " + downloadSize + " bytes");
                    CoreGame.Instance.StartCoroutine(StartDownload(handle, actDownloading));
                }
                T o = await handle.Task;
                return o;
            }
            catch (Exception e)
            {
                Ez.Log(e.ToString());
                return null;
            }
        }

        static IEnumerator StartDownload(AsyncOperationHandle handle, Action<long, long, int> actDownloading)
        {
            WaitForSeconds wait = new WaitForSeconds(1);
            while (handle.Status == AsyncOperationStatus.None)
            {
                DownloadStatus downloadStatus = handle.GetDownloadStatus();
                int percent = (int)(downloadStatus.Percent * 100);
                actDownloading?.Invoke(downloadStatus.DownloadedBytes, downloadStatus.TotalBytes, percent);
                yield return null;
            }
        }
    }
}
