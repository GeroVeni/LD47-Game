using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMPro.TextMeshProUGUI percent;

    public void SetValue(float value, float maxValue)
    {
        value = Mathf.Clamp(value, 0, maxValue);
        slider.value = value / maxValue;
        percent.text = ((int)value).ToString() + " / " + ((int)maxValue).ToString();
    }
}
