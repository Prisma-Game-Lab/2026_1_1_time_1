using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
[Header("References")]

[SerializeField] private GameObject options;

[SerializeField] private GameObject mainMenu;

private bool optionsToggle = false;

private bool menuToggle = true;

    public void ToggleOptions()
    {
        optionsToggle = !optionsToggle;
        menuToggle = !menuToggle;
        options.SetActive(optionsToggle);
        mainMenu.SetActive(menuToggle);
        
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        GameManager.LoadSceneByName("MainScene");
    }
   
}
