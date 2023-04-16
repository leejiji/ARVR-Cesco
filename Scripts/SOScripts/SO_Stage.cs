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
    private int Reward; //ó�� Ŭ����� ����

    //���� �� �̳� Ŭ����� �� 1�� Ȱ��ȭ
    [SerializeField]
    private float Mission_1;

    [SerializeField]
    private float Mission_2;

    [SerializeField]
    private float Mission_3;

    [SerializeField]
    private int Tool_1_ID;//"���������� ���ѵ� �������� ���� �� �ش� ������ ��� ���� Tool ��Ʈ�� Tool_ID ����"
    [SerializeField]
    private int Tool_2_ID;//"���������� ���ѵ� �������� ���� �� �ش� ������ ��� ���� Tool ��Ʈ�� Tool_ID ����"

    [SerializeField]
    private int BA_Amount; // "���������� ������ �������� ��ġ�� �� ���������� HP�� 1���� ������"

    [SerializeField]
    private int BAs_Amount; // "���������� ������ �������� ��ġ�� �� �������� ��ġ�� HP�� 3���� ������"

    [SerializeField]
    private int Fly_Count; // ���� �� ��ŭ ��������(����)�� óġ�� ������ ����(���ĸ�) ȿ�� �ߵ� // 10000�϶� : ����

    [SerializeField]
    private int Caterpillar_Count; //���� �� ��ŭ ��������(����)�� óġ�� ������ ������ ȿ�� �ߵ� // 10000�϶� : ����

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