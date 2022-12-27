using System;
using UnityEngine;

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
        const int sampleRate = 44100;
        var duration = attackTime + decayTime + releaseTime;
        var numSamples = (int)(sampleRate * duration);
        var samples = new float[numSamples];
        for (var i = 0; i < numSamples; i++)
        {
            var t = i / (float)sampleRate;
            float envelope;
            if (t < attackTime)
                envelope = t / attackTime;
            else if (t < attackTime + decayTime)
                envelope = 1 - (t - attackTime) / decayTime * (1 - sustainLevel);
            else
                envelope = sustainLevel * (1 - (t - attackTime - decayTime) / releaseTime);

            var frequency = startFrequency + (cutoffFrequency - startFrequency) * t / duration * slideRate;
            var vibrato = Mathf.Sin(2 * Mathf.PI * vibratoFrequency * t) * vibratoDepth;
            var arpeggio = Mathf.Pow(2,
                (Mathf.Sin(2 * Mathf.PI * arpeggioFrequency * t) * arpeggioOctaveRange + arpeggioOctaveRange) / 12) - 1;

            var phase = (frequency * arpeggio + vibrato) * t * 2 * Mathf.PI;
            switch (type)
            {
                case SoundWave.Square:
                    samples[i] = Mathf.Sign(Mathf.Sin(phase)) * envelope * volume;
                    break;
                case SoundWave.Sawtooth:
                    samples[i] = (2 * (phase % (2 * Mathf.PI)) / (2 * Mathf.PI) - 1) * envelope * volume;
                    break;
                case SoundWave.Noise:
                    var noise = Mathf.PerlinNoise(t * noiseFrequency, 0) * 2 - 1;
                    samples[i] = noise * envelope * volume;
                    break;
                case SoundWave.Sine:
                    samples[i] = Mathf.Sin(phase) * envelope * volume;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        // removing 3d because is deprecated
        var clip = AudioClip.Create("Jump Sound", numSamples, 1, sampleRate, false);
        clip.SetData(samples, 0);

        audioSource.clip = clip;
    }

    public void PlayClip()
    {
        audioSource.Play();
    }
}