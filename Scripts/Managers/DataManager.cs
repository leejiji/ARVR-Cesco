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
        //SO 데이터가 추가되면 업데이트 해주는 용도

        gameData.stages = stageList;
        gameData.weapons = toolList;

        for (int i = 0; i < gameData.stages.Length; i++)
        {
            if(gameData.stageTime.ContainsKey(200 + i) == false)
            {
                gameData.stageTime.Add(200 + i, -1f); //데이터 없을때만 추가
            }
        }
        for (int i = 0; i < gameData.weapons.Length; i++)
        {
            if (gameData.tools.ContainsKey(300 + i) == false)
            {
                gameData.tools.Add(300 + i, false); //데이터 없을때만 추가..
            }
            gameData.tools[301] = true;

        }
    }

    public void LoadGameData()
    {
        string filePath = GameDataFileName;

        if(File.Exists(filePath))
        {
            //불러오기 성공 시

            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonConvert.DeserializeObject<GameData>(FromJsonData);

            LoadSOData();

            Debug.Log("불러오기 성공 : " + gameData.clearStage.ToString());
        }
        else
        {
            //불러오기 실패 시 새 파일 생성
            _gameData = new GameData();
            LoadSOData();

            Debug.Log("불러오기 실패 (새 파일 생성)");
        }
    }

    public void SaveGameData()
    {
        //저장
        string ToJsonData = JsonConvert.SerializeObject(gameData);
        string filePath = GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }

    public void StageClear()
    {
        if(gameData.stageTime.ContainsKey(GameManager.instance.stageData.id)) //데이터 확인
        {
            if(gameData.stageTime[GameManager.instance.stageData.id] < 0f) //처음 클리어한 스테이지라면
            {
                gameData.clearStage += 1; //클리어 스테이지 개수 늘리기
                gameData.price += GameManager.instance.stageData.reward; //스테이지 클리어 보상
                gameData.stageTime[GameManager.instance.stageData.id] = GameManager.instance.gameTime; //게임 타임 업데이트
            }

            if(gameData.stageTime[GameManager.instance.stageData.id] > GameManager.instance.gameTime) //신기록일 경우
            {
                gameData.stageTime[GameManager.instance.stageData.id] = GameManager.instance.gameTime; //게임 타임만 업데이트
            }
        }
        else 
        {
            Debug.Log("잘못된 접근");
        }

        SaveGameData();
    }
}
