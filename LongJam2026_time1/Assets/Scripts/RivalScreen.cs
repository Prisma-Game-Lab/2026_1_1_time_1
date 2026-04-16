using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RivalScreen : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI rivalName;

    [SerializeField] public TextMeshProUGUI flavorText;

    [SerializeField] public Image rivalSprite;


    public void SetUp(EnemyRivalSO rival)
    {
        rivalName.text = rival.rivalName + " É SEU PRÓXIMO OPONENTE!";
        flavorText.text = rival.rivalFlavor;
        rivalSprite.sprite = rival.rivalSprite;
    }
}
