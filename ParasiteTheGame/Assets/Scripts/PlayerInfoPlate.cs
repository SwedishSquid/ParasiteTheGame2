using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfoPlate : MonoBehaviour, IBossfightListener
{
    [SerializeField] private HealthBar playerHealthBar;
    [SerializeField] private HealthBar enemyHealthBar;
    [SerializeField] private HealthBar bossHealthBar;
    private AEnemy boss;

    public void AwakeData(PlayerController player)
    {
        playerHealthBar.SetMaxHealth(player.GetMaxHealth(), false);
    }

    public void UpdateData(PlayerController player)
    {
        UpdatePlayerHealthBar(player);
        UpdateEnemyHealthBar(player);
        UpdateBossHealthBar();
    }

    private void UpdatePlayerHealthBar(PlayerController player)
    {
        playerHealthBar.SetValue(player.GetHealth());
    }
    
    private void UpdateEnemyHealthBar(PlayerController player)
    {
        if (player.controlled is null && enemyHealthBar.gameObject.activeSelf)
            enemyHealthBar.gameObject.SetActive(false);
        else if (player.controlled is AEnemy enemy)
        {
            if (enemyHealthBar.gameObject.activeSelf)
                enemyHealthBar.SetValue(enemy.GetHealth());
            else
            {
                enemyHealthBar.SetMaxHealth(enemy.GetMaxHealth(), false);
                enemyHealthBar.SetValue(enemy.GetHealth());
            }
        }
    }

    private void UpdateBossHealthBar()
    {
        if (boss is null)
            return;
        bossHealthBar.SetValue(BossfightController.Instance.GetBoss().Health);
    }

    public void OnBossfightStart()
    {
        boss = BossfightController.Instance.GetBoss();
        bossHealthBar.SetMaxHealth(boss.MaxHealth, false);
        bossHealthBar.gameObject.SetActive(true);
    }

    public void OnLoadDuringBossfight() {}

    public void OnBossfightEnd()
    {
        boss = null;
        bossHealthBar.gameObject.SetActive(false);
    }

    public void OnLoadAfterBossfight() {}
}
