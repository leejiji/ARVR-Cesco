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

    [HideInInspector]public int clearStars = 0; // 클리어한 미션 수
    private float clearTime; // 클리어 한 시간

    public void Init()
    {
        mapText.text = data.stage_name;

        if (!DataManager.instance.gameData.stageTime.ContainsKey(data.id))
        {
            clearStars = 0;
            GetComponent<Button>().onClick.AddListener(() => mainManager.MapButton(data, clearStars));
            return;
        }
        
        clearTime = DataManager.instance.gameData.stageTime[data.id]; //클리어 타임 가져오기

        if (clearTime <= data.missoin_1 && clearTime > data.missoin_2)
        {
            clearStars = 1; //미션 1 클리어
        }
        else if (clearTime <= data.missoin_2 && clearTime > data.missoin_3)
        {
            clearStars = 2; //미션 2 클리어
        }
        else if (clearTime <= data.missoin_3 && clearTime > 0f)
        {
            clearStars = 3; //미션 3 클리어
        }
        else
        {
            clearStars = 0; //미션 클리어 못함
        }

        for (int i = 0; i < clearStars; i++)
        {
            starImages[i].sprite = starSprite;
        }

        GetComponent<Button>().onClick.AddListener(() => mainManager.MapButton(data, clearStars));

        return;
    }
}
