using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private bool is_dynamic;
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    private float lastHealthUpdate;

    public void SetMaxHealth(IDamagable creature, int health, bool dynamic)
    {
        if (creature is AEnemy enemy)
            SetGradientForEnemy(enemy);
        slider.maxValue = health;
        slider.value = health;
        is_dynamic = dynamic;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetValue(int value)
    {
        gameObject.SetActive(true);
        slider.value = value > 0 ? value : 0;
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

    private void SetGradientForEnemy(AEnemy enemy)
    {
        var colorKeys = gradient.colorKeys;
        colorKeys[1].time = enemy.RelativeTerminalHealth;
        gradient.colorKeys = colorKeys;
    }
}
