using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelectButtonObject : MonoBehaviour
{
    public SO_Weapon data;

    [SerializeField] private Image toolImage;
    [SerializeField] private Sprite[] toolSprites;

    public void Init()
    {
        toolImage.sprite = toolSprites[data.id - 300];

        GetComponent<Button>().onClick.AddListener(() => SelectButton());
    }

    public void SelectButton()
    {
        //무기 오브젝트 변경
        GameManager.instance.nowWeaponObject[GameManager.instance.nowTool - 300].SetActive(false);
        GameManager.instance.nowTool = data.id;
        GameManager.instance.nowWeaponObject[GameManager.instance.nowTool - 300].SetActive(true);

        GameManager.instance.toolPanel.SetActive(false);
        GameManager.instance.isToolChange = false;
    }
}
