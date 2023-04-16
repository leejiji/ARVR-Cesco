using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Cockrach", menuName = "ScriptableObjects/Cockrach", order = 1)]
public class SO_Cockrach : ScriptableObject
{
    [SerializeField]
    private int ID;

    [SerializeField]
    private float HP;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private int Coin;

    //Getter
    public int id => ID;
    public float hp => HP;
    public float speed => Speed;
    public int coin => Coin;
}
