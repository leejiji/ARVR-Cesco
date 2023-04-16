using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text testText; //�׽�Ʈ �ؽ�Ʈ
    [SerializeField] private Text timeText; //�ð� �ؽ�Ʈ

    //==���ӿ��� ����
    [SerializeField] private Text overtimeText; //�ð� �ؽ�Ʈ
    [SerializeField] private Image[] starImage; //�� �̹���
    [SerializeField] private Sprite starSprite; //�� ��������Ʈ


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

        int clearTime = (int)GameManager.instance.gameTime; //Ŭ���� Ÿ�� ��������
        int clearStars = 0;

        if (clearTime <= GameManager.instance.stageData.missoin_1 && clearTime > GameManager.instance.stageData.missoin_2)
        {
            clearStars = 1; //�̼� 1 Ŭ����
        }
        else if (clearTime <= GameManager.instance.stageData.missoin_2 && clearTime > GameManager.instance.stageData.missoin_3)
        {
            clearStars = 2; //�̼� 2 Ŭ����
        }
        else if (clearTime <= GameManager.instance.stageData.missoin_3 && clearTime > 0f)
        {
            clearStars = 3; //�̼� 3 Ŭ����
        }
        else
        {
            clearStars = 0; //�̼� Ŭ���� ����
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
        FindObjectOfType<StageManager>().GotoMain(); //��������
    }
}
