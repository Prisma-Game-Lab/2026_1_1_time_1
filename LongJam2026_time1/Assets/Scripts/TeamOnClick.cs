using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TeamOnClick : MonoBehaviour, IPointerClickHandler
{

    [Header("Manager Reference")]
    [SerializeField] private TeamSelectionManager teamManager;
    public TimeSortManager sortManager;

    public FishSO myFishData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (myFishData == null) return;

        sortManager.membros += 1;
        Debug.Log("Peixe selecionado: " + myFishData.fishName);

        this.gameObject.SetActive(false);
        teamManager.AddFish(myFishData);

    }

}
