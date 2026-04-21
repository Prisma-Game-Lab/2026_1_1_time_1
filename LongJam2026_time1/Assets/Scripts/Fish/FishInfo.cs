using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class FishInfo : MonoBehaviour
{
    [Header("Object References")]

    [SerializeField] private Image cardImage;

    [SerializeField] private Image tribeImage;

    [SerializeField] private Image positionImage;

    [SerializeField] private Image fishImage;

    [SerializeField] private TextMeshProUGUI atkValue;

    [SerializeField] private TextMeshProUGUI hpValue;

    [SerializeField] private TextMeshProUGUI critValue;

    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Sprite References")]

    [Header("Cards")]

    [SerializeField] private Sprite crustaceanCard;

    [SerializeField] private Sprite molluskCard;

    [SerializeField] private Sprite fishCard;


    [Header("Tribe Icons")]
    [SerializeField] private Sprite crustaceanIcon;

    [SerializeField] private Sprite molluskIcon;

    [SerializeField] private Sprite fishIcon;

    [Header("Attack Position Icons")]
    [SerializeField] private Sprite frontIcon;

    [SerializeField] private Sprite middleIcon;

    [SerializeField] private Sprite backIcon;


    public void SetUp(FishSO fish)
    {
        fishImage.sprite = fish.fishSprite;
        nameText.text = fish.fishName;
        atkValue.text = fish.fishDamage.ToString();
        hpValue.text = fish.fishMaxHealth.ToString();
        critValue.text = (fish.fishCritChance * 100).ToString() + "%";

        if(fish.targets[0] == PossibleTargets.front)
        {
           positionImage.sprite = frontIcon; 
        }
        else if(fish.targets[0] == PossibleTargets.mid)
        {
           positionImage.sprite = middleIcon; 
        }
        else if(fish.targets[0] == PossibleTargets.end)
        {
           positionImage.sprite = backIcon; 
        }


        if (fish.fishTribe == FishTribes.Crustáceo)
        {
            tribeImage.sprite = crustaceanIcon;
            cardImage.sprite = crustaceanCard;
        }
        else if (fish.fishTribe == FishTribes.Molusco)
        {
            tribeImage.sprite = molluskIcon;
            cardImage.sprite = molluskCard;
        }
        else if (fish.fishTribe == FishTribes.Peixe)
        {
            tribeImage.sprite = fishIcon;
            cardImage.sprite = fishCard;
        }
    }
}
