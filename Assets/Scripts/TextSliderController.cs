using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextSliderController : MonoBehaviour
{
    public Slider slider;
    public Text text;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChanged(); });
    }

    private void ValueChanged()
    {
        text.text = slider.value.ToString(CultureInfo.InvariantCulture);
    }
}