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
    

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
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
    }


    public void Rotete() 
    {
        RotatePower = Random.Range(500, 1500);
        print(RotatePower);

        if(inRotate == 0)
        {
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
                Debug.Log("VOCÊ GANHOU UM: " + bichoGanhado.fishName);
            }
        }


    }


}
