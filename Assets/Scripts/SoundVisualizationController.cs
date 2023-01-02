using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVisualizationController : MonoBehaviour
{
    private const float WaveHeight = 1.0f;
    private const float EmissionThreshold = 0.5f;

    public AudioSource audioSource;
    public ParticleSystem ballsParticleSystem;
    [Header("SoundWave")] public RawImage soundWaveImage;

    private void Update()
    {
        UpdateBallsParticleSystem();
    }

    private void UpdateBallsParticleSystem()
    {
        var spectrumData = new float[256];
        var outputData = new float[256];
        audioSource.GetOutputData(outputData, 0);
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

        UpdateParticlePosition(spectrumData);
        UpdateEmission(outputData);
    }

    private void UpdateEmission(IReadOnlyCollection<float> outputData)
    {
        var audioAmplitude = 0f;
        foreach (var t in outputData) audioAmplitude += Mathf.Abs(t);
        audioAmplitude /= outputData.Count;
        if (audioAmplitude > EmissionThreshold) ballsParticleSystem.Emit(2);
    }

    private void UpdateParticlePosition(IReadOnlyList<float> spectrumData)
    {
        var particles = new ParticleSystem.Particle[ballsParticleSystem.particleCount];
        ballsParticleSystem.GetParticles(particles);
        for (var i = 0; i < particles.Length; i++)
        {
            var particle = particles[i];
            var multiplier = Random.Range(0, 2) * 2 - 1;
            particle.position +=
                Vector3.up * (spectrumData[i] * WaveHeight * multiplier);
            particles[i] = particle;
        }

        ballsParticleSystem.SetParticles(particles, particles.Length);
    }

    public void DrawSoundWave(float[] data)
    {
        // width height are hardcoded
        if (data != null)
            soundWaveImage.texture =
                DrawSoundWaveTimeDomain(data, 1000, 136, Color.black); // not sure why the must be hardcoded, is bug
    }

    private static Texture2D DrawSoundWaveTimeDomain(IReadOnlyList<float> data, int width, int height, Color col,
        Texture2D tex = null)
    {
        if (width < 1 || height < 1) return null;

        var backgroundColor = new Color(0, 0, 0, 0);

        if (tex == null)
            tex = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };

        if (data != null)
        {
            // condense clip contents
            var waveform = new float[width];
            for (var w = 0; w < waveform.Length; w++)
                waveform[w] = data[Mathf.RoundToInt((float)w / waveform.Length * (data.Count - 1))];

            // clear
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                tex.SetPixel(x, y, backgroundColor);

            // draw
            for (var x = 0; x < width; x++)
            for (var y = 0; y < waveform[x] * (height * .5f); y++)
            {
                tex.SetPixel(x, height / 2 + y, col);
                tex.SetPixel(x, height / 2 - y, col);
            }
        }
        else
        {
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                tex.SetPixel(x, y, col);
        }

        tex.Apply();
        return tex;
    }
}