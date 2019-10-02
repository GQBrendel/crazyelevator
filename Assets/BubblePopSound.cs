using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePopSound : MonoBehaviour
{
    [SerializeField] private AudioSource _bubblePopSoundPrefab;
    [SerializeField] private int _numberOfInstances;
    [SerializeField] private float _volumeVariance = 1f;
    [SerializeField] private float _pitchVariance = 1f;

    public List<AudioSource> _audios;

    void Start()
    {
        _audios = new List<AudioSource>();
        
        for(int i = 0; i < _numberOfInstances; i++)
        {
            var a = Instantiate(_bubblePopSoundPrefab, transform);
            _audios.Add(a.GetComponent<AudioSource>());
        }
    }

    public void PlayBubbleSound(int repeat)
    {
        StartCoroutine(PlayAllWithDelay(repeat));
    }
    private IEnumerator PlayAllWithDelay(int repeat)
    {
        if(repeat > _numberOfInstances)
        {
            repeat = _numberOfInstances;
            Debug.LogWarningFormat("Tried to play {0} sounds repetitions, but the max number is {1}" , repeat, _numberOfInstances);
        }
        for(int i = 0; i < repeat; i++)
        {
            _audios[i].volume = _audios[i].volume * (1f + UnityEngine.Random.Range(-_volumeVariance / 2f, _volumeVariance / 2f));
            _audios[i].pitch = _audios[i].pitch * (1f + UnityEngine.Random.Range(-_pitchVariance / 2f, _pitchVariance / 2f));

            _audios[i].Play();
            float randomDelay = Random.Range(0.1f, 0.12f);

            yield return new WaitForSeconds(randomDelay);

        }
    }
}
