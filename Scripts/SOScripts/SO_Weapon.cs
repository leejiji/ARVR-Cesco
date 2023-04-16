using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class SO_Weapon : ScriptableObject
{
    [SerializeField]
    private int ID;

    [SerializeField]
    private string Name;

    [SerializeField]
    private string Ex;

    [SerializeField]
    private int Price;

    [SerializeField]
    private int Range;

    [SerializeField]
    private float Power;

    [SerializeField]
    private bool isGun;

    //Getter
    public int id => ID;
    public string tool_Name => Name;
    public string ex => Ex;
    public int price => Price;
    public int range => Range;
    public float power => Power;
    public bool isgun => isGun;
}