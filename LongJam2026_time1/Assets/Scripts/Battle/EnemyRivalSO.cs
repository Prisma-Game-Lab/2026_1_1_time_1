using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "ScriptableObjects/New Rival") ]
public class EnemyRivalSO : ScriptableObject
{

    [Header("Rival Name")]
    public string rivalName;

    [Header("Flavor Text")]

    public string rivalFlavor;
    

   [Header("Rival's Teams")]

    public List<FishSO> firstTeam = new List<FishSO>();

    public List<FishSO> secondTeam = new List<FishSO>();


    [Header("Rival Sprite")]

    public Sprite rivalSprite;

    [Header("Rival Resonance")]

    public ResonanceSO enemyResonance;
}

