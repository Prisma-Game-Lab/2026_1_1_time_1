using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Resonance/Combo Resonance")]
public class TwoComboResonanceSO : ResonanceSO
{
    [Header("Effect Settings")]
    


    [Header("Fish 1 Stat Increases")]

    public int fish1AtkIncrease;
    public int fish1HpIncrease;
    public float fish1CritIncrease;

    [Header("Fish 2 Stat Increases")]
    
    public int fish2AtkIncrease;
    public int fish2HpIncrease;
    public float fish2CritIncrease;


    public override void ApplyResonance(List<BattleFish> team)
    {
        foreach (var fish in team)
        {
            // Use .fishName comparison to be safe
            if (fish.data == fishConditions[0])
            {
                fish.currentCritChance += fish1CritIncrease;
                fish.currentDamage += fish1AtkIncrease;
                fish.currentHealth += fish1HpIncrease;
            }
            else if (fish.data == fishConditions[1])
            {
                fish.currentCritChance += fish2CritIncrease;
                fish.currentDamage += fish2AtkIncrease;
                fish.currentHealth += fish2HpIncrease;
            }
        }
    }


    public override List<BattleFish> GetParticipants(List<BattleFish> team)
    {
    List<BattleFish> participants = new List<BattleFish>();
    foreach (var fish in team)
    {
        if (fish.data == fishConditions[0] || fish.data == fishConditions[1])
            participants.Add(fish);
    }
    return participants;
    }
}
