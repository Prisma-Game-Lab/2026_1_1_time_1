using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishDisplay : MonoBehaviour
{

    [Header("Settings")]

    [SerializeField] private float attackTextduration = 0.75f;

    [Header("UI References")]
    [SerializeField] public Image fishSprite;
    [SerializeField] public TextMeshProUGUI fishDamage;
    [SerializeField] public TextMeshProUGUI fishHealth;

    [Header("Effects")]
    [SerializeField] private TextMeshProUGUI damageText;

    private Vector3 originalPos;
    private bool isInitialized = false;

    [HideInInspector] public FishSO fishData;

    // Battle Setup

    public void Setup(BattleFish fish, bool isPlayer)
    {
        
        if (!isInitialized) {
            originalPos = transform.localPosition;
            isInitialized = true;
        }

        if (fish == null) return;

        fishData = fish.data;

        fishSprite.sprite = fish.data.fishSprite;
        fishDamage.text = fish.currentDamage.ToString();
        fishHealth.text = fish.currentHealth.ToString();
        damageText.text = "";

        // Change the text's when buffed
        fishDamage.color = (fish.currentDamage > fish.data.fishDamage) ? Color.green : Color.white;
        fishHealth.color = (fish.currentHealth > fish.data.fishMaxHealth) ? Color.green : Color.white;

        float xRotation = isPlayer ? 180f : 0f; 
        fishSprite.transform.localRotation = Quaternion.Euler(0, xRotation, 0);
    }


    // Team Selection Setup
    public void Setup(FishSO so)
    {
        InitializePosition();
        if (so == null) return;

        fishData = so; 

        fishSprite.sprite = so.fishSprite;
        fishDamage.text = so.fishDamage.ToString();
        fishHealth.text = so.fishMaxHealth.ToString();
        damageText.text = "";
        
        
        fishSprite.transform.localRotation = Quaternion.identity;
    }

    private void InitializePosition()
    {
        if (!isInitialized) {
            originalPos = transform.localPosition;
            isInitialized = true;
        }
    }

    public IEnumerator PlayAttack(bool isPlayer)
    {
        
        float lungeDistance = isPlayer ? 100f : -100f; 
        float windupDistance = isPlayer ? -20f : 20f;
        
        Vector3 windupPos = originalPos + new Vector3(windupDistance, 0, 0);
        Vector3 lungePos = originalPos + new Vector3(lungeDistance, 0, 0);

        // Wind-up 
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime * 10f; 
            transform.localPosition = Vector3.Lerp(originalPos, windupPos, t);
            yield return null;
        }

        // Lunge 
        t = 0;
        while (t < 1) {
            t += Time.deltaTime * 25f; 
            transform.localPosition = Vector3.Lerp(windupPos, lungePos, t);
            yield return null;
        }

       
        yield return new WaitForSeconds(0.1f);

        // Return to start
        t = 0;
        while (t < 1) {
            t += Time.deltaTime * 15f;
            transform.localPosition = Vector3.Lerp(lungePos, originalPos, t);
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public IEnumerator ShowDamageText(int amount, bool isCrit, float duration)
    {   
        damageText.text = isCrit ? $"-{amount}!!!" : $"-{amount}";
        damageText.color = Color.red;
        damageText.gameObject.SetActive(true);

        Vector3 startPos = damageText.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 50f, 0); 
        float elapsed = 0;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            damageText.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            damageText.alpha = Mathf.Lerp(1, 0, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        damageText.gameObject.SetActive(false);
        damageText.transform.localPosition = startPos; 
    }


    public IEnumerator PlayHit(float duration)
    {
        float elapsed = 0;
        while (elapsed < duration) 
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * 8f;
            // Small constant for rapid shaking frequency
            float shakeStep = 0.03f;
            elapsed += shakeStep;
            yield return new WaitForSeconds(shakeStep);
        }
        transform.localPosition = originalPos;
    }
}