using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

public enum BattleTime
{
    Normal,
    Paused,
    FastForwarded,
    Slowed,

}

public class BattleManager : MonoBehaviour
{
    [Header("Manager References")]

    [SerializeField] private SetManager setManager;

    [SerializeField] private ResonanceManager resonanceManager;

    [Header("UI References")]
    
    [SerializeField] public TextMeshProUGUI winnerText;

    [SerializeField] public GameObject winnerGm;

    //[Header("Teams Setup")]
    private List<FishSO> playerTeamSO = new List<FishSO>();
    private List<FishSO> enemyTeamSO = new List<FishSO>();

    private List<BattleFish> playerTeam = new List<BattleFish>();
    private List<BattleFish> enemyTeam = new List<BattleFish>();

    [Header("UI Slots")]
    [SerializeField] private FishDisplay[] playerSlots; 
    [SerializeField] private FishDisplay[] enemySlots;

    [Header("Time Buttons")]

    [SerializeField] private Button pauseButton;

    [SerializeField] private Button fastForwardButton;

    [SerializeField] private Button slowDownButton;

    [Header("Time Buttons' Alternate Sprites")]

    [SerializeField] private Sprite pressedPause;

    [SerializeField] private Sprite pressedForward;

    [SerializeField] private Sprite pressedSlow;

    [Header("Settings")]
    [SerializeField] private float timeBetweenTurns = 1.2f;
    [SerializeField] private float timeAfterDeathCleanup = 0.4f;
    [SerializeField] private float critMultiplier = 2.0f;

    [Header("Animation Timings")]
    
    [SerializeField] private float delayBeforeDamage = 0.25f; 

    
    [SerializeField] private float delayAfterDamage = 0.6f;

    
    [SerializeField] private float damageTextDuration = 0.75f;

    
    [SerializeField] private float hitShakeDuration = 0.18f;
    

    private ResonanceSO pendingEnemyResonance;

    private BattleTime currentTime = BattleTime.Normal;

    
    private float baseTimeBetweenTurns;
    private float baseTimeAfterDeathCleanup;
    
    private float baseDelayBeforeDamage; 

    private float baseDelayAfterDamage;

    private float baseDamageTextDuration;

    private float baseHitShakeDuration;

    private Sprite unpressedPause;

    private Sprite unpressedSlow;

    private Sprite unpressedForward;

    public void Start()
    {
        baseTimeBetweenTurns = timeBetweenTurns;
        baseTimeAfterDeathCleanup = timeAfterDeathCleanup;

        baseDelayBeforeDamage = delayBeforeDamage; 
        baseDelayAfterDamage = delayAfterDamage;

        baseDamageTextDuration = damageTextDuration;
        baseHitShakeDuration = hitShakeDuration;

        unpressedPause = pauseButton.image.sprite;

        unpressedSlow = slowDownButton.image.sprite;

        unpressedForward = fastForwardButton.image.sprite;
    }
    public void StartBattle()
    {
        StopAllCoroutines();
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
        

        AudioManager.Instance?.PlayLoopingSFX("auraSFX");
        ActivateResonance();
        UpdateUI();
        ApplyAuras();
    }

    IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(timeBetweenTurns); 

        while (playerTeam.Count > 0 && enemyTeam.Count > 0)
        {
            BattleFish pAttacker = playerTeam[0];
            BattleFish eAttacker = enemyTeam[0];

            StartCoroutine(playerSlots[0].PlayAttack(true));
            StartCoroutine(enemySlots[0].PlayAttack(false));

            
            yield return new WaitForSeconds(delayBeforeDamage); 

            bool pCrit = ExecuteAttack(pAttacker, enemyTeam, true);
            bool eCrit = ExecuteAttack(eAttacker, playerTeam, false);
            AudioManager.Instance?.PlaySFX((pCrit || eCrit) ? "critSFX" : "hitSFX");

            
            yield return new WaitForSeconds(delayAfterDamage); 

            CleanupDeadFish();
            UpdateUI();
            ApplyAuras();

            yield return new WaitForSeconds(timeAfterDeathCleanup);
        }
        DetermineWinner();
    }

    private void UpdateAttackVisuals(int index, int damage, bool crit, bool isPlayerAttacking, int newHealth)
    {
    FishDisplay[] targetSlots = isPlayerAttacking ? enemySlots : playerSlots;
    if (index < targetSlots.Length && targetSlots[index].gameObject.activeSelf)
    {
        // Pass the timing settings down to the display component
        StartCoroutine(targetSlots[index].PlayHit(hitShakeDuration));
        
        if (damage > 0) 
        {
            StartCoroutine(targetSlots[index].ShowDamageText(damage, crit, damageTextDuration));
        }
        
        targetSlots[index].fishHealth.text = newHealth.ToString();
    }
    }

    bool ExecuteAttack(BattleFish attacker, List<BattleFish> opponentTeam, bool isAttackerPlayer)
    {
        if (opponentTeam.Count == 0) return false;

        // 1. Calculate Damage and Crit once for the entire attack instance
        int damageAmount = attacker.currentDamage;
        bool isCrit = Random.value <= attacker.currentCritChance;
        if (isCrit) damageAmount = (int)(damageAmount * critMultiplier);

        HashSet<BattleFish> hitTargets = new HashSet<BattleFish>();

        // 2. We iterate through every intended target in the ScriptableObject
        foreach (PossibleTargets targetType in attacker.data.targets)
        {
            if (hitTargets.Count >= opponentTeam.Count) break; // No unique targets left to hit

            int preferredIndex = (int)targetType - 1;
        
            // 3. FIND THE TARGET:
            // If the preferred index is alive and not hit, take it.
            // Otherwise, find the closest available fish that hasn't been hit.
            BattleFish targetFish = GetValidTarget(preferredIndex, opponentTeam, hitTargets);

            if (targetFish != null)
            {
                targetFish.currentHealth -= damageAmount;
                hitTargets.Add(targetFish);

                // Find the actual index of the fish we just decided to hit to update the correct slot
                int visualIndex = opponentTeam.IndexOf(targetFish);
                UpdateAttackVisuals(visualIndex, damageAmount, isCrit, isAttackerPlayer, targetFish.currentHealth);
            }
        }
        return isCrit;
    }

    // Helper method to find the "Smart" target
    private BattleFish GetValidTarget(int preferredIndex, List<BattleFish> team, HashSet<BattleFish> alreadyHit)
    {
        // Try the intended slot first
        if (preferredIndex < team.Count && !alreadyHit.Contains(team[preferredIndex]))
        {
            return team[preferredIndex];
        }

        // If intended slot is invalid/already hit, grab the nearest available fish (from front to back)
        for (int i = 0; i < team.Count; i++)
        {
            if (!alreadyHit.Contains(team[i]))
            {
                return team[i];
            }
        }
        return null;
    }

    void CleanupDeadFish()
    {
        bool anyDied = playerTeam.Any(f => f.IsDead) || enemyTeam.Any(f => f.IsDead);

        playerTeam.RemoveAll(f => f.IsDead);
        enemyTeam.RemoveAll(f => f.IsDead);

        if (anyDied)
        {
            AudioManager.Instance?.PlaySFX("deathSFX");
        }
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
        winnerGm.SetActive(true);
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

        AudioManager.Instance?.StopLoopingSFX();
        setManager.EndRound();
        if(setManager.setWinner == Winner.Draw)
        {
            StartCoroutine(EndBattle());
        }
        
    }

    private IEnumerator EndBattle()
    {
        yield  return new WaitForSeconds(3);
        winnerGm.SetActive(false);
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

    public void InitializeEnemyTeam(List<FishSO> team, ResonanceSO resonance)
    {
        enemyTeamSO.Clear();
        enemyTeam.Clear();
        foreach (var so in team) 
            if (so != null) enemyTeamSO.Add(so);

       pendingEnemyResonance = resonance;
        UpdateUI();
    }
    
    public void ActivatePlayerResonance()
    {
        if(resonanceManager.currentPlayerResonance != null)
            resonanceManager.ActivateResonance(resonanceManager.currentPlayerResonance,playerTeam);
    }


    public void ActivateEnemyResonance()
    {
        
        if (pendingEnemyResonance != null)
            resonanceManager.ActivateResonance(pendingEnemyResonance, enemyTeam);
    }

    public void ActivateResonance()
    {
        ActivateEnemyResonance();
        ActivatePlayerResonance();
    }

    private void ApplyAuras()
    {
    
        foreach (var slot in playerSlots) slot.SetAura(false);
        foreach (var slot in enemySlots) slot.SetAura(false);

    
        if (resonanceManager.currentPlayerResonance != null)
        {
            var participants = resonanceManager.currentPlayerResonance.GetParticipants(playerTeam);
            foreach (var fish in participants)
            {
                int idx = playerTeam.IndexOf(fish);
                if (idx >= 0 && idx < playerSlots.Length)
                    playerSlots[idx].SetAura(true);
            }
        }

    
        if (pendingEnemyResonance != null)
        {
            var participants = pendingEnemyResonance.GetParticipants(enemyTeam);
            foreach (var fish in participants)
            {
                int idx = enemyTeam.IndexOf(fish);
                if (idx >= 0 && idx < enemySlots.Length)
                    enemySlots[idx].SetAura(true);
            }
        }
    }

    public void SetButtonColor(Button btn, Color color)
    {
        btn.GetComponent<Image>().color = color;
    }
    public void ToggleNormalTime()
    {
        Time.timeScale = 1;

        currentTime = BattleTime.Normal;
        timeBetweenTurns = baseTimeBetweenTurns;
        timeAfterDeathCleanup = baseTimeAfterDeathCleanup;

        delayBeforeDamage = baseDelayBeforeDamage; 
        delayAfterDamage = baseDelayAfterDamage;

        damageTextDuration = baseDamageTextDuration;
        hitShakeDuration = baseHitShakeDuration; 
    }

    public void TogglePause()
    {
        if(currentTime == BattleTime.Paused)
        {
            Time.timeScale = 1;
            //SetButtonColor(pauseButton, Color.white);
            pauseButton.image.sprite = unpressedPause;
            currentTime = BattleTime.Normal;
            AudioManager.Instance?.PlayLoopingSFX("auraSFX");
        }
        else
        {
            ToggleNormalTime();
            //SetButtonColor(pauseButton, Color.green);
            pauseButton.image.sprite = pressedPause;
            //SetButtonColor(fastForwardButton, Color.white);
            fastForwardButton.image.sprite = unpressedForward;
            SetButtonColor(slowDownButton, Color.white);
            AudioManager.Instance?.StopLoopingSFX();

            currentTime = BattleTime.Paused;
            Time.timeScale = 0;
        }
    }

    public void ToggleSlowDown()
    {
        if(currentTime == BattleTime.Slowed)
        {
            SetButtonColor(slowDownButton, Color.white);
            ToggleNormalTime();
        }

        else
        {
        
        
        //SetButtonColor(pauseButton, Color.white);
        pauseButton.image.sprite = unpressedPause;
        //SetButtonColor(fastForwardButton, Color.white);
        fastForwardButton.image.sprite = unpressedForward;
        SetButtonColor(slowDownButton, Color.green);
        ToggleNormalTime();

        currentTime = BattleTime.Slowed;

        timeBetweenTurns *= 2;
        timeAfterDeathCleanup *= 2;

        delayBeforeDamage *= 2; 
        delayAfterDamage *= 2;

        damageTextDuration *= 2;
        hitShakeDuration *= 2;

        }
    }
    
    public void ToggleFastForward()
    {
       if(currentTime == BattleTime.FastForwarded)
        {
            //SetButtonColor(fastForwardButton, Color.white);
            fastForwardButton.image.sprite = unpressedForward;
            ToggleNormalTime();
        }

        else
        {
        
        
        //SetButtonColor(pauseButton, Color.white);
        pauseButton.image.sprite = unpressedPause;
        SetButtonColor(slowDownButton, Color.white);
        //SetButtonColor(fastForwardButton, Color.green);
        fastForwardButton.image.sprite = pressedForward;
        ToggleNormalTime();

        currentTime = BattleTime.FastForwarded;

        timeBetweenTurns /= 2;
        timeAfterDeathCleanup /= 2;

        delayBeforeDamage /= 2; 
        delayAfterDamage /= 2;

        damageTextDuration /= 2;
        hitShakeDuration /= 2;

        }
        
 
    }
}