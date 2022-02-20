using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public float currentValue;

    private void Awake()
    {
        currentValue = 1;
    }

    public void SetHealthBar(float f)
    {
        currentValue = f;
    }
    private void Update()
    {
        slider.value = currentValue;
    }
}
