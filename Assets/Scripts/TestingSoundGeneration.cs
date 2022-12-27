using UnityEngine;

public class TestingSoundGeneration : MonoBehaviour
{
    public AudioSource audioSource;

    private Generator _generator;

    // Start is called before the first frame update
    private void Start()
    {
        _generator = gameObject.AddComponent<Generator>();
        _generator.audioSource = audioSource;
    }

    public void TestFunction()
    {
        _generator.CreateJumpingSound();
        _generator.PlayClip();
    }
}