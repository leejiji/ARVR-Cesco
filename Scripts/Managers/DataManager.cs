using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField] private SO_Stage[] stageList;
    [SerializeField] private SO_Weapon[] toolList;
    [HideInInspector] public string FilenPath = "/data.json";
    [HideInInspector] public string GameDataFileName = "";

    private void Awake()
    {
        instance = this;
        GameDataFileName = Application.persistentDataPath + FilenPath;
    }

    [HideInInspector] public GameData _gameData;
    public GameData gameData
    {
        get
        {
            return _gameData;
        }
    }

    public void LoadSOData()
    {
        //SO �����Ͱ� �߰��Ǹ� ������Ʈ ���ִ� �뵵

        gameData.stages = stageList;
        gameData.weapons = toolList;

        for (int i = 0; i < gameData.stages.Length; i++)
        {
            if(gameData.stageTime.ContainsKey(200 + i) == false)
            {
                gameData.stageTime.Add(200 + i, -1f); //������ �������� �߰�
            }
        }
        for (int i = 0; i < gameData.weapons.Length; i++)
        {
            if (gameData.tools.ContainsKey(300 + i) == false)
            {
                gameData.tools.Add(300 + i, false); //������ �������� �߰�..
            }
            gameData.tools[301] = true;

        }
    }

    public void LoadGameData()
    {
        string filePath = GameDataFileName;

        if(File.Exists(filePath))
        {
            //�ҷ����� ���� ��

            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonConvert.DeserializeObject<GameData>(FromJsonData);

            LoadSOData();

            Debug.Log("�ҷ����� ���� : " + gameData.clearStage.ToString());
        }
        else
        {
            //�ҷ����� ���� �� �� ���� ����
            _gameData = new GameData();
            LoadSOData();

            Debug.Log("�ҷ����� ���� (�� ���� ����)");
        }
    }

    public void SaveGameData()
    {
        //����
        string ToJsonData = JsonConvert.SerializeObject(gameData);
        string filePath = GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }

    public void StageClear()
    {
        if(gameData.stageTime.ContainsKey(GameManager.instance.stageData.id)) //������ Ȯ��
        {
            if(gameData.stageTime[GameManager.instance.stageData.id] < 0f) //ó�� Ŭ������ �����������
            {
                gameData.clearStage += 1; //Ŭ���� �������� ���� �ø���
                gameData.price += GameManager.instance.stageData.reward; //�������� Ŭ���� ����
                gameData.stageTime[GameManager.instance.stageData.id] = GameManager.instance.gameTime; //���� Ÿ�� ������Ʈ
            }

            if(gameData.stageTime[GameManager.instance.stageData.id] > GameManager.instance.gameTime) //�ű���� ���
            {
                gameData.stageTime[GameManager.instance.stageData.id] = GameManager.instance.gameTime; //���� Ÿ�Ӹ� ������Ʈ
            }
        }
        else 
        {
            Debug.Log("�߸��� ����");
        }

        SaveGameData();
    }
}
