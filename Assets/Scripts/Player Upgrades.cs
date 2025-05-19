using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;




    public void UpgradeMaxHp()
    {
        player.maxHealthPoint += 20;
    }

    public void UpgradeMovementSpeed()
    {
        player.speed += 0.5f;
    }
    public void UpgradeAttackSpeed()
    {
        player.attackSpeed -= 0.2f;
    }
    public void UpgradeAttackDamage()
    {
        player.damage += 5;
    }
    
    public void CurrentHP()
    {
        player.currentHealthPoint += 25;
        if (player.currentHealthPoint >= player.maxHealthPoint)
        {
            player.currentHealthPoint = player.maxHealthPoint;
        }
    }
}
