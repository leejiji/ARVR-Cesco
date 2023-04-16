using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Stage", menuName = "ScriptableObjects/Stage", order = 3)]
public class SO_Stage : ScriptableObject
{
    [SerializeField]
    private int ID;

    [SerializeField]
    private string Name;

    [SerializeField]
    private string Ex;

    [SerializeField]
    private int Reward; //처음 클리어시 보상

    //값의 초 이내 클리어시 별 1개 활성화
    [SerializeField]
    private float Mission_1;

    [SerializeField]
    private float Mission_2;

    [SerializeField]
    private float Mission_3;

    [SerializeField]
    private int Tool_1_ID;//"스테이지에 제한된 도구값이 있을 시 해당 도구만 사용 가능 Tool 시트의 Tool_ID 참조"
    [SerializeField]
    private int Tool_2_ID;//"스테이지에 제한된 도구값이 있을 시 해당 도구만 사용 가능 Tool 시트의 Tool_ID 참조"

    [SerializeField]
    private int BA_Amount; // "스테이지에 나오는 바퀴벌레 뭉치의 수 바퀴벌레는 HP를 1으로 고정함"

    [SerializeField]
    private int BAs_Amount; // "스테이지에 나오는 바퀴벌레 뭉치의 수 바퀴벌레 뭉치는 HP를 3으로 고정함"

    [SerializeField]
    private int Fly_Count; // 값의 수 만큼 바퀴벌레(개별)를 처치할 때마다 포그(날파리) 효과 발동 // 10000일때 : 없음

    [SerializeField]
    private int Caterpillar_Count; //값의 수 만큼 바퀴벌레(개별)을 처치할 때마다 송충이 효과 발동 // 10000일때 : 없음

    //Getter
    public int id => ID;
    public string stage_name => Name;
    public string ex => Ex;
    public float missoin_1 => Mission_1;
    public float missoin_2 => Mission_2;
    public float missoin_3 => Mission_3;
    public int reward => Reward;
    public int tool_1_id => Tool_1_ID;
    public int tool_2_id => Tool_2_ID;
    public int ba_amount => BA_Amount;
    public int bas_amount => BAs_Amount;
    public int fly_count => Fly_Count;
    public int caterpillar_count => Caterpillar_Count;
}