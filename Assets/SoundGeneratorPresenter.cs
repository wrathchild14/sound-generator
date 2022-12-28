using UnityEngine;
using UnityEngine.UI;

public class SoundGeneratorPresenter : MonoBehaviour
{
    public Generator generator;
    private float[] _generatorValues;

    private Slider[] _sliders;

    private void Start()
    {
        _sliders = GetComponentsInChildren<Slider>();
    }

    private void UpdateSliders()
    {
        // note: must be in the correct order
        _generatorValues = generator.GetCurrentValues();
        for (var i = 0; i < Generator.ParametersCount; i++) _sliders[i].value = _generatorValues[i];
    }

    public void RandomizeAndPlaySoundClip()
    {
        generator.Randomize();
        generator.GenerateAndPlaySoundClip();
        UpdateSliders();
    }
}