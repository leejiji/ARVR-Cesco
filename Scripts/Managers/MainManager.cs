using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    //==�� �г� ĵ���� �׷�==//
    [SerializeField] private CanvasGroup[] panelList; //0 = Main, 1=Quest, 2=Tools, 3=History, 4=Setting
    [SerializeField] private StageManager StageManager;
    [SerializeField] private Text coinText;

    //==�� ��ư==//
    [Header("�� ��ư ����"), Space(10)]
    [SerializeField] private GameObject mapContents;
    [SerializeField] private GameObject mapButton;

    //==�� �˾�==//
    [Header("�� �˾� ����"), Space(10)]
    [SerializeField] private GameObject mapPopupPanel;
    [SerializeField] private Text stageNameText;
    [SerializeField] private Text stageExText;
    [SerializeField] private Text[] missionText;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private GameObject mapToolPanel;
    [SerializeField] private Image[] toolImages;
    [SerializeField] private Sprite[] toolSprites;

    //==������ ��==//
    private int selectMapID;

    //==���� �˾�==//
    [Header("���� ��ư ����"), Space(10)]
    [SerializeField] private GameObject toolContents;
    [SerializeField] private GameObject toolButton;

    [Header("���� �˾� ����"), Space(10)]
    [SerializeField] private GameObject toolPopupPanel;
    [SerializeField] private Text toolNameText;
    [SerializeField] private Text toolExText;
    [SerializeField] private Text toolButtonText;
    [SerializeField] private Text toolPriceText;
    [SerializeField] private Image toolButtonImage;
    [SerializeField] private Image toolImage;

    //==�����丮 ��ư==//
    [Header("�����丮 ��ư ����"), Space(10)]
    [SerializeField] private GameObject historyContents;
    [SerializeField] private GameObject historyButton;

    //==�����丮 �˾�==//
    [Header("�����丮 �˾� ����"), Space(10)]
    [SerializeField] private GameObject historyPopupPanel;
    [SerializeField] private Text history_stageNameText;
    [SerializeField] private Text history_stageExText;
    [SerializeField] private Text[] history_missionText;
    [SerializeField] private GameObject history_mapToolPanel;
    [SerializeField] private Image[] history_toolImages;

    //==������ ����==//
    private SO_Weapon selectTool;

    private void Start()
    {
        DataManager.instance.LoadGameData();
        DataManager.instance.SaveGameData();

        CoinUpdate(); //���� UI������Ʈ

        for (int j = 0; j < DataManager.instance.gameData.clearStage; j++) //�� ��ư �ҷ�����
        {
            int i = j + 1;
            if (DataManager.instance.gameData.stages[i].missoin_3 >= DataManager.instance.gameData.stageTime[i + 200]
            && DataManager.instance.gameData.stageTime[i + 200] > 0f) //�̹� Ŭ������ ���������� ��
            {
                    //�����丮 ��ư ����
                    GameObject historyBtn = Instantiate(historyButton, Vector3.zero, Quaternion.identity);
                    historyBtn.transform.parent = historyContents.transform;
                    historyBtn.GetComponent<HistoryButtonObject>().mainManager = GetComponent<MainManager>();
                    historyBtn.GetComponent<HistoryButtonObject>().data = DataManager.instance.gameData.stages[i];
                    historyBtn.GetComponent<HistoryButtonObject>().Init();
            }
            else
            {
                //�� ��ư ����
                GameObject mapBtn = Instantiate(mapButton, Vector3.zero, Quaternion.identity);
                mapBtn.transform.parent = mapContents.transform;
                mapBtn.GetComponent<MapButtonObject>().mainManager = GetComponent<MainManager>();
                mapBtn.GetComponent<MapButtonObject>().data = DataManager.instance.gameData.stages[i];
                mapBtn.GetComponent<MapButtonObject>().Init();
            }

        }

        for(int i = 1; i < DataManager.instance.gameData.weapons.Length; i++) //���� ��ư �ҷ�����
        {
            GameObject toolBtn = Instantiate(toolButton, Vector3.zero, Quaternion.identity);
            toolBtn.transform.parent = toolContents.transform;
            toolBtn.GetComponent<ToolButtonObject>().mainManager = GetComponent<MainManager>();
            toolBtn.GetComponent<ToolButtonObject>().data = DataManager.instance.gameData.weapons[i];
            toolBtn.GetComponent<ToolButtonObject>().Init();
        }

        SoundManager.instance.PLAY_BGM(0);
    }

    public void PanelOnButton(int fadeInPanel) 
    {
        SoundManager.instance.PLAY_SFX(0);
        StartCoroutine(FadeOut(0)); //����ȭ�� ���̵� �ƿ�
        StartCoroutine(FadeIn(fadeInPanel)); //�� �г� ���̵� ��
    }
    public void HomeButton(int fadeOutPanel)
    {
        SoundManager.instance.PLAY_SFX(0);
        StartCoroutine(FadeOut(fadeOutPanel)); //�� �г� ���̵� �ƿ�
        StartCoroutine(FadeIn(0)); //����ȭ�� ���̵� ��
    }
    private void CoinUpdate()
    {
        coinText.text = DataManager.instance.gameData.price.ToString("#,##0");
    }

    #region ========== PrefabButton ==========
    public void MapButton(SO_Stage data, int _clearStars)
    {
        SoundManager.instance.PLAY_SFX(0);

        selectMapID = data.id; //������ �� ���̵� ����

        stageNameText.text = data.stage_name;
        stageExText.text = data.ex;

        //�̼� �ð� ǥ��
        missionText[0].text = ((int)data.missoin_1 / 60).ToString().PadLeft(2, '0')
            + ":" + (data.missoin_1 % 60).ToString().PadLeft(2, '0');

        missionText[1].text = ((int)data.missoin_2 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_2 % 60).ToString().PadLeft(2, '0');

        missionText[2].text = ((int)data.missoin_3 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_3 % 60).ToString().PadLeft(2, '0');

        //���� ���� ǥ��
        if(data.tool_1_id < 400)
        {
            toolImages[0].sprite = toolSprites[data.tool_1_id - 300];
            if(data.tool_2_id < 400)
            {
                toolImages[1].sprite = toolSprites[data.tool_2_id - 300];
            }
            mapToolPanel.SetActive(true);
        }
        else
        {
            mapToolPanel.SetActive(false);
        }

        //Ŭ������ �̼�(��) ǥ��
        if (_clearStars > 0)
        {
            for(int i = 0; i < _clearStars; i++)
            {
                starImages[i].sprite = starSprite;
            }
        }

        mapPopupPanel.SetActive(true);
    }
    public void ToolButton(SO_Weapon data)
    {
        SoundManager.instance.PLAY_SFX(0);

        selectTool = data; //������ �� ����

        toolNameText.text = data.tool_Name;
        toolExText.text = data.ex;
        toolPriceText.text = "���� : " + data.price.ToString("#,##0");
        toolImage.sprite = toolSprites[data.id - 300];

        if (!DataManager.instance.gameData.tools.ContainsKey(selectTool.id))
        {
            return; //������ ������ ����
        }

        if (DataManager.instance.gameData.tools[selectTool.id]) //�̹� �������ִ� ������ �� 
        {
            toolButtonImage.color = Color.gray;
            toolButtonText.text = "���� �Ϸ�";
        }
        else
        {
            if(DataManager.instance.gameData.price >= data.price) //�� ������
            {
                toolButtonImage.color = Color.white;
                toolButtonText.text = "����";
            }
            else //�� ������
            {
                toolButtonImage.color = Color.gray;
                toolButtonText.text = "���� ����";
            }
        }

        toolPopupPanel.SetActive(true);
    }
    public void HistoryButton(SO_Stage data)
    {
        SoundManager.instance.PLAY_SFX(0);

        history_stageNameText.text = data.stage_name;
        history_stageExText.text = data.ex;

        //�̼� �ð� ǥ��
        history_missionText[0].text = ((int)data.missoin_1 / 60).ToString().PadLeft(2, '0')
            + ":" + (data.missoin_1 % 60).ToString().PadLeft(2, '0');

        history_missionText[1].text = ((int)data.missoin_2 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_2 % 60).ToString().PadLeft(2, '0');

        history_missionText[2].text = ((int)data.missoin_3 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_3 % 60).ToString().PadLeft(2, '0');

        //���� ���� ǥ��
        if (data.tool_1_id < 400)
        {
            history_toolImages[0].sprite = toolSprites[data.tool_1_id - 300];
            if (data.tool_2_id < 400)
            {
                history_toolImages[1].sprite = toolSprites[data.tool_2_id - 300];
            }
            history_mapToolPanel.SetActive(true);
        }
        else
        {
            history_mapToolPanel.SetActive(false);
        }

        historyPopupPanel.SetActive(true);
    }
    #endregion

    #region ==========PopupButton========== 

    public void StartGameButton()
    {
        SoundManager.instance.PLAY_SFX(0);

        StageManager.StageSelect(selectMapID - 200);        
    }

    public void ToolOrderButton()
    {
        SoundManager.instance.PLAY_SFX(0);

        if (!DataManager.instance.gameData.tools.ContainsKey(selectTool.id))
        {            
            return; //������ ������ ����
        }
        if (DataManager.instance.gameData.tools[selectTool.id])
        {
            //Debug.Log("�̹� ����");
            //�̹� �������ִ� ������ �� ��ġ�ص� ����
            return;
        }
        if (DataManager.instance.gameData.price < selectTool.price)
        {
           // Debug.Log("���� ����");
            return; //�� ������ ��ġ�ص� ����
        }

        DataManager.instance.gameData.price -= selectTool.price; //���� ����
        DataManager.instance.gameData.tools[selectTool.id] = true; //���� ����

        DataManager.instance.SaveGameData(); //����

        //UI ������Ʈ
        CoinUpdate();
        ToolButton(selectTool);
    }

    #endregion

    private IEnumerator FadeIn(int type)
    {
        float accumTime = 0f;
        yield return new WaitForSeconds(0.3f);

        while (accumTime < 0.3f)
        {
            panelList[type].alpha = Mathf.Lerp(0f, 1f, accumTime / 0.3f);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        panelList[type].alpha = 1f;
        panelList[type].blocksRaycasts = true;
        
    }
    private IEnumerator FadeOut(int type)
    {
        panelList[type].blocksRaycasts = false;
        float accumTime = 0f;
        while (accumTime < 0.3f)
        {
            panelList[type].alpha = Mathf.Lerp(1f, 0f, accumTime / 0.3f);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        panelList[type].alpha = 0f;
    }
}
