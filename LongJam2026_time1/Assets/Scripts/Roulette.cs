using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class Roulette : MonoBehaviour
{
    public float RotatePower;
    public float StopPower;
    public ListaPersonagens bancoDeDados;

    private Rigidbody2D rbody;
    int inRotate;
    private GameObject currentSpinning;

    public int dinheiro = 1;

    [Header("Manager Reference")]
    [SerializeField] private TeamSelectionManager teamManager;



    [Header ("UI References")]

    [SerializeField] public TextMeshProUGUI prizeText;
    //[SerializeField] public TextMeshProUGUI moneyText;

    [SerializeField] public GameObject prize;

    [SerializeField] public GameObject tribeRoulette;

    [SerializeField] public GameObject fishRoulette;
    [SerializeField] public GameObject gmTribeRoulette;

    [SerializeField] public GameObject gmFishRoulette;

    [Header("UI References")]

    [SerializeField] public SpriteRenderer[] rouletteSprites;


    [Header("Canvas References")]

    [SerializeField] private GameObject cassinoEroletaUI;

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject battleUI;

    [SerializeField] private GameObject setUI;

    [SerializeField] private GameObject cassinoUI;

    [Header("Roulette Settings")]

    [SerializeField] private float endRouletteTimer = 5;

    [SerializeField] private Animator fadeAnim;

    [SerializeField] private float fadeDuration = 1.0f;

    private FishTribes rewardTribe;

    private List<FishSO> rewardList = new List<FishSO>();

    private FishSO rewardFish;

    float t;
    private void Update()
    {
        //moneyText.text = "Dinheiro: " + dinheiro;

        if (rbody != null && rbody.angularVelocity > 0)
        {
            rbody.angularVelocity -= StopPower*Time.deltaTime;

            rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
        }

        if (rbody != null && rbody.angularVelocity == 0 && inRotate == 1)
        {
            t +=1*Time.deltaTime;
            if(t >= 0.5f)
            {
                // Reset state before calling handlers so SpinFishWheel() inside GetTribe() can set inRotate = 1
                inRotate = 0;
                t = 0;

                if (currentSpinning == tribeRoulette)
                    GetTribe();
                else if (currentSpinning == fishRoulette)
                    GetFish();
            }
        }
    }

    // Parameterless wrapper for Unity UI button — spins the tribe roulette
    public void RoteteTribe()
    {
        tribeRoulette.SetActive(true);
        Rotete(tribeRoulette);
    }

    public void Rotete(GameObject target)
    {
        if(dinheiro > 0)
        {
            cassinoUI.SetActive(false);
            RotatePower = Random.Range(500, 1500);
            print(RotatePower);
            dinheiro -= 1;

            if(inRotate == 0)
            {
                currentSpinning = target;
                rbody = target.GetComponent<Rigidbody2D>();
                rbody.AddTorque(RotatePower);
                inRotate = 1;
            }
        }
    }

    private void SpinFishWheel()
    {
        SetImages();
        RotatePower = Random.Range(500, 1500);
        currentSpinning = fishRoulette;
        rbody = fishRoulette.GetComponent<Rigidbody2D>();
        rbody.AddTorque(RotatePower);
        inRotate = 1;
    }


    public void GetTribe()
    {
        float rot = tribeRoulette.transform.eulerAngles.z;
        string triboSorteada = "";

        if (rot > 0 && rot <= 60)
        {
            tribeRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,30);
            triboSorteada = "Crustáceo";
            rewardTribe = FishTribes.Crustáceo;
            rewardList = bancoDeDados.allCrustacean;
        }
        else if (rot > 60 && rot <= 120)
        {
            tribeRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,90);
            triboSorteada = "Peixe";
            rewardTribe = FishTribes.Peixe;
            rewardList = bancoDeDados.allFish;
        }
        else if (rot > 120 && rot <= 180)
        {
            tribeRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,150);
            triboSorteada = "Molusco";
            rewardTribe = FishTribes.Molusco;
            rewardList = bancoDeDados.allMollusks;
        }
        else if (rot > 180 && rot <= 240)
        {
            tribeRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,210);
            triboSorteada = "Crustáceo";
            rewardTribe = FishTribes.Crustáceo;
            rewardList = bancoDeDados.allCrustacean;
        }
        else if (rot > 240 && rot <= 300)
        {
            tribeRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,270);
            triboSorteada = "Peixe";
            rewardTribe = FishTribes.Peixe;
            rewardList = bancoDeDados.allFish;
        }
        else if (rot > 300 && rot <= 360)
        {
            tribeRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,330);
            triboSorteada = "Molusco";
            rewardTribe = FishTribes.Molusco;
            rewardList = bancoDeDados.allMollusks;
        }

        Debug.Log(triboSorteada);
        gmTribeRoulette.SetActive(false);
        gmFishRoulette.SetActive(true);

        SpinFishWheel();
    }

    public void SetImages()
    {
        if(rewardTribe == FishTribes.Peixe)
        {
            for(int i=0; i < 5; i++)
            {
                rouletteSprites[i].sprite = bancoDeDados.allFish[i].fishSprite;
            }
        }

        else if(rewardTribe == FishTribes.Molusco)
        {
            for(int i=0; i < 5; i++)
            {
                rouletteSprites[i].sprite = bancoDeDados.allMollusks[i].fishSprite;
            }
        }

        else if(rewardTribe == FishTribes.Crustáceo)
        {
            for(int i=0; i < 5; i++)
            {
                rouletteSprites[i].sprite = bancoDeDados.allCrustacean[i].fishSprite;
            }
        }


    }

    public void GetFish()
    {
        if(bancoDeDados == null) return;

        float rot = fishRoulette.transform.eulerAngles.z;

        if (rot > 0 && rot <= 72)
        {
            fishRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,36);
            rewardFish = rewardList[0];

        }
        else if (rot > 72 && rot <= 144)
        {
            fishRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,108);
            rewardFish = rewardList[1];

        }
        else if (rot > 144 && rot <= 216)
        {
            fishRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,180);
            rewardFish = rewardList[2];

        }
        else if (rot > 216 && rot <= 288)
        {
            fishRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,252);
            rewardFish = rewardList[3];

        }
        else if (rot > 288 && rot <= 360)
        {
            fishRoulette.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,324);
            rewardFish = rewardList[4];

        }

        prize.SetActive(true);
        prizeText.text = "VOCÊ GANHOU UM: " + rewardFish.fishName;
        
        AudioManager.Instance?.PlaySFX("rewardSFX");
        teamManager.AddFish(rewardFish);
        StartCoroutine(EndRoulette());

    }
    
    private IEnumerator EndRoulette()
    {
        yield return new WaitForSeconds(endRouletteTimer);

        prize.SetActive(false);
        prizeText.text = "";
        fadeAnim.SetTrigger("Start");
        yield return new WaitForSeconds(fadeDuration);
        gmTribeRoulette.SetActive(true);
        gmFishRoulette.SetActive(false);
        cassinoEroletaUI.SetActive(false);
        teamSelectionUI.SetActive(true);
        teamManager.InitializeUI();
        setUI.SetActive(true);
        fadeAnim.SetTrigger("End");
    }


}
