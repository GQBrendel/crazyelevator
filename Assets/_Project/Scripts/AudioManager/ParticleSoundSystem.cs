using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSoundSystem : MonoBehaviour
{
    private ParticleSystem _parentParticleSystem;

    private int _currentNumberOfParticles = 0;

    public AudioClip[] BornSounds;
    public AudioClip[] DieSounds;

    //private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSourcePrefab;

    void Start()
    {
       // _audioSource = GetComponent<AudioSource>();
        _parentParticleSystem = this.GetComponent<ParticleSystem>();
        if (_parentParticleSystem == null)
            Debug.LogError("Missing ParticleSystem!", this);

    }

    // Update is called once per frame
    void Update()
    {
        var amount = Mathf.Abs(_currentNumberOfParticles - _parentParticleSystem.particleCount);

        if (_parentParticleSystem.particleCount < _currentNumberOfParticles)
        {
            StartCoroutine(PlaySound(DieSounds[Random.Range(0, DieSounds.Length)], amount));
        }

       /* if (_parentParticleSystem.particleCount > _currentNumberOfParticles)
        {
            StartCoroutine(PlaySound(BornSounds[Random.Range(0, BornSounds.Length)], amount));
        }*/

        _currentNumberOfParticles = _parentParticleSystem.particleCount;
    }

    private IEnumerator PlaySound(AudioClip clip, int amount)
    {

        //Debug.Log($"Distance to player '{distanceToPlayer}', therefore delayed sound of '{soundDelay}' sec.");

        for (int i = 0; i < amount; i++)
        {          
            StartCoroutine(PlaySound(clip, 0f));

            //Attempt to avoid multiple of the same audio being played at the exact same time - as it sounds wierd
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator PlaySound(AudioClip clip, float delayInSeconds)
    {
       // _audioSource.clip = clip;
        yield return new WaitForSeconds(delayInSeconds);
        Instantiate(_audioSourcePrefab.gameObject);
//        _audioSource.Play();
    }
}