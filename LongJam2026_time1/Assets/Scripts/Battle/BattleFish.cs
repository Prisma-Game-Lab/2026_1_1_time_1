using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFish
{
    public FishSO data;

    public int currentHealth;
    public int currentDamage;

    public float currentCritChance;

    public FishTribes fishTribe;

    public BattleFish(FishSO so)
    {
        data = so;
        currentHealth = so.fishMaxHealth;
        currentDamage = so.fishDamage;
        currentCritChance = so.fishCritChance;
        fishTribe = so.fishTribe;
    }

    public bool IsDead => currentHealth <= 0;
}
