using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFade
{
    public static bool isFading;
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        isFading = true;
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
        isFading = false;
    }
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float max)
    {
        while (isFading)
        {
            yield return new WaitForSeconds(0.5f);
        }
        float startVolume = 0.2f;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < max)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        isFading = false;
        audioSource.volume = max;
    }
}