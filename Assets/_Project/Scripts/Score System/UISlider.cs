using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISlider : MonoBehaviour
{
    [SerializeField] private float _fillSpeed = 0.2f;
    private Slider _slider;
    private float _sliderValue = 0f;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateSliderValue(float value)
    {
        _sliderValue = value;
        if(value == 0)
        {
            _slider.value = value;
        }
    }

    void Update()
    {
        if (_slider.value < _sliderValue)
        {
            _slider.value += _fillSpeed * Time.deltaTime;
            if (_slider.value > _sliderValue)
            {
                _slider.value = _sliderValue;
            }
        }
    }
}
