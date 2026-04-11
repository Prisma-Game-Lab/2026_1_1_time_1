using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ResonanceManager : MonoBehaviour
{
    [Header("All Resonance References")]

    [SerializeField] private ResonanceSO[] allResonances;

    [HideInInspector] public List<ResonanceSO> activeResonances = new List<ResonanceSO>();

    [HideInInspector] public ResonanceSO currentPlayerResonance = null;

    [HideInInspector] public ResonanceSO currentEnemyResonance = null;

    [Header("UI References")]

    [SerializeField] public GameObject[] gmButtons;

    [SerializeField] public ResonanceButton[] resButtons;

    public void CheckAllResonances(List<FishSO> team)
    {
        activeResonances.Clear(); 

        bool isActive;
        foreach(var resonance in allResonances)
        {
            isActive = CheckResonance(team,resonance);
            if(isActive)
            {
                activeResonances.Add(resonance);
                print("RESSONANCE: " + resonance.resonanceName + " IS ACTIVE");
            }

        }
        if(!activeResonances.Contains(currentPlayerResonance))
            currentPlayerResonance = null;
        
        if(activeResonances.Count() > 0)
            currentPlayerResonance = activeResonances[0];
        SetButtons();
    }


    public bool CheckResonance(List<FishSO> team, ResonanceSO resonance)
    {
        List<FishSO> remainingFish = new List<FishSO>(resonance.fishConditions);
        List<FishTribes> remainingTribes = new List<FishTribes>(resonance.tribeConditions);

        int totalConditions = remainingFish.Count + remainingTribes.Count;
        int matched = 0;

        HashSet<int> usedFishIndices = new HashSet<int>();

        
        for (int c = remainingFish.Count - 1; c >= 0; c--)
        {
        for (int i = 0; i < team.Count; i++)
        {
            if (usedFishIndices.Contains(i)) continue;
            if (team[i] == remainingFish[c])
            {
                usedFishIndices.Add(i);
                remainingFish.RemoveAt(c);
                matched++;
                break;
            }
        }
        }

    
        for (int c = remainingTribes.Count - 1; c >= 0; c--)
        {
        for (int i = 0; i < team.Count; i++)
        {
            if (usedFishIndices.Contains(i)) continue;
            if (team[i].fishTribe == remainingTribes[c])
            {
                usedFishIndices.Add(i);
                remainingTribes.RemoveAt(c);
                matched++;
                break;
            }
        }
        }

    return matched == totalConditions;
    }
       public void ActivateResonance(ResonanceSO resonance, List<BattleFish> team)
    {
        resonance.ApplyResonance(team);
    }

    public void SetPlayerResonance(ResonanceSO resonance)
    {
        currentPlayerResonance = resonance;
    }

    public void SetEnemyResonance(ResonanceSO resonance)
    {
        currentEnemyResonance = resonance;
    }
    
    public void SetButtons()
    {
        foreach(var button in gmButtons)
        {
            button.SetActive(false);
        }

        int numberOfButtons = activeResonances.Count();

        for(int i=0; i<numberOfButtons; i++)
        {
            gmButtons[i].SetActive(true);
            resButtons[i].SetResonanceButton(activeResonances[i]);
        }
    }

}
