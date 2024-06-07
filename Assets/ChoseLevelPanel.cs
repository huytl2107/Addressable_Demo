using UnityEngine;
using UnityEngine.UI;

public class ChoseLevelPanel : MonoBehaviour
{
    [SerializeField] private Loading _loadingPanel;
    [SerializeField] private Button lv1, lv2, btnBack;
    private void OnEnable()
    {
        lv1.onClick.AddListener(delegate{OnButtonLevelClick(1);});
        lv2.onClick.AddListener(delegate{OnButtonLevelClick(2);});
        btnBack.onClick.AddListener(OnButtonBackClick);
    }

    private void OnDisable()
    {
        lv1.onClick.RemoveAllListeners();
        lv2.onClick.RemoveAllListeners();
        btnBack.onClick.RemoveAllListeners();
    }

    private void OnButtonLevelClick(int lv)
    {
        _loadingPanel.SetLevel(lv);
        _loadingPanel.gameObject.SetActive(true);
    }

    private void OnButtonBackClick()
    {
        this.gameObject.SetActive(false);
    }
}
