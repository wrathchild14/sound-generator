using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVisualizationController : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem ballsParticleSystem;
    [Header("SoundWave")] public RawImage soundWaveImage;

    private void Update()
    {
        UpdateBallsParticleSystem();
    }

    private void UpdateBallsParticleSystem()
    {
        var data = new float[256];
        audioSource.GetOutputData(data, 0);
        for (var i = 0; i < data.Length; i++)
        {
            // Use the audio data to drive the visualization
            var particleSystemSizeOverLifetime = ballsParticleSystem.sizeOverLifetime;
            particleSystemSizeOverLifetime.sizeMultiplier = data[i] + 1.0f;
        }
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