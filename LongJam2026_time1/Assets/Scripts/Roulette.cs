using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette : MonoBehaviour
{
    public float RotatePower;
    public float StopPower;
    public ListaPersonagens bancoDeDados;

    private Rigidbody2D rbody;
    int inRotate;

    public int money;
    
<<<<<<< Updated upstream
=======
    [Header("Manager Reference")]
    [SerializeField] private TeamSelectionManager teamManager;

    

    [Header ("UI References")]

    [SerializeField] public TextMeshProUGUI prizeText;

    [SerializeField] public TextMeshProUGUI moneyText;
    

    [Header("Canvas References")]

    [SerializeField] private GameObject cassinoUI;

    [SerializeField] private GameObject teamSelectionUI;

    [SerializeField] private GameObject battleUI;

    [SerializeField] private GameObject setUI;

    [Header("Roulette Settings")]

    [SerializeField] private float endRouletteTimer = 5;
>>>>>>> Stashed changes

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        moneyText.text = "Dinheiro: $" + money.ToString();
    }

    float t;
    private void Update()
    {           
        
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

        if(gameObject.activeSelf)
        {
            moneyText.text = "Dinheiro: $" + money.ToString();
        }
    }


    public void Rotete() 
    {
        RotatePower = Random.Range(500, 1500);
        print(RotatePower);

        if(inRotate == 0 && money > 0)
        {
            money -= 1;
            moneyText.text = "Dinheiro: $" + money.ToString();
            rbody.AddTorque(RotatePower);
            inRotate = 1;
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
        
        if (bancoDeDados != null && triboSorteada != "")
        {   
            FishSO bichoGanhado = bancoDeDados.SortearBicho(triboSorteada);
        
            if (bichoGanhado != null)
            {
<<<<<<< Updated upstream
=======
                prizeText.text = "VOCÊ GANHOU UM: " + bichoGanhado.fishName;
                teamManager.AddFish(bichoGanhado);
>>>>>>> Stashed changes
                Debug.Log("VOCÊ GANHOU UM: " + bichoGanhado.fishName);
            }

            if(money < 1)
            {
                StartCoroutine(EndRoulette());
            }
        }


    }


}
