using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSortManager : MonoBehaviour
{
    [Header("Manager Reference")]

    [SerializeField] private TeamSelectionManager teamManager;
    
    [Header("Canvas References")]

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject setUI;

    [SerializeField] private GameObject timeSortUI;


    public int membros = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(membros == 3)
        {
            EndRoulette();
        }
    }

    private void EndRoulette()
    {
        timeSortUI.SetActive(false);
        teamSelectionUI.SetActive(true);
        teamManager.InitializeUI();
        setUI.SetActive(true);

    }
}
