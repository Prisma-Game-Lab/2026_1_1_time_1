using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResonanceButton : MonoBehaviour
{
    [Header("References")]

    [SerializeField] public ResonanceManager resonanceManager;

    [SerializeField] public TextMeshProUGUI buttonText;
    [HideInInspector] public ResonanceSO currentResonance;

    public void ActivateResonance()
    {
        resonanceManager.SetPlayerResonance(currentResonance);
    }

    public void SetResonanceButton(ResonanceSO resonance)
    {
        currentResonance = resonance;
        buttonText.text = currentResonance.resonanceName;   
    }
    
}
