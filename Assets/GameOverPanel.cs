using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button btnHome;

    private void OnEnable()
    {
        btnHome.onClick.AddListener(OnButtonHomeClick);
    }

    private void OnDisable()
    {
        btnHome.onClick.RemoveAllListeners();
    }

    private void OnButtonHomeClick()
    {
        SceneManager.LoadScene(0);
    }
}
