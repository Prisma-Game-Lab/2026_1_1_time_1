using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FishTribes
{
    Peixe,
    Molusco,
    Crustáceo
}
public enum PossibleTargets
{
    front = 1,
    mid = 2,
    end = 3
}

[CreateAssetMenu (menuName = "ScriptableObjects/New Fish") ]
public class FishSO : ScriptableObject
{
    [Header("Fish Name")]
    public string fishName;
    [Header("Fish Tribe")]
    public FishTribes fishTribe;

    [Header("Fish Health")]

    public int fishMaxHealth;

    private int fishCurrentHealth;

    [Header("Fish Damage")]

    public int fishDamage;

    [Header("Fish Crit-Chance")]

    public float fishCritChance;

    [Header("Fish Target")]

    public PossibleTargets[] targets;

    [Header("Sprites")]

    public Sprite fishSprite;

    public Sprite auraSprite;
}