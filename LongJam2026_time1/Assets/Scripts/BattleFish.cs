using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFish
{
    public FishSO data;
    public int currentHealth;
    public int currentDamage;

    public BattleFish(FishSO so)
    {
        data = so;
        currentHealth = so.fishMaxHealth;
        currentDamage = so.fishDamage;
    }

    public bool IsDead => currentHealth <= 0;
}
