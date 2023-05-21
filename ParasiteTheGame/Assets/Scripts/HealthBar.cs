using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public bool is_dynamic { get; set; }
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    private float lastHealthUpdate;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        is_dynamic = true;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(int value)
    {
        gameObject.SetActive(true);
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        lastHealthUpdate = Time.time;
    }

    private void Update()
    {
        if (!is_dynamic || !gameObject.activeSelf)
            return;
        if (Time.time - lastHealthUpdate > 5)
            gameObject.SetActive(false);
    }
}
