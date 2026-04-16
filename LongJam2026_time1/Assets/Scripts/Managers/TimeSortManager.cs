using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSortManager : MonoBehaviour
{
    [Header("Manager Reference")]

    [SerializeField] private TeamSelectionManager teamManager;
    [SerializeField] private ListaPersonagens bancoDeDados;

    
    [Header("Canvas References")]

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject setUI;

    [SerializeField] private GameObject timeSortUI;

    [Header("Slots por Tribo")]
    [SerializeField] private List<FishDisplay> slotsMoluscos;  
    [SerializeField] private List<FishDisplay> slotsPeixes;    
    [SerializeField] private List<FishDisplay> slotsCrustaceos;


    public int membros = 0;
    // Start is called before the first frame update
    void Start()
    {
        SortearTimeDisponivel();
    }

    // Update is called once per frame
    void Update()
    {
        if(membros == 3)
        {
            EndRoulette();
        }
    }

    public void SortearTimeDisponivel()
    {
        // Sorteia para cada slot de acordo com a tribo
        PreencherSlots(slotsMoluscos, "Molusco");
        PreencherSlots(slotsPeixes, "Peixe");
        PreencherSlots(slotsCrustaceos, "Crustáceo");
    }

    private void PreencherSlots(List<FishDisplay> slots, string tribo)
    {
        foreach (FishDisplay slot in slots)
        {
            // Puxa um peixe aleatório da lista filtrada
            FishSO peixeSorteado = bancoDeDados.SortearBicho(tribo);
            
            if (peixeSorteado != null)
            {
                slot.Setup(peixeSorteado);
                
                if (slot.TryGetComponent<TeamOnClick>(out TeamOnClick clickScript))
                {
                    clickScript.myFishData = peixeSorteado;
                    clickScript.sortManager = this;
                }
            }
        }
    }

    public void ResetarSlots()
{
    // 1. Resetar o contador de membros selecionados
    membros = 0;

    // 2. Reativar todos os GameObjects dos slots
    ReativarLista(slotsMoluscos);
    ReativarLista(slotsPeixes);
    ReativarLista(slotsCrustaceos);

    // 3. Sortear novos peixes para os slots reativados
    SortearTimeDisponivel();
}

private void ReativarLista(List<FishDisplay> slots)
{
    foreach (FishDisplay slot in slots)
    {
        slot.gameObject.SetActive(true);
        // Opcional: Limpar os dados visuais antes do novo sorteio
        // slot.fishSprite.sprite = null; 
    }
}

    private void EndRoulette()
    {

        timeSortUI.SetActive(false);
        teamSelectionUI.SetActive(true);
        setUI.SetActive(true);
        teamManager.InitializeUI();
        

    }
}
