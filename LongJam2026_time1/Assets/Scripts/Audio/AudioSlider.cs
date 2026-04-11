using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;

    [SerializeField] private AudioSource AudioSource;

    [SerializeField] TMP_Text displayText;

    [SerializeField] Slider slider;

    [SerializeField] string subgroupVolume;

       private void Start()
    {
        SyncSlider();
      
    }

    private void SyncSlider()
    {
        if (Mixer.GetFloat(subgroupVolume, out float dB))
        {
            float linear = Mathf.Pow(10f, dB / 20f);
            slider.value = linear;

            if (slider.targetGraphic != null)
                UpdateDisplayText(slider.value);
        }
        else
        {
            Debug.LogWarning($"Could not get {subgroupVolume} value from AudioMixer");
        }
    }

    private void UpdateDisplayText(float value)
    {
        if (displayText != null)
        {
            displayText.text = value.ToString("N2");
        }
    }

    public void OnChangeSlider(float Value)
    {
        displayText.text = Value.ToString("N2");
        Mixer.SetFloat(subgroupVolume, Mathf.Log10(Value) * 20);
    }

    

}