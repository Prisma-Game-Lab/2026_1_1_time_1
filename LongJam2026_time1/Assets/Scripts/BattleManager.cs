using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Teams Setup")]
    [SerializeField] private FishSO[] playerTeamSO;
    [SerializeField] private FishSO[] enemyTeamSO;

    private List<BattleFish> playerTeam = new List<BattleFish>();
    private List<BattleFish> enemyTeam = new List<BattleFish>();

    [Header("UI Slots")]
    [SerializeField] private FishDisplay[] playerSlots; 
    [SerializeField] private FishDisplay[] enemySlots;

    [Header("Settings")]
    [SerializeField] private float timeBetweenTurns = 1.2f;

    void Start()
    {
        InitializeBattle();
        StartCoroutine(BattleLoop());
    }

    void InitializeBattle()
    {
        
        playerTeam.Clear();
        enemyTeam.Clear();

        foreach (var so in playerTeamSO) 
            if (so != null) playerTeam.Add(new BattleFish(so));

        foreach (var so in enemyTeamSO) 
            if (so != null) enemyTeam.Add(new BattleFish(so));
        
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
        foreach (PossibleTargets targetType in attacker.data.targets)
        {
            if (opponentTeam.Count == 0) continue;

            int preferredIndex = (int)targetType - 1;
            int actualIndex = Mathf.Clamp(preferredIndex, 0, opponentTeam.Count - 1);

            // Capture the damage BEFORE the fish potentially dies or stats change
            int damageAmount = attacker.currentDamage; 
            opponentTeam[actualIndex].currentHealth -= damageAmount;
        
            FishDisplay[] targetSlots = isAttackerPlayer ? enemySlots : playerSlots;
        
            // Only trigger the visual if the damage is meaningful and the slot exists
            if (actualIndex < targetSlots.Length && targetSlots[actualIndex].gameObject.activeSelf)
            {
                StartCoroutine(targetSlots[actualIndex].PlayHit());
            
                if (damageAmount > 0) 
                {
                    StartCoroutine(targetSlots[actualIndex].ShowDamageText(damageAmount));
                }

                // Force the health number to update immediately so it matches the damage text
                targetSlots[actualIndex].fishHealth.text = opponentTeam[actualIndex].currentHealth.ToString();
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
        if (playerTeam.Count > 0) Debug.Log("PLAYER WINS!");
        else if (enemyTeam.Count > 0) Debug.Log("ENEMY WINS!");
        else Debug.Log("IT'S A DRAW!");
    }
}