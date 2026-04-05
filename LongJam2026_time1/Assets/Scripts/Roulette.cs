using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    public float RotatePower;
    public float StopPower;
    public ListaPersonagens bancoDeDados;

    private Rigidbody2D rbody;
    int inRotate;

    public int dinheiro = 1;
    
    [Header("Manager Reference")]
    [SerializeField] private TeamSelectionManager teamManager;

    

    [Header ("UI References")]

    [SerializeField] public TextMeshProUGUI prizeText;
    [SerializeField] public TextMeshProUGUI moneyText;

    [Header("Canvas References")]

    [SerializeField] private GameObject cassinoEroletaUI;

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject battleUI;

    [SerializeField] private GameObject setUI;

    [SerializeField] private GameObject cassinoUI;

    [Header("Roulette Settings")]

    [SerializeField] private float endRouletteTimer = 5;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    float t;
    private void Update()
    {           
        moneyText.text = "Dinheiro: " + dinheiro;

        if (rbody.angularVelocity > 0)
        {
            rbody.angularVelocity -= StopPower*Time.deltaTime;

            rbody.angularVelocity =  Mathf.Clamp(rbody.angularVelocity, 0 , 1440);
        }

        if(rbody.angularVelocity == 0 && inRotate == 1) 
        {
            t +=1*Time.deltaTime;
            if(t >= 0.5f)
            {
                GetReward();

                inRotate = 0;
                t = 0;
            }
        }
    }


    public void Rotete() 
    {
        if(dinheiro > 0)
        {
            cassinoUI.SetActive(false);
            RotatePower = Random.Range(500, 1500);
            print(RotatePower);
            dinheiro -= 1;


            if(inRotate == 0)
            {
                rbody.AddTorque(RotatePower);
                inRotate = 1;
            }
        }
    }



    public void GetReward()
    {
        float rot = transform.eulerAngles.z;
        string triboSorteada = "";

        if (rot > 0 && rot <= 60)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,30);
            triboSorteada = "Crustáceo";
        }
        else if (rot > 60 && rot <= 120)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,90);
            triboSorteada = "Peixe";
        }
        else if (rot > 120 && rot <= 180)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,150);
            triboSorteada = "Molusco";
        }
        else if (rot > 180 && rot <= 240)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,210);
            triboSorteada = "Crustáceo";
        }
        else if (rot > 240 && rot <= 300)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,270);
            triboSorteada = "Peixe";
        }
        else if (rot > 300 && rot <= 360)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,330);
            triboSorteada = "Molusco";
        }

        Debug.Log(triboSorteada);
        cassinoUI.SetActive(true);
        
        if (bancoDeDados != null && triboSorteada != "")
        {   
            FishSO bichoGanhado = bancoDeDados.SortearBicho(triboSorteada);
        
            if (bichoGanhado != null)
            {
                prizeText.text = "VOCÊ GANHOU UM: " + bichoGanhado.fishName;
                teamManager.AddFish(bichoGanhado);
                Debug.Log("VOCÊ GANHOU UM: " + bichoGanhado.fishName);
                if(dinheiro <= 0)
                {
                    StartCoroutine(EndRoulette());
                }
            }
        }


    }

    private IEnumerator EndRoulette()
    {
        yield return new WaitForSeconds(endRouletteTimer);
        prizeText.text = "";
        cassinoEroletaUI.SetActive(false);
        teamSelectionUI.SetActive(true);
        teamManager.InitializeUI();
        setUI.SetActive(true);

    }


}
