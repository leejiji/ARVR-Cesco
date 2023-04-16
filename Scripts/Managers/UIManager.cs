using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text testText; //테스트 텍스트
    [SerializeField] private Text timeText; //시간 텍스트

    //==게임오버 관련
    [SerializeField] private Text overtimeText; //시간 텍스트
    [SerializeField] private Image[] starImage; //별 이미지
    [SerializeField] private Sprite starSprite; //별 스프라이트


    private void Update()
    {
        testText.text = "[Test]\n" + "State : " + GameManager.instance.state.ToString()
            + "\nPlanes : " + GameManager.instance.arPlanes.Count.ToString()
            + "\nCoin : " + GameManager.instance.nowCoin.ToString();

        timeText.text = ((int)GameManager.instance.gameTime / 60).ToString().PadLeft(2, '0') +
            " : " + ((int)GameManager.instance.gameTime % 60).ToString().PadLeft(2, '0');        
    }
    public void GameOverUI()
    {
        overtimeText.text = ((int)GameManager.instance.gameTime / 60).ToString().PadLeft(2, '0') +
            " : " + ((int)GameManager.instance.gameTime % 60).ToString().PadLeft(2, '0');

        int clearTime = (int)GameManager.instance.gameTime; //클리어 타임 가져오기
        int clearStars = 0;

        if (clearTime <= GameManager.instance.stageData.missoin_1 && clearTime > GameManager.instance.stageData.missoin_2)
        {
            clearStars = 1; //미션 1 클리어
        }
        else if (clearTime <= GameManager.instance.stageData.missoin_2 && clearTime > GameManager.instance.stageData.missoin_3)
        {
            clearStars = 2; //미션 2 클리어
        }
        else if (clearTime <= GameManager.instance.stageData.missoin_3 && clearTime > 0f)
        {
            clearStars = 3; //미션 3 클리어
        }
        else
        {
            clearStars = 0; //미션 클리어 못함
        }

        for (int i = 0; i < clearStars; i++)
        {
            starImage[i].sprite = starSprite;
        }
    }
    public void GameStartButton()
    {
        GameManager.instance.ExitState();
        GameManager.instance.EnterState(State.READY);
    }

    public void GamePauseButton()
    {
        GameManager.instance.ExitState();
    }
    public void GameRestartButton()
    {
        GameManager.instance.ExitState();
    }
    public void ToolChangeButton()
    {
        GameManager.instance.isToolChange = true;
        GameManager.instance.toolPanel.SetActive(true);
    }
    public void MainButton()
    {
        FindObjectOfType<StageManager>().GotoMain(); //메인으로
    }
}
