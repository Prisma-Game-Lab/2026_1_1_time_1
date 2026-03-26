using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics.Tracing;


public enum CurrentResonance
    {
        Fish,
        Crustacean,
        Mollusk,
        Joker,

        None
    }
public class BattleManager : MonoBehaviour
{
    [Header("Manager References")]

    [SerializeField] private SetManager setManager;

    [Header("UI References")]
    
    [SerializeField] private TextMeshProUGUI winnerText;

    [Header("Teams Setup")]
    [SerializeField] private List<FishSO> playerTeamSO = new List<FishSO>();
    [SerializeField] private List<FishSO> enemyTeamSO = new List<FishSO>();

    private List<BattleFish> playerTeam = new List<BattleFish>();
    private List<BattleFish> enemyTeam = new List<BattleFish>();

    [Header("UI Slots")]
    [SerializeField] private FishDisplay[] playerSlots; 
    [SerializeField] private FishDisplay[] enemySlots;


    [Header("Resonance Settings")]

    [SerializeField] private int crustaceanHPIncrease = 2;

    [SerializeField] private int fishAtkIncrease = 2;

    [SerializeField] private float molluskCritIncrease = 0.2f;

    [SerializeField] private int jokerHPIncrease = 1;

    [SerializeField] private int jokerAtkIncrease = 1;

    [SerializeField] private float jokerCritIncrease = 0.1f;



    [Header("Settings")]
    [SerializeField] private float timeBetweenTurns = 1.2f;

    [SerializeField] private float critMultiplier = 2.0f;

    [HideInInspector] public CurrentResonance playerResonance = CurrentResonance.None;

    [HideInInspector] public CurrentResonance enemyResonance = CurrentResonance.None;
    

    public void StartBattle()
    {
        InitializeBattle();
        StartCoroutine(BattleLoop());
    }

    void InitializeBattle()
    {
        
        enemyTeam.Clear();

        foreach (var so in playerTeamSO) 
            if (so != null) playerTeam.Add(new BattleFish(so));
        
        foreach (var so in enemyTeamSO) 
            if (so != null) enemyTeam.Add(new BattleFish(so));
        
        EnemyResonanceCheck();
        ActivateResonance();
        UpdateUI();
    }

    IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(1f); 

        while (playerTeam.Count > 0 && enemyTeam.Count > 0)
        {
            BattleFish pAttacker = playerTeam[0];
            BattleFish eAttacker = enemyTeam[0];

            
            StartCoroutine(playerSlots[0].PlayAttack(true));
            StartCoroutine(enemySlots[0].PlayAttack(false));

            
            yield return new WaitForSeconds(0.25f); 

            
            ExecuteAttack(pAttacker, enemyTeam, true);
            ExecuteAttack(eAttacker, playerTeam, false);

            
            yield return new WaitForSeconds(0.6f); 

            CleanupDeadFish();
            UpdateUI();

            yield return new WaitForSeconds(0.4f);
        }
        DetermineWinner();
}

    void ExecuteAttack(BattleFish attacker, List<BattleFish> opponentTeam, bool isAttackerPlayer)
    {
        if (opponentTeam.Count == 0) return;

        // 1. Calculate Damage once per "Attack Instance"
        int damageAmount = attacker.currentDamage;
        float random = Random.value;
        bool isCrit = false;

        // 2. Check for Crit once for the whole attack
        if (random <= attacker.currentCritChance)
        {
            damageAmount = (int)(damageAmount * critMultiplier);
            isCrit = true;
        }

        // 3. Track which fish we have already hit this turn
        HashSet<BattleFish> hitTargets = new HashSet<BattleFish>();

        foreach (PossibleTargets targetType in attacker.data.targets)
        {
            if (opponentTeam.Count == 0) continue;

            int preferredIndex = (int)targetType - 1;
            int actualIndex = Mathf.Clamp(preferredIndex, 0, opponentTeam.Count - 1);
        
            BattleFish targetFish = opponentTeam[actualIndex];

           
            if (!hitTargets.Contains(targetFish))
            {
                targetFish.currentHealth -= damageAmount;
                hitTargets.Add(targetFish); 

                // Visuals
                FishDisplay[] targetSlots = isAttackerPlayer ? enemySlots : playerSlots;
                if (actualIndex < targetSlots.Length && targetSlots[actualIndex].gameObject.activeSelf)
                {
                    StartCoroutine(targetSlots[actualIndex].PlayHit());
                
                    if (damageAmount > 0) 
                    {
                        StartCoroutine(targetSlots[actualIndex].ShowDamageText(damageAmount, isCrit));
                    }
                
                    targetSlots[actualIndex].fishHealth.text = targetFish.currentHealth.ToString();
                }
            }
        }
    }

    void CleanupDeadFish()
    {
        playerTeam.RemoveAll(f => f.IsDead);
        enemyTeam.RemoveAll(f => f.IsDead);
    }

    void UpdateUI()
    {
        
        for (int i = 0; i < 3; i++) 
        {
            if (i < playerSlots.Length) playerSlots[i].gameObject.SetActive(false);
            if (i < enemySlots.Length) enemySlots[i].gameObject.SetActive(false);
        }

       
        for (int i = 0; i < playerTeam.Count; i++) 
        {
            if (i < playerSlots.Length)
            {
                playerSlots[i].gameObject.SetActive(true);
                playerSlots[i].Setup(playerTeam[i], true); 
            }
        }
        
        
        for (int i = 0; i < enemyTeam.Count; i++) 
        {
            if (i < enemySlots.Length)
            {
                enemySlots[i].gameObject.SetActive(true);
                enemySlots[i].Setup(enemyTeam[i], false);
            }
        }
    }

    void DetermineWinner()
    {
        if (playerTeam.Count > 0)
        {
            Debug.Log("JOGADOR GANHOU!");
            winnerText.text = "JOGADOR GANHOU!";
            setManager.roundWinner = Winner.Player;
        }
         
        else if (enemyTeam.Count > 0)
        {
            Debug.Log("INIMIGO GANHOU!");
            winnerText.text = "INIMIGO GANHOU!";
            setManager.roundWinner = Winner.Enemy;
        } 
        else
        {
            Debug.Log("EMPATOU!");
            winnerText.text = "EMPATOU!";
            setManager.roundWinner = Winner.Draw;
        }

        setManager.EndRound();
        if(setManager.setWinner == Winner.Draw)
        StartCoroutine(EndBattle());
        
    }

    private IEnumerator EndBattle()
    {
        yield  return new WaitForSeconds(5);
        winnerText.text = "";
        setManager.RestartRound();
        
        
    }

    public void RestartBattle()
    {
        
        StopAllCoroutines(); 

       
        playerTeam.Clear();
        enemyTeam.Clear();

        
        foreach (var so in playerTeamSO) 
            if (so != null) playerTeam.Add(new BattleFish(so));

        foreach (var so in enemyTeamSO) 
            if (so != null) enemyTeam.Add(new BattleFish(so));

       
        ActivateResonance();
        UpdateUI();

        
        StartCoroutine(BattleLoop());
    }
    public void InitializePlayerTeam(List<FishSO> team)
    {
        playerTeamSO.Clear(); 
        playerTeam.Clear();
        foreach (var so in team) 
            if (so != null) playerTeamSO.Add(so);

        UpdateUI();
    }

    public void InitializeEnemyTeam(List<FishSO> team)
    {
        enemyTeamSO.Clear();
        enemyTeam.Clear();
        foreach (var so in team) 
            if (so != null) enemyTeamSO.Add(so);

        UpdateUI();
    }

    public void ActivatePlayerResonance()
    {
        if (playerResonance == CurrentResonance.None)
            return;
        
        if (playerResonance == CurrentResonance.Fish)
        {
            foreach (var fish in playerTeam)
            {
                fish.currentDamage += fishAtkIncrease;
            }
        }

        if (playerResonance == CurrentResonance.Crustacean)
        {
            foreach (var crustacean in playerTeam)
            {
                crustacean.currentHealth += crustaceanHPIncrease;
            }
        }

        if (playerResonance == CurrentResonance.Mollusk)
        {
            foreach (var mollusk in playerTeam)
            {
                mollusk.currentCritChance += molluskCritIncrease;
            }
        }

        if (playerResonance == CurrentResonance.Joker)
        {
            foreach (var fish in playerTeam)
            {
                fish.currentDamage += jokerAtkIncrease;
                fish.currentHealth += jokerHPIncrease;
                fish.currentCritChance += jokerCritIncrease;
            }
        }
    }

    public void EnemyResonanceCheck()
    {
        
        
        if (enemyTeam.Count < 3 || enemyTeam[0] == null || enemyTeam[1] == null || enemyTeam[2] == null)
        {
            enemyResonance = CurrentResonance.None;
            return; 
        }

        
        FishTribes t0 = enemyTeamSO[0].fishTribe;
        FishTribes t1 = enemyTeamSO[1].fishTribe;
        FishTribes t2 = enemyTeamSO[2].fishTribe;

        
        if (t0 == FishTribes.Peixe && t1 == FishTribes.Peixe && t2 == FishTribes.Peixe)
        {
           enemyResonance = CurrentResonance.Fish;
            
        }
        else if (t0 == FishTribes.Molusco && t1 == FishTribes.Molusco && t2 == FishTribes.Molusco)
        {
           enemyResonance = CurrentResonance.Mollusk;
            
        }
        else if (t0 == FishTribes.Crustáceo && t1 == FishTribes.Crustáceo && t2 == FishTribes.Crustáceo)
        {
            enemyResonance = CurrentResonance.Crustacean;
            
        }
        
        else if (t0 != t1 && t0 != t2 && t1 != t2)
        {
            enemyResonance = CurrentResonance.Joker;
            
        }
        
        else 
        {
            enemyResonance = CurrentResonance.None;
           
        }
    }
    public void ActivateEnemyResonance()
    {
        if (enemyResonance == CurrentResonance.None)
            return;
        
        if (enemyResonance == CurrentResonance.Fish)
        {
            foreach (var fish in enemyTeam)
            {
                fish.currentDamage += fishAtkIncrease;
            }
        }

        if (enemyResonance == CurrentResonance.Crustacean)
        {
            foreach (var crustacean in enemyTeam)
            {
                crustacean.currentHealth += crustaceanHPIncrease;
            }
        }

        if (enemyResonance == CurrentResonance.Mollusk)
        {
            foreach (var mollusk in enemyTeam)
            {
                mollusk.currentCritChance += molluskCritIncrease;
            }
        }

        if (enemyResonance == CurrentResonance.Joker)
        {
            foreach (var fish in enemyTeam)
            {
                fish.currentDamage += jokerAtkIncrease;
                fish.currentHealth += jokerHPIncrease;
                fish.currentCritChance += jokerCritIncrease;
            }
        }
    }

    public void ActivateResonance()
    {
        ActivateEnemyResonance();
        ActivatePlayerResonance();
    }
    
}