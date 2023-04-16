using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButtonObject : MonoBehaviour
{
    public SO_Stage data;
    public MainManager mainManager;

    [SerializeField] private Text mapText;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starSprite;   

    [HideInInspector]public int clearStars = 0; // Ŭ������ �̼� ��
    private float clearTime; // Ŭ���� �� �ð�

    public void Init()
    {
        mapText.text = data.stage_name;

        if (!DataManager.instance.gameData.stageTime.ContainsKey(data.id))
        {
            clearStars = 0;
            GetComponent<Button>().onClick.AddListener(() => mainManager.MapButton(data, clearStars));
            return;
        }
        
        clearTime = DataManager.instance.gameData.stageTime[data.id]; //Ŭ���� Ÿ�� ��������

        if (clearTime <= data.missoin_1 && clearTime > data.missoin_2)
        {
            clearStars = 1; //�̼� 1 Ŭ����
        }
        else if (clearTime <= data.missoin_2 && clearTime > data.missoin_3)
        {
            clearStars = 2; //�̼� 2 Ŭ����
        }
        else if (clearTime <= data.missoin_3 && clearTime > 0f)
        {
            clearStars = 3; //�̼� 3 Ŭ����
        }
        else
        {
            clearStars = 0; //�̼� Ŭ���� ����
        }

        for (int i = 0; i < clearStars; i++)
        {
            starImages[i].sprite = starSprite;
        }

        GetComponent<Button>().onClick.AddListener(() => mainManager.MapButton(data, clearStars));

        return;
    }
}
