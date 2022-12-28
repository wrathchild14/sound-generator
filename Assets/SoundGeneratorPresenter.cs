using UnityEngine;
using UnityEngine.UI;

public class SoundGeneratorPresenter : MonoBehaviour
{
    public Generator generator;

    private Slider[] _sliders;

    private void Start()
    {
        _sliders = GetComponentsInChildren<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void UpdateSliders()
    {
        foreach (var slider in _sliders) slider.value = generator.volume;
    }

    public void RandomizeAndPlaySoundClip()
    {
        generator.Randomize();
        generator.GenerateAndPlaySoundClip();
        UpdateSliders();
    }
}