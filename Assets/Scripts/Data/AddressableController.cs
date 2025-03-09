using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class AddressableController : SingletonClass<AddressableController>, IService
{
    private static HashSet<object> _keys = new HashSet<object>();
    public bool isInited = false;

    public void Init()
    {
        Addressables.InitializeAsync().Completed += Addressables_CompletedAsync;
    }

    private void Addressables_CompletedAsync(AsyncOperationHandle<IResourceLocator> handle)
    {
        Debug.Log("Addressables Init Completed");
        _keys = new HashSet<object>(handle.Result.Keys);
        isInited = true;
        //Debug.Log(isInited);
    }

    public T LoadAssetByName<T>(string nameAsset) where T : Object
    {
        if (!isInited)
        {
            return null;
        }

        T res = default;
        var load = Addressables.LoadAssetAsync<T>(nameAsset);
        load.Completed += (operationHandle) =>
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                res = operationHandle.Result;
            }
        };
        load.WaitForCompletion(); // có th? dùng LoadAsset ?? ch?y ??ng b? nh?ng ch?a test

        return res;
    }

    //public void LoadAssetByName<T>(string nameAsset, Action<T> complateAction, bool isWait = false) where T : Object
    //{
    //    if (!isInited)
    //    {
    //        Context.Waiting.HideWaiting();
    //        return;
    //    }

    //    T res = default;
    //    var load = Addressables.LoadAssetAsync<T>(nameAsset);
    //    load.Completed += (operationHandle) =>
    //    {
    //        if (operationHandle.Status == AsyncOperationStatus.Succeeded)
    //        {
    //            res = operationHandle.Result;

    //            if (complateAction != null)
    //                complateAction.Invoke(res);
    //        }
    //        else if (operationHandle.Status == AsyncOperationStatus.Failed)
    //        {
    //            Context.Waiting.HideWaiting();
    //        }
    //    };

    //    if (isWait)
    //    {
    //        load.WaitForCompletion(); // có thể dùng LoadAsset để chạy đồng bộ nhưng chưa test
    //    }
    //}

    public void LoadSceneByName(string nameAsset, bool activeOnLoad = false, Action<SceneInstance> complateAction = null)
    {
        if (!isInited)
        {
            return;
        }

        var load = Addressables.LoadSceneAsync(nameAsset, LoadSceneMode.Single, activeOnLoad);

        load.Completed += (operationHandle) =>
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (complateAction != null)
                    complateAction.Invoke(operationHandle.Result);
            }
        };
    }

    public IObservable<T> LoadObservableAssetByName<T>(string nameAsset) where T : Object
    {
        if (!isInited)
        {
            return null;
        }

        T res = default;
        var load = Addressables.LoadAssetAsync<T>(nameAsset);
        load.Completed += (operationHandle) =>
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                res = operationHandle.Result;
            }
        };
        load.WaitForCompletion(); // có th? dùng LoadAsset ?? ch?y ??ng b? nh?ng ch?a test

        return Observable.Return(res);
    }

    public bool IsValidAddress(string address)
    {
        return _keys.Contains(address);
    }

    IEnumerator ClearAllAssetCoro()
    {
        foreach (var locats in Addressables.ResourceLocators)
        {
            var async = Addressables.ClearDependencyCacheAsync(locats.Keys, false);
            yield return async;
            Addressables.Release(async);
        }
        Caching.ClearCache();
    }

    IEnumerator ClearAssetCoro(string label)
    {
        var async = Addressables.LoadResourceLocationsAsync(label);
        yield return async;
        var locats = async.Result;
        foreach (var locat in locats)
            Addressables.ClearDependencyCacheAsync(locat.PrimaryKey);
    }
}
