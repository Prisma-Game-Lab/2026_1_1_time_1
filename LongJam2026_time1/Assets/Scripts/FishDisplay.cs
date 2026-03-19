using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public Image fishSprite;
    [SerializeField] public TextMeshProUGUI fishDamage;
    [SerializeField] public TextMeshProUGUI fishHealth;

    [Header("Effects")]
    [SerializeField] private TextMeshProUGUI damageText;

    private Vector3 originalPos;
    private bool isInitialized = false;

    public void Setup(BattleFish fish, bool isPlayer)
    {
        
        if (!isInitialized) {
            originalPos = transform.localPosition;
            isInitialized = true;
        }

        if (fish == null) return;

        fishSprite.sprite = fish.data.fishSprite;
        fishDamage.text = fish.currentDamage.ToString();
        fishHealth.text = fish.currentHealth.ToString();

        float xRotation = isPlayer ? 180f : 0f; 
        fishSprite.transform.localRotation = Quaternion.Euler(0, xRotation, 0);
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

    public IEnumerator ShowDamageText(int amount)
    {
        damageText.text = $"-{amount}";
        damageText.color = Color.red;
        damageText.gameObject.SetActive(true);

        Vector3 startPos = damageText.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 50f, 0); 
        float elapsed = 0;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            damageText.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
            // Fade out
            damageText.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        damageText.gameObject.SetActive(false);
        damageText.transform.localPosition = startPos; // Reset for next time
    }
    
    public IEnumerator PlayHit()
    {
        
        for (int i = 0; i < 6; i++) {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * 8f;
            yield return new WaitForSeconds(0.03f);
        }
        transform.localPosition = originalPos;
    }
}