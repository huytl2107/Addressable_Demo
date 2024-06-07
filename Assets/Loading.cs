using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Threading;

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
        btnCancel.onClick.AddListener(OnButtonCancel);
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
        downloadCoroutine = StartCoroutine("LoadGamePlay");
    }

    private void OnButtonAbortClick()
    {
        btnDownload.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
        //StopAllCoroutines();

        if (downloadCoroutine != null)
        {
            StopCoroutine(downloadCoroutine);
        }

        // Huỷ tải về nếu đang có
        if (downloadHandler.IsValid())
        {
            Addressables.Release(downloadHandler); // Release the handle
            downloadHandler = default(AsyncOperationHandle);
        }
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
            while (!downloadHandler.IsDone)
            {
                _loadingSlider.value = downloadHandler.PercentComplete;
                yield return null;
            }
            yield return downloadHandler;
        }
        SceneManager.LoadSceneAsync(_levelLoad, LoadSceneMode.Single);
    }

    private void OnButtonDeleteClick()
    {
        string key = "lv" + _levelLoad.ToString();

        Addressables.ClearDependencyCacheAsync(key);
    }
}
