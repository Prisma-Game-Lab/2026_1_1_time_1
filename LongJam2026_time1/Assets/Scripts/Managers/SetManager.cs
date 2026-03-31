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

    [SerializeField] private TextMeshProUGUI playerCount;

    [SerializeField] private TextMeshProUGUI enemyCount;


    [Header("End Screen References")]

    [SerializeField] private GameObject EndScreen;

    [SerializeField] private TextMeshProUGUI winnerText;

    [SerializeField] private GameObject replayButton;

    [SerializeField] private GameObject nextRivalButton;


    [Header("Set Settings")]

    [SerializeField] private int maxRounds = 2;

    [Header("Manager References")]

    [SerializeField] private TeamSelectionManager teamManager;

    [SerializeField] private BattleManager battleManager;

    [Header("Canvas References")]

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject battleUI;

    [SerializeField] private GameObject cassinoUI;

    [SerializeField] private GameObject setUI;

    [HideInInspector] public Winner roundWinner;

    [HideInInspector] public Winner setWinner = Winner.Draw;

    private int playerRounds = 0;

    private int enemyRounds = 0;

    private int currentRival = 0;

    private int lastRival;
    void Start()
    {
        lastRival = enemyRivalSOs.Count-1;
        battleManager.InitializeEnemyTeam(enemyRivalSOs[currentRival].firstTeam);
        StartSet();
    }
    public void StartSet()
    {
        EndScreen.SetActive(false);
        replayButton.SetActive(false);
        nextRivalButton.SetActive(false);

        teamManager.reserveTeam.Clear();

        cassinoUI.SetActive(true);
        setUI.SetActive(false);
        
        playerCount.text = playerRounds.ToString() + "/" + maxRounds.ToString();
        enemyCount.text = enemyRounds.ToString() + "/" + maxRounds.ToString();
        
    }
    public void LoadNextRival()
    {
        currentRival++; 
    
        
        playerRounds = 0;
        enemyRounds = 0;
        setWinner = Winner.Draw;

        
        battleManager.InitializeEnemyTeam(enemyRivalSOs[currentRival].firstTeam);

        
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
                battleManager.InitializeEnemyTeam(enemyRivalSOs[currentRival].secondTeam);
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

        battleManager.StopAllCoroutines();
        EndScreen.SetActive(true);
    }
    public void RestartRound()
    {
        cassinoUI.SetActive(true);
        battleUI.SetActive(false);
        setUI.SetActive(false);
        
    }

    public void RestartSet()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
