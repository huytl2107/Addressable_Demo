using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _chooseLevelPanel;
    [SerializeField] private Text _text;

    private void OnEnable()
    {
        _text.text = PlayerPrefs.GetInt("Count").ToString();
        btnPlay.onClick.AddListener(OnButtonPlayClick);
        _loadingPanel.gameObject.SetActive(false);
        _chooseLevelPanel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        btnPlay.onClick.RemoveAllListeners();
    }

    private void OnButtonPlayClick()
    {
        Debug.Log("Button Play Clicked");
        PlayerPrefs.SetInt("Count", PlayerPrefs.GetInt("Count") + 1);
        _chooseLevelPanel.gameObject.SetActive(true);
    }

    // private void DownLoadAddressAbleAsset()
    // {
    //     _sceneLoadOpHandle = Addressables.LoadSceneAsync("LoadingScene", activateOnLoad: true);

    //     //_player.LoadAssetAsync().Completed += OnAddressableLoaded;
    // }
}
