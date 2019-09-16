using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WavePanel : MonoBehaviour
{
    public delegate void AnimationEndedHandler();
    public AnimationEndedHandler OnAnimationEnded;

    [SerializeField] private TextMeshProUGUI _panelText;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _sliderWaveCount;
    [SerializeField] private Slider _waveSlider;

    public void AnimationEnded()
    {
        OnAnimationEnded?.Invoke();
        gameObject.SetActive(false);
    }
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void UpdateUI(int currentWave, int maxUsers, int scoredUsers)
    {
        currentWave++;
        _panelText.text = "Wave\n" + currentWave;
        _waveText.text = "Wave: " + currentWave;
        _sliderWaveCount.text = scoredUsers + "/" + maxUsers;
        float sliderValue = (float)scoredUsers / maxUsers;
        _waveSlider.value = sliderValue;

        if (maxUsers == int.MaxValue)
        {
            _panelText.text = "Final\nWave";
            _waveText.text = "Final Wave";
            _sliderWaveCount.text = scoredUsers + "/∞";
             sliderValue = (sliderValue > 500) ? 500f : (float)scoredUsers / 500f;
            _waveSlider.value = sliderValue;
        }
    }
}
