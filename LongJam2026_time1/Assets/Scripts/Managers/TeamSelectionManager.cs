using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamSelectionManager : MonoBehaviour
{   
    [Header ("UI Canvas references")]

    [SerializeField] public GameObject teamSelectionUI;

    [Header ("Battle Manager")]

    [SerializeField] public BattleManager battleManager;

    [Header ("Fish Displays")]

    [SerializeField] public FishDisplay[] teamDisplays;

    [SerializeField] public FishDisplay[] reserveDisplays;
    
    [Header("Slots")]
    [SerializeField] private FishSlot[] teamSlots;    // Max 3
    [SerializeField] private FishSlot[] reserveSlots; // Currently 3

    [Header("Current Data")]
    public List<FishSO> activeTeam = new List<FishSO>();
    public List<FishSO> reserveTeam = new List<FishSO>();

    void Start()
    {
    
    InitializeUI();

    }

    public void InitializePlayerTeam()
    {
        for (int i=0;i<3;i++)
        {
            teamDisplays[i].Setup(activeTeam[i]);
        }

        ResonanceCheck();
    }

    public void InitializeReserveTeam()
    {
        for (int i=0;i<3;i++)
        {
            reserveDisplays[i].Setup(reserveTeam[i]);
        }
    }

    public void InitializeUI()
    {
        InitializePlayerTeam();
        InitializeReserveTeam();
    }
    
    public void SaveTeamLayout()
    {
        activeTeam.Clear();
        reserveTeam.Clear();

        // Scan Team Slots
        foreach (FishSlot slot in teamSlots)
        {
            if (slot.transform.childCount > 0)
            {
                // Grab the data from the FishDisplay component sitting on the child
                FishSO fishData = slot.transform.GetChild(0).GetComponent<FishDisplay>().fishData;
                activeTeam.Add(fishData);
            }
        }

        // Scan Reserve Slots
        foreach (FishSlot slot in reserveSlots)
        {
            if (slot.transform.childCount > 0)
            {
                FishSO fishData = slot.transform.GetChild(0).GetComponent<FishDisplay>().fishData;
                reserveTeam.Add(fishData);
            }
        }
        ResonanceCheck();
        Debug.Log("Team Saved! Active count: " + activeTeam.Count);
    }
    public void RequestSave()
    {
        StopAllCoroutines();
        StartCoroutine(SaveAtEndOfFrame());
    }

    private System.Collections.IEnumerator SaveAtEndOfFrame()
    {
        // Wait until the hierarchy is physically updated
        yield return new WaitForEndOfFrame();
        SaveTeamLayout();
    }

    public void SetPlayerBattleTeam()
    {
        battleManager.InitializePlayerTeam(activeTeam);
    }

    public void ToBattle()
    {
        
        
        SetPlayerBattleTeam();
        battleManager.StartBattle();
        teamSelectionUI.SetActive(false);

    }

    public void ResonanceCheck()
    {
        if(activeTeam[0].fishTribe == FishTribes.Peixe 
        && activeTeam[1].fishTribe == FishTribes.Peixe 
        && activeTeam[2].fishTribe == FishTribes.Peixe)
        {
            battleManager.currentResonance = CurrentResonance.Fish;
            print("FISH RESONANCE ACTIVATED");
            
        }
        else if(activeTeam[0].fishTribe == FishTribes.Molusco 
        && activeTeam[1].fishTribe == FishTribes.Molusco 
        && activeTeam[2].fishTribe == FishTribes.Molusco)
        {
            battleManager.currentResonance = CurrentResonance.Mollusk;
            print("MOLLUSK RESONANCE ACTIVATED");
            
        }
        else if(activeTeam[0].fishTribe == FishTribes.Crustáceo
        && activeTeam[1].fishTribe == FishTribes.Crustáceo 
        && activeTeam[2].fishTribe == FishTribes.Crustáceo)
        {
            battleManager.currentResonance = CurrentResonance.Crustacean;
            print("CRUSTACEAN RESONANCE ACTIVATED");
            
        }
        else if(activeTeam[0].fishTribe != activeTeam[1].fishTribe
        && activeTeam[0].fishTribe != activeTeam[2].fishTribe 
        && activeTeam[1].fishTribe != activeTeam[2].fishTribe)
        {
            battleManager.currentResonance = CurrentResonance.Joker;
            print("JOKER RESONANCE ACTIVATED");
            
        }

        else 
        {
            battleManager.currentResonance = CurrentResonance.None;
            print("No Resonance Active :(");
        }
        
    }
    
}