using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _creditsPanel;

    void Start()
    {
        _playButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        _creditsButton.onClick.AddListener(() => { _creditsPanel.SetActive(true); });
        _backButton.onClick.AddListener(() => { _creditsPanel.SetActive(false); });
    }
}
