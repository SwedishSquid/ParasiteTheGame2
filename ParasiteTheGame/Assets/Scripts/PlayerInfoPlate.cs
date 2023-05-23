using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfoPlate : MonoBehaviour
{
    [SerializeField] private HealthBar healthBarPlayer;
    [SerializeField] private HealthBar healthBarEnemy;

    public void AwakeData(PlayerController player)
    {
        healthBarPlayer.SetMaxHealth(player.GetMaxHealth(), false);
    }

    public void UpdateData(PlayerController player)
    {
        UpdatePlayerHealthBar(player);
        UpdateEnemyHealthBar(player);
    }

    private void UpdatePlayerHealthBar(PlayerController player)
    {
        healthBarPlayer.SetValue(player.GetHealth());
    }
    
    private void UpdateEnemyHealthBar(PlayerController player)
    {
        if (player.controlled is null && healthBarEnemy.gameObject.activeSelf)
            healthBarEnemy.gameObject.SetActive(false);
        else if (player.controlled is AEnemy enemy)
        {
            if (healthBarEnemy.gameObject.activeSelf)
                healthBarEnemy.SetValue(enemy.GetHealth());
            else
            {
                healthBarEnemy.SetMaxHealth(enemy.GetMaxHealth(), false);
                healthBarEnemy.SetValue(enemy.GetHealth());
            }
        }
    }
}
