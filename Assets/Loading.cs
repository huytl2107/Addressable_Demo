using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.AddressableAssets.ResourceLocators;

public class Loading : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject[] _resources;
    [SerializeField] private AssetReferenceGameObject[] _resourcesLv1;
    [SerializeField] private AssetReferenceGameObject[] _resourcesLv2;
    private static AsyncOperationHandle<GameObject> _resourceLoadOpHandle;

    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private Button btnDownload, btnCancel, btnDelete;
    private bool _isSceneLoading = false;
    private float percent = 0f;
    private int _levelLoad;
    private Coroutine downloadCoroutine;
    private AsyncOperationHandle downloadHandler;

    public void SetLevel(int lv)
    {
        _levelLoad = lv;
    }

    //private GameObject m_PlayButton, m_LoadingText;

    private void Awake()
    {
        _loadingSlider.gameObject.SetActive(false);
        btnDownload.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        btnDownload.onClick.AddListener(OnButtonDownloadClick);
        btnCancel.onClick.AddListener(OnButtonAbortClick);
        btnDelete.onClick.AddListener(OnButtonDeleteClick);
    }

    private void OnDisable()
    {
        _loadingSlider.gameObject.SetActive(false);
        btnDownload.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(false);

        btnDownload.onClick.RemoveAllListeners();
        btnCancel.onClick.RemoveAllListeners();
        btnDelete.onClick.RemoveAllListeners();
    }
    private void OnButtonDownloadClick()
    {
        btnCancel.gameObject.SetActive(true);
        btnDownload.gameObject.SetActive(false);
        _loadingSlider.gameObject.SetActive(true);

        downloadCoroutine = StartCoroutine("UpdateCatalogs");
    }

    private void OnButtonDeleteClick()
    {
        // string key = "lv" + _levelLoad.ToString();
        // Addressables.ClearDependencyCacheAsync(key);
        StartCoroutine("DeleteData");
    }

    private IEnumerator DeleteData()
    {
        string key = "lv" + _levelLoad.ToString();

        if (downloadHandler.IsValid())
        {
            Addressables.Release(key);
            Addressables.Release(downloadHandler); // Release the handle
            downloadHandler = default(AsyncOperationHandle);
            yield return null;
        }
        Addressables.ClearDependencyCacheAsync(key);
    }

    private void OnButtonAbortClick()
    {
        btnDownload.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(false);

        //this.gameObject.SetActive(false);
        //StopAllCoroutines();

        if (downloadCoroutine != null)
        {
            Debug.Log("Stop Courotine Download");
            StopCoroutine(downloadCoroutine);
        }

        // Huỷ tải về
        StartCoroutine("DeleteData");
    }

    private IEnumerator LoadGamePlay()
    {
        string key = "lv" + _levelLoad.ToString();

        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;

        //If the download size is greater than 0, download all the dependencies.
        if (getDownloadSize.Result > 0)
        {
            //AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key);
            downloadHandler = Addressables.DownloadDependenciesAsync(key);
            while (!downloadHandler.IsDone && downloadHandler.IsValid())
            {
                _loadingSlider.value = downloadHandler.PercentComplete;
                yield return null;
            }
            yield return downloadHandler;
        }
        SceneManager.LoadSceneAsync(_levelLoad, LoadSceneMode.Single);
    }

    IEnumerator UpdateCatalogs()
    {
        List<string> catalogsToUpdate = new List<string>();
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
        checkForUpdateHandle.Completed += op =>
        {
            catalogsToUpdate.AddRange(op.Result);
        };
        yield return checkForUpdateHandle;
        if (catalogsToUpdate.Count > 0)
        {
            Debug.Log("Phat hien " + catalogsToUpdate.Count + " update");
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
            yield return updateHandle;
        }
        else
            Debug.Log("Khong co update");
        SceneManager.LoadSceneAsync(_levelLoad, LoadSceneMode.Single);
    }

    // private IEnumerator LoadGamePlay()
    // {
    //     string key = "lv" + _levelLoad.ToString();

    //     AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
    //     yield return getDownloadSize;

    //     //If the download size is greater than 0, download all the dependencies.
    //     if (getDownloadSize.Result > 0)
    //     {
    //         downloadHandler = Addressables.DownloadDependenciesAsync(key);
    //         while (!downloadHandler.IsDone && downloadHandler.IsValid())
    //         {
    //             _loadingSlider.value = downloadHandler.PercentComplete;
    //             yield return null;
    //         }
    //         yield return downloadHandler;

    //         AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates(false);
    //         yield return checkForUpdateHandle;

    //         // Kiểm tra xem có cập nhật nào không
    //         if (checkForUpdateHandle.Status == AsyncOperationStatus.Succeeded)
    //         {
    //             List<string> catalogs = checkForUpdateHandle.Result;
    //             foreach (string catalog in catalogs)
    //             {
    //                 if (catalog.Contains(key))
    //                 {
    //                     // Có cập nhật, thực hiện tải lại
    //                     Debug.Log("There is an update for " + key);

    //                     // Tiến hành tải lại
    //                     downloadHandler = Addressables.DownloadDependenciesAsync(key);
    //                     while (!downloadHandler.IsDone && downloadHandler.IsValid())
    //                     {
    //                         _loadingSlider.value = downloadHandler.PercentComplete;
    //                         yield return null;
    //                     }
    //                     yield return downloadHandler;

    //                     break; // Thoát vòng lặp, không cần kiểm tra các catalog khác nữa
    //                 }
    //                 else
    //                 {
    //                     Debug.Log(key + "not have update");
    //                 }
    //             }
    //         }
    //         yield return downloadHandler;
    //     }

    //     SceneManager.LoadSceneAsync(_levelLoad, LoadSceneMode.Single);
    // }

}
