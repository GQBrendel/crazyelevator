using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public delegate void WaveStartHandler();
    public WaveStartHandler OnWaveStarted;

    [SerializeField] private int[] _waveCount;
    [SerializeField] private WavePanel _wavePanel;

    public int CurrentWave { get; set; }
    public int CurrentWaveCount
    {
        get
        {
            if(CurrentWave < _waveCount.Length)
            {
                return _waveCount[CurrentWave];
            }
            return int.MaxValue;
        } }
    public int SpawnedUsers { get; set; }

    private void Awake()
    {
        _wavePanel.OnAnimationEnded += HandleAnimationEnded;
        _wavePanel.UpdateUI(0, CurrentWaveCount, 0);
    }
    private void HandleAnimationEnded()
    {
        OnWaveStarted?.Invoke();
    }

    public void CallNextWave()
    {
        SpawnedUsers = 0;
        CurrentWave++;
        _wavePanel.UpdateUI(CurrentWave, CurrentWaveCount, SpawnedUsers);
        _wavePanel.ShowPanel();
    }

    public void UpdateUI(int scoredUsers)
    {
        _wavePanel.UpdateUI(CurrentWave, CurrentWaveCount, scoredUsers);
    }
}
