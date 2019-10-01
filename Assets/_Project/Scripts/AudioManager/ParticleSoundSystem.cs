using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSoundSystem : MonoBehaviour
{
    private ParticleSystem _parentParticleSystem;

    private int _currentNumberOfParticles = 0;

    void Start()
    {
        _parentParticleSystem = this.GetComponent<ParticleSystem>();
        if (_parentParticleSystem == null)
            Debug.LogError("Missing ParticleSystem!", this);
    }

    void Update()
    {
        var amount = Mathf.Abs(_currentNumberOfParticles - _parentParticleSystem.particleCount);

        if (_parentParticleSystem.particleCount < _currentNumberOfParticles)
        {
            StartCoroutine(PlaySound(amount));
        }
        _currentNumberOfParticles = _parentParticleSystem.particleCount;
    }

    private IEnumerator PlaySound(int amount)
    {
        for (int i = 0; i < amount * 2; i++)
        {
            StartCoroutine(PlaySound(0f));

            yield return null;
        }
    }

    private IEnumerator PlaySound(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        AudioManager.instance.Play("BubblePop");
    }
}