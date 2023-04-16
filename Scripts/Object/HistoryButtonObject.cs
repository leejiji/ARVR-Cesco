using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryButtonObject : MonoBehaviour
{
    public SO_Stage data;
    public MainManager mainManager;

    [SerializeField] private Text mapText;

    public void Init()
    {
        mapText.text = data.stage_name;

        GetComponent<Button>().onClick.AddListener(() => mainManager.HistoryButton(data));

        return;
    }
}
