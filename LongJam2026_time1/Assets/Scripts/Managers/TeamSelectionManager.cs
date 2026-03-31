using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class TeamSelectionManager : MonoBehaviour
{   
    [Header ("UI Canvas references")]

    [SerializeField] public GameObject teamSelectionUI;

    [SerializeField] public GameObject battleUI;

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
        
        for (int i = 0; i < teamSlots.Length; i++)
        {
            if (i < activeTeam.Count && activeTeam[i] != null)
            {
            
                teamDisplays[i].gameObject.SetActive(true);
                teamDisplays[i].transform.SetParent(teamSlots[i].transform);
                teamDisplays[i].transform.localPosition = Vector3.zero; 
                teamDisplays[i].Setup(activeTeam[i]);
            }
            else
            {
                teamDisplays[i].gameObject.SetActive(false);
            }
        }
        ResonanceCheck();
    }

    public void InitializeReserveTeam()
    {
        for (int i = 0; i < reserveDisplays.Length; i++)
        {
            if (i < reserveTeam.Count && reserveTeam[i] != null)
            {
                reserveDisplays[i].gameObject.SetActive(true);
                reserveDisplays[i].transform.SetParent(reserveSlots[i].transform);
                reserveDisplays[i].transform.localPosition = Vector3.zero;
                reserveDisplays[i].Setup(reserveTeam[i]);
            }
            else
            {
                reserveDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    public void InitializeUI()
    {
        InitializePlayerTeam();
        InitializeReserveTeam();
    }
    
    public void SaveTeamLayout()
    {
        
        List<FishSO> newActive = new List<FishSO>();
        List<FishSO> newReserve = new List<FishSO>();

        
        foreach (FishSlot slot in teamSlots)
        {
            if (slot.transform.childCount > 0)
            {
                var display = slot.GetComponentInChildren<FishDisplay>();
                if (display != null && display.fishData != null) 
                    newActive.Add(display.fishData);
            }
        }

        foreach (FishSlot slot in reserveSlots)
        {
            if (slot.transform.childCount > 0)
            {
                var display = slot.GetComponentInChildren<FishDisplay>();
                if (display != null && display.fishData != null) 
                    newReserve.Add(display.fishData);
            }
        }

        if (newActive.Count > 0)
        {
            activeTeam = new List<FishSO>(newActive);
            reserveTeam = new List<FishSO>(newReserve);
        }
    
        ResonanceCheck();
    }
    public void RequestSave()
    {
        StopAllCoroutines();
        StartCoroutine(SaveAtEndOfFrame());
    }

    private IEnumerator SaveAtEndOfFrame()
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
        
        SaveTeamLayout();
        SetPlayerBattleTeam();
        battleUI.SetActive(true);
        battleManager.StartBattle();
        teamSelectionUI.SetActive(false);

    }

    public void ResonanceCheck()
    {
        
        
        if (activeTeam.Count < 3 || activeTeam[0] == null || activeTeam[1] == null || activeTeam[2] == null)
        {
            battleManager.playerResonance = CurrentResonance.None;
            print("Not enough fish for resonance!");
            return; 
        }

        
        FishTribes t0 = activeTeam[0].fishTribe;
        FishTribes t1 = activeTeam[1].fishTribe;
        FishTribes t2 = activeTeam[2].fishTribe;

        
        if (t0 == FishTribes.Peixe && t1 == FishTribes.Peixe && t2 == FishTribes.Peixe)
        {
            battleManager.playerResonance = CurrentResonance.Fish;
            print("FISH RESONANCE ACTIVATED");
        }
        else if (t0 == FishTribes.Molusco && t1 == FishTribes.Molusco && t2 == FishTribes.Molusco)
        {
            battleManager.playerResonance = CurrentResonance.Mollusk;
            print("MOLLUSK RESONANCE ACTIVATED");
        }
        else if (t0 == FishTribes.Crustáceo && t1 == FishTribes.Crustáceo && t2 == FishTribes.Crustáceo)
        {
            battleManager.playerResonance = CurrentResonance.Crustacean;
            print("CRUSTACEAN RESONANCE ACTIVATED");
        }
        
        else if (t0 != t1 && t0 != t2 && t1 != t2)
        {
            battleManager.playerResonance = CurrentResonance.Joker;
            print("JOKER RESONANCE ACTIVATED");
        }
        
        else 
        {
            battleManager.playerResonance = CurrentResonance.None;
            print("No Resonance Active :(");
        }
    }

    public void AddFish(FishSO fish)
    {
        if(activeTeam.Count < 3)
        {
            activeTeam.Add(fish);
            
        }
        else
        {
            reserveTeam.Add(fish);
            
        }
        InitializeUI();
    }
    
}