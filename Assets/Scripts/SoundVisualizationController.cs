using UnityEngine;

public class SoundVisualizationController : MonoBehaviour
{
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        var data = new float[256];
        audioSource.GetOutputData(data, 0);

        for (var i = 0; i < data.Length; i++)
        {
            // Use the audio data to drive the visualization
            transform.localScale = new Vector3(1, data[i], 1);
        }
    }
}