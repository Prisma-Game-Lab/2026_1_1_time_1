using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class TeamSelectionManager : MonoBehaviour
{   
    [Header ("UI Canvas references")]

    [SerializeField] public GameObject teamSelectionUI;

    [SerializeField] public GameObject battleUI;

    [Header ("Managers")]

    [SerializeField] public BattleManager battleManager;

    [SerializeField] public ResonanceManager resonanceManager;

    [SerializeField] private Animator fadeAnim;

    [SerializeField] private float fadeDuration = 1.0f;

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

        resonanceManager.CheckAllResonances(activeTeam);
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
        resonanceManager.CheckAllResonances(activeTeam);
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
        StartCoroutine(ToBattleTransition());
    }

    private IEnumerator ToBattleTransition()
    {
        fadeAnim.SetTrigger("Start");
        yield return new WaitForSeconds(fadeDuration);
        SetPlayerBattleTeam();
        battleUI.SetActive(true);
        battleManager.StartBattle();
        teamSelectionUI.SetActive(false);
        fadeAnim.SetTrigger("End");
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