using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Resonance/Tribe Resonance")]
public class TribeResonanceSO : ResonanceSO
{
    [Header("Effect Settings")]
    public FishTribes targetTribe; 
    public int atkIncrease;
    public int hpIncrease;
    public float critIncrease;

    public override void ApplyResonance(List<BattleFish> team)
    {
        
        int tribeCount = 0;
        foreach (var fish in team)
        {
            if (fish.fishTribe == targetTribe) tribeCount++;
        }

        float multiplier = 1f;
        if (tribeCount == 3)
            multiplier = 2f;

        
        foreach (var fish in team)
        {
            if (fish.fishTribe == targetTribe)
            {
                fish.currentDamage += (int)(atkIncrease * multiplier);
                fish.currentHealth += (int)(hpIncrease * multiplier);
                fish.currentCritChance += critIncrease * multiplier;
            }
        }
    }

    public override List<BattleFish> GetParticipants(List<BattleFish> team)
    {
    List<BattleFish> participants = new List<BattleFish>();
    foreach (var fish in team)
    {
        if (fish.fishTribe == targetTribe)
            participants.Add(fish);
    }
    return participants;
    }

    }


