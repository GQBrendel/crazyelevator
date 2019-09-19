using TMPro;
using UnityEngine;

public class WavePanel : MonoBehaviour
{
    public delegate void AnimationEndedHandler();
    public AnimationEndedHandler OnAnimationEnded;

    [SerializeField] private TextMeshProUGUI _panelText;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _sliderWaveCount;
    [SerializeField] private ParticleSystem _bubbleEffect;
    [SerializeField] private UISlider _uiSlider;
    private RectTransform _bubbleEffectTransform;
    private readonly float _bubbleMaxSize = 345f;
    private float _bubbleXPos = 0;

    private void Awake()
    {
        _bubbleEffectTransform = _bubbleEffect.GetComponent<RectTransform>();
    }

    public void AnimationEnded()
    {
        OnAnimationEnded?.Invoke();
        gameObject.SetActive(false);
    }
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    float sliderValue = 0f;

    public void UpdateUI(int currentWave, int maxUsers, int scoredUsers)
    {
        currentWave++;
        _panelText.text = "Wave\n" + currentWave;
        _waveText.text = "Wave: " + currentWave;
        _sliderWaveCount.text = scoredUsers + "/" + maxUsers;
        sliderValue = (float)scoredUsers / maxUsers;
        _bubbleXPos = _bubbleMaxSize * sliderValue;
        Debug.Log(_bubbleXPos);
        if(_bubbleXPos > 0)
        {
            _bubbleEffectTransform.anchoredPosition = new Vector3(_bubbleXPos, 0, 0);
            _bubbleEffect.Play();
        }

        _uiSlider.UpdateSliderValue(sliderValue);

        if (maxUsers == int.MaxValue)
        {
            _panelText.text = "Final\nWave";
            _waveText.text = "Final Wave";
            _sliderWaveCount.text = scoredUsers + "/∞";
             sliderValue = (sliderValue > 500) ? 500f : (float)scoredUsers / 500f;
        }
    }
}
