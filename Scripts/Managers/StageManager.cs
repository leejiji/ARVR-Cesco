using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour //스테이지 관리와 씬 관리 동시에
{
    public SO_Stage stageData;

    [SerializeField] private SO_Stage[] stageList;

    private void Awake()
    {
        var obj = FindObjectsOfType<StageManager>();
        if(obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StageSelect(int _id)
    {
        stageData = stageList[_id];

        DataManager.instance.SaveGameData();

        SceneManager.LoadScene("1_Game");
    }
    public void GotoMain()
    {
        SceneManager.LoadScene("0_Main");
    }
}
