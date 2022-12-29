using UnityEngine;
using UnityEngine.Serialization;

public class SoundVisualizationController : MonoBehaviour
{
    public AudioSource audioSource;

    public ParticleSystem ballsParticleSystem;

    // Update is called once per frame
    private void Update()
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
}