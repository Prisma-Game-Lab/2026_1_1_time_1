using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Winner
{
    Player,
    Enemy,
    Draw,

}
public class SetManager : MonoBehaviour
{

    [Header("UI References")]

    [SerializeField] private TextMeshProUGUI playerCount;

    [SerializeField] private TextMeshProUGUI enemyCount;

    [Header("End Screen References")]

    [SerializeField] private GameObject EndScreen;

    [SerializeField] private TextMeshProUGUI winnerText;


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

    void Start()
    {
        playerCount.text = playerRounds.ToString() + "/" + maxRounds.ToString();
        enemyCount.text = enemyRounds.ToString() + "/" + maxRounds.ToString();
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
            winnerText.text = "Jogador Venceu!";
            winnerText.color = Color.green;
        }
        else if(setWinner == Winner.Enemy)
        {
            winnerText.text = "Inimigo Venceu!";
            winnerText.color = Color.red;
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
