using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour
{
    public void Start()
    {
        Init();

        GetComponent<Toggle>().onValueChanged.AddListener(delegate
        {
            ModeChange();
        });
    }
    private void Init()
    {
        if (DataManager.instance.gameData.cuteMode == true)
        {
            GetComponent<Toggle>().isOn = true;
        }
        else
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

    void ModeChange()
    {
        if(GetComponent<Toggle>().isOn == true)
        {
            DataManager.instance.gameData.cuteMode = true;
        }
        else
        {
            DataManager.instance.gameData.cuteMode = false;
        }
    }
}
