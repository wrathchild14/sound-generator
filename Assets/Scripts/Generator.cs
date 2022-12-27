using UnityEngine;
using UnityEngine.Serialization;

public class Generator : MonoBehaviour
{
    public AudioSource audioSource;

    public float frequency = 440;
    public float volume = 1;
    public float duration = 0.1f;

    public void CreateJumpingSound()
    {
        const int sampleRate = 44100;
        var numSamples = (int)(sampleRate * duration);
        var samples = new float[numSamples];
        for (var i = 0; i < numSamples; i++)
        {
            float t = i / (float)sampleRate;
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * Mathf.Pow(1 - t / duration, 3) * volume;
        }

        var clip = AudioClip.Create("Jump Sound", numSamples, 1, sampleRate, false, false);
        clip.SetData(samples, 0);

        audioSource.clip = clip;
    }

    public void PlayClip()
    {
        audioSource.Play();
    }
}