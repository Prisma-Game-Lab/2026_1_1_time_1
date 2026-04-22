using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TutorialManager : MonoBehaviour, IPointerClickHandler
{
    [Header("Configurações de Persistência")]
    [SerializeField] private string tutorialKey = "tutorial_visto";

    private bool canInteract = false;

    void Awake()
    {
        // Se já foi marcado como visto antes, deleta antes de qualquer coisa
        if (PlayerPrefs.GetInt(tutorialKey, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Iniciamos uma Corotina para esperar o Unity estabilizar a UI
        canInteract = false;
        StartCoroutine(EnableInteractionDelay());
    }

    IEnumerator EnableInteractionDelay()
    {
        // Espera 2 frames para garantir que o OnDisable falso do Unity não dispare
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        canInteract = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canInteract) FinalizarTutorial();
    }

    void Update()
    {
        // Se clicar em qualquer lugar da tela após o delay
        if (canInteract && Input.GetMouseButtonDown(0))
        {
            FinalizarTutorial();
        }
    }

    void OnDisable()
    {
        // Se o objeto for desativado (ex: você mudou de aba na UI) 
        // e ele já era interativo, consideramos que o jogador viu.
        if (canInteract)
        {
            FinalizarTutorial();
        }
    }

    private void FinalizarTutorial()
    {
        // Evita chamadas duplas
        if (PlayerPrefs.GetInt(tutorialKey, 0) == 1) return;

        PlayerPrefs.SetInt(tutorialKey, 1);
        PlayerPrefs.Save();
        
        Debug.Log($"Sucesso: Tutorial '{tutorialKey}' salvo e removido.");
        Destroy(gameObject);
    }

    [ContextMenu("Resetar Este Tutorial")]
    public void Resetar() => PlayerPrefs.DeleteKey(tutorialKey);

}