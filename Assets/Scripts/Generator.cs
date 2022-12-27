using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Generator : MonoBehaviour
{
    public enum SoundWave
    {
        Sine,
        Square,
        Sawtooth,
        Noise
    }

    public AudioSource audioSource;

    public float startFrequency = 440;
    public float cutoffFrequency = 880;
    public float slideRate = 1;
    public float vibratoFrequency = 5;
    public float vibratoDepth = 1;
    public float volume = 1;
    public float attackTime = 0.1f;
    public float decayTime = 0.1f;
    public float sustainLevel = 0.5f;
    public float releaseTime = 0.1f;
    public float arpeggioFrequency = 4;
    public int arpeggioOctaveRange = 2;

    public float noiseFrequency = 10;

    public SoundWave type = SoundWave.Sine;

    public void CreateJumpingSound()
    {
        int sampleRate = 44100;
        float duration = attackTime + decayTime + releaseTime;
        int numSamples = (int)(sampleRate * duration);
        float[] samples = new float[numSamples];
        for (int i = 0; i < numSamples; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = 1;
            if (t < attackTime)
            {
                envelope = t / attackTime;
            }
            else if (t < attackTime + decayTime)
            {
                envelope = 1 - (t - attackTime) / decayTime * (1 - sustainLevel);
            }
            else
            {
                envelope = sustainLevel * (1 - (t - attackTime - decayTime) / releaseTime);
            }

            float frequency = startFrequency + (cutoffFrequency - startFrequency) * t / duration * slideRate;
            float vibrato = Mathf.Sin(2 * Mathf.PI * vibratoFrequency * t) * vibratoDepth;
            float arpeggio = Mathf.Pow(2,
                (Mathf.Sin(2 * Mathf.PI * arpeggioFrequency * t) * arpeggioOctaveRange + arpeggioOctaveRange) / 12) - 1;

            float phase = (frequency * arpeggio + vibrato) * t * 2 * Mathf.PI;
            switch (type)
            {
                case SoundWave.Square:
                    samples[i] = Mathf.Sign(Mathf.Sin(phase)) * envelope * volume;
                    break;
                case SoundWave.Sawtooth:
                    samples[i] = (2 * (phase % (2 * Mathf.PI)) / (2 * Mathf.PI) - 1) * envelope * volume;
                    break;
                case SoundWave.Noise:
                    float noise = Mathf.PerlinNoise(t * noiseFrequency, 0) * 2 - 1;
                    samples[i] = noise * envelope * volume;
                    break;
                case SoundWave.Sine:
                    samples[i] = Mathf.Sin(phase) * envelope * volume;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        AudioClip clip = AudioClip.Create("Jump Sound", numSamples, 1, sampleRate, false, false);
        clip.SetData(samples, 0);

        audioSource.clip = clip;
    }

    public void PlayClip()
    {
        audioSource.Play();
    }
}