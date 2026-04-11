using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResonanceSO : ScriptableObject
{

    [Header("Resonance Name")]
    public string resonanceName;
    

   [Header("Conditions")]

   [Header("Fish Conditions")]

    public List<FishSO> fishConditions = new List<FishSO>();

   [Header("Fish Tribe Conditions")]

   public List<FishTribes> tribeConditions = new List<FishTribes>();

   public abstract void ApplyResonance(List<BattleFish> team);

   public abstract List<BattleFish> GetParticipants(List<BattleFish> team);

    
}

