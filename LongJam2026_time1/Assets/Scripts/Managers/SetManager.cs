using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Winner
{
    Player,
    Enemy,
    Draw,

}
public class SetManager : MonoBehaviour
{

    [Header("Enemy Rivals")]

    [SerializeField] private List<EnemyRivalSO> enemyRivalSOs = new List<EnemyRivalSO>();

    [Header("UI References")]

    [Header("Corners")]


    [SerializeField] private TextMeshProUGUI playerCount;

    [SerializeField] private TextMeshProUGUI enemyCount;


    [Header("End Screen References")]

    [SerializeField] private GameObject EndScreen;

    [SerializeField] private TextMeshProUGUI winnerText;

    [SerializeField] private GameObject replayButton;

    [SerializeField] private GameObject nextRivalButton;


    [Header("Set Settings")]

    [SerializeField] private int maxRounds = 2;

    [SerializeField] private float rivalScreenTime = 3f;

    [SerializeField] private float fadeDuration = 1.0f;

    [Header("Manager References")]

    [SerializeField] private TeamSelectionManager teamManager;

    [SerializeField] private BattleManager battleManager;

    [SerializeField] private ResonanceManager resonanceManager;

    [SerializeField] private TimeSortManager sortManager;

    [SerializeField] private RivalScreen rivalManager;


    [Header("Canvas References")]

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject battleUI;

    [SerializeField] private GameObject cassinoUI;
    [SerializeField] private GameObject tribeRollete;
    [SerializeField] private GameObject gachaUI;

    [SerializeField] private GameObject setUI;

    [SerializeField] private GameObject teamSortUI;

    [SerializeField] private GameObject rivalUI;

    [SerializeField] private Animator fadeAnim;

    [HideInInspector] public Winner roundWinner;

    [HideInInspector] public Winner setWinner = Winner.Draw;

    public Roulette money;

    private int playerRounds = 0;

    private int enemyRounds = 0;

    private int currentRival = 0;

    private int lastRival;
    void Start()
    {
        lastRival = enemyRivalSOs.Count-1;

        battleManager.InitializeEnemyTeam(enemyRivalSOs[currentRival].firstTeam, enemyRivalSOs[currentRival].enemyResonance );
        StartSet();
    }
    public void StartSet()
    {
        EndScreen.SetActive(false);
        replayButton.SetActive(false);
        nextRivalButton.SetActive(false);
        rivalManager.SetUp(enemyRivalSOs[currentRival]);
        rivalUI.SetActive(true);
        teamSortUI.SetActive(false);
        playerCount.text = playerRounds.ToString() + "/" + maxRounds.ToString();
        enemyCount.text = enemyRounds.ToString() + "/" + maxRounds.ToString();
        setUI.SetActive(false);
        
        StartCoroutine(StartSetAfterRivalScreen());
    }

    private IEnumerator StartSetAfterRivalScreen()
    {
        yield return new WaitForSeconds(rivalScreenTime);
        fadeAnim.SetTrigger("Start");
        yield return new WaitForSeconds(fadeDuration);
        rivalUI.SetActive(false);
        teamManager.reserveTeam.Clear();
        teamManager.activeTeam.Clear();
        sortManager.membros = 0;
        sortManager.ResetarSlots();
        teamSortUI.SetActive(true);
        money.dinheiro = 1;
        setUI.SetActive(false);
        fadeAnim.SetTrigger("End");
    }
    public void LoadNextRival()
    {
        currentRival++; 
    
        
        playerRounds = 0;
        enemyRounds = 0;
        setWinner = Winner.Draw;

        
        battleManager.InitializeEnemyTeam(enemyRivalSOs[currentRival].firstTeam, enemyRivalSOs[currentRival].enemyResonance);

        
        StartSet(); 
    }
    public void EndRound()
    {
        if (roundWinner == Winner.Player)
        {
            playerRounds++;
            playerCount.text = playerRounds.ToString() + "/" + maxRounds.ToString();
            if (playerRounds == maxRounds)
            {
                setWinner = Winner.Player;
                EndSet();
            }
            else
            {
                battleManager.InitializeEnemyTeam(enemyRivalSOs[currentRival].secondTeam,enemyRivalSOs[currentRival].enemyResonance);
            }

        }

        if (roundWinner == Winner.Enemy)
        {
            enemyRounds++;
            enemyCount.text = enemyRounds.ToString() + "/" + maxRounds.ToString();
            if (enemyRounds == maxRounds)
            {
                setWinner = Winner.Enemy;
                EndSet();
            }

        }



       
    }

    public void EndSet()
    {
        
        battleUI.SetActive(false);
        if(setWinner == Winner.Player)
        {
            if(currentRival == lastRival)
            {
                winnerText.text = "Todos os Rivais Foram Derrotados!";
                winnerText.color = Color.green;
                replayButton.SetActive(true);
            }
            else
            {
                winnerText.text = "Você derrotou " + enemyRivalSOs[currentRival].rivalName + " !";
                winnerText.color = Color.green;
                nextRivalButton.SetActive(true);
            }
        }
        else if(setWinner == Winner.Enemy)
        {
            winnerText.text = "Inimigo Venceu!";
            winnerText.color = Color.red;
            replayButton.SetActive(true);
        }

        battleManager.winnerText.text = "";
        battleManager.StopAllCoroutines();
        EndScreen.SetActive(true);
    }
    public void RestartRound()
    {
        StartCoroutine(RestartRoundTransition());
    }

    private IEnumerator RestartRoundTransition()
    {
        fadeAnim.SetTrigger("Start");
        yield return new WaitForSeconds(fadeDuration);

        if(roundWinner != Winner.Draw)
        {
            cassinoUI.SetActive(true);
            tribeRollete.SetActive(true);
            gachaUI.SetActive(true);
            money.dinheiro = 1;
            battleUI.SetActive(false);
            setUI.SetActive(false);
        }
        else
        {
            battleUI.SetActive(false);
            setUI.SetActive(true);
            teamSelectionUI.SetActive(true);
        }

        fadeAnim.SetTrigger("End");
    }

    public void RestartSet()
    {
       GameManager.LoadSceneByName("MainMenu");
    }

    
}
