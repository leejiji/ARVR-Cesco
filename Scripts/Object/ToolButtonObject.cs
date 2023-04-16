using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButtonObject : MonoBehaviour
{
    public SO_Weapon data;
    public MainManager mainManager;

    [SerializeField] private Image toolImage;
    [SerializeField] private Sprite[] toolSprites;

    public void Init()
    {
        toolImage.sprite = toolSprites[data.id - 300];

        GetComponent<Button>().onClick.AddListener(() => mainManager.ToolButton(data));
    }
}
