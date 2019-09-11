using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private AudioMixer _mainMixer;
    private bool _muted;

    private void Awake()
    {
        _playButton.onClick.AddListener(HandlePlayButtonClicked);
        _audioButton.onClick.AddListener(HandleAudioButtonClicked);
        _creditsButton.onClick.AddListener(HandleCreditsButtonClicked);
        _backButton.onClick.AddListener(handleBackButtonClicked);

    }
    private void Start()
    {
        AudioManager.instance.Play("MainSound");
    }

    private void HandlePlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    private void HandleAudioButtonClicked()
    {
        AudioManager.instance.Mute();
    }
    private void HandleCreditsButtonClicked()
    {
        _creditsPanel.SetActive(true);
    }
    private void handleBackButtonClicked()
    {
        _creditsPanel.SetActive(false);
    }

}
