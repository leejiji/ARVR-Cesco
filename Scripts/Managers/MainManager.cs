using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    //==각 패널 캔버스 그룹==//
    [SerializeField] private CanvasGroup[] panelList; //0 = Main, 1=Quest, 2=Tools, 3=History, 4=Setting
    [SerializeField] private StageManager StageManager;
    [SerializeField] private Text coinText;

    //==맵 버튼==//
    [Header("맵 버튼 생성"), Space(10)]
    [SerializeField] private GameObject mapContents;
    [SerializeField] private GameObject mapButton;

    //==맵 팝업==//
    [Header("맵 팝업 관련"), Space(10)]
    [SerializeField] private GameObject mapPopupPanel;
    [SerializeField] private Text stageNameText;
    [SerializeField] private Text stageExText;
    [SerializeField] private Text[] missionText;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private GameObject mapToolPanel;
    [SerializeField] private Image[] toolImages;
    [SerializeField] private Sprite[] toolSprites;

    //==선택한 맵==//
    private int selectMapID;

    //==무기 팝업==//
    [Header("무기 버튼 생성"), Space(10)]
    [SerializeField] private GameObject toolContents;
    [SerializeField] private GameObject toolButton;

    [Header("무기 팝업 관련"), Space(10)]
    [SerializeField] private GameObject toolPopupPanel;
    [SerializeField] private Text toolNameText;
    [SerializeField] private Text toolExText;
    [SerializeField] private Text toolButtonText;
    [SerializeField] private Text toolPriceText;
    [SerializeField] private Image toolButtonImage;
    [SerializeField] private Image toolImage;

    //==히스토리 버튼==//
    [Header("히스토리 버튼 생성"), Space(10)]
    [SerializeField] private GameObject historyContents;
    [SerializeField] private GameObject historyButton;

    //==히스토리 팝업==//
    [Header("히스토리 팝업 관련"), Space(10)]
    [SerializeField] private GameObject historyPopupPanel;
    [SerializeField] private Text history_stageNameText;
    [SerializeField] private Text history_stageExText;
    [SerializeField] private Text[] history_missionText;
    [SerializeField] private GameObject history_mapToolPanel;
    [SerializeField] private Image[] history_toolImages;

    //==선택한 무기==//
    private SO_Weapon selectTool;

    private void Start()
    {
        DataManager.instance.LoadGameData();
        DataManager.instance.SaveGameData();

        CoinUpdate(); //코인 UI업데이트

        for (int j = 0; j < DataManager.instance.gameData.clearStage; j++) //맵 버튼 불러오기
        {
            int i = j + 1;
            if (DataManager.instance.gameData.stages[i].missoin_3 >= DataManager.instance.gameData.stageTime[i + 200]
            && DataManager.instance.gameData.stageTime[i + 200] > 0f) //이미 클리어한 스테이지일 시
            {
                    //히스토리 버튼 생성
                    GameObject historyBtn = Instantiate(historyButton, Vector3.zero, Quaternion.identity);
                    historyBtn.transform.parent = historyContents.transform;
                    historyBtn.GetComponent<HistoryButtonObject>().mainManager = GetComponent<MainManager>();
                    historyBtn.GetComponent<HistoryButtonObject>().data = DataManager.instance.gameData.stages[i];
                    historyBtn.GetComponent<HistoryButtonObject>().Init();
            }
            else
            {
                //맵 버튼 생성
                GameObject mapBtn = Instantiate(mapButton, Vector3.zero, Quaternion.identity);
                mapBtn.transform.parent = mapContents.transform;
                mapBtn.GetComponent<MapButtonObject>().mainManager = GetComponent<MainManager>();
                mapBtn.GetComponent<MapButtonObject>().data = DataManager.instance.gameData.stages[i];
                mapBtn.GetComponent<MapButtonObject>().Init();
            }

        }

        for(int i = 1; i < DataManager.instance.gameData.weapons.Length; i++) //도구 버튼 불러오기
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
        StartCoroutine(FadeOut(0)); //메인화면 페이드 아웃
        StartCoroutine(FadeIn(fadeInPanel)); //각 패널 페이드 인
    }
    public void HomeButton(int fadeOutPanel)
    {
        SoundManager.instance.PLAY_SFX(0);
        StartCoroutine(FadeOut(fadeOutPanel)); //각 패널 페이드 아웃
        StartCoroutine(FadeIn(0)); //메인화면 페이드 인
    }
    private void CoinUpdate()
    {
        coinText.text = DataManager.instance.gameData.price.ToString("#,##0");
    }

    #region ========== PrefabButton ==========
    public void MapButton(SO_Stage data, int _clearStars)
    {
        SoundManager.instance.PLAY_SFX(0);

        selectMapID = data.id; //선택한 맵 아이디 저장

        stageNameText.text = data.stage_name;
        stageExText.text = data.ex;

        //미션 시간 표시
        missionText[0].text = ((int)data.missoin_1 / 60).ToString().PadLeft(2, '0')
            + ":" + (data.missoin_1 % 60).ToString().PadLeft(2, '0');

        missionText[1].text = ((int)data.missoin_2 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_2 % 60).ToString().PadLeft(2, '0');

        missionText[2].text = ((int)data.missoin_3 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_3 % 60).ToString().PadLeft(2, '0');

        //제한 도구 표시
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

        //클리어한 미션(별) 표시
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

        selectTool = data; //선택한 툴 저장

        toolNameText.text = data.tool_Name;
        toolExText.text = data.ex;
        toolPriceText.text = "가격 : " + data.price.ToString("#,##0");
        toolImage.sprite = toolSprites[data.id - 300];

        if (!DataManager.instance.gameData.tools.ContainsKey(selectTool.id))
        {
            return; //데이터 없으면 리턴
        }

        if (DataManager.instance.gameData.tools[selectTool.id]) //이미 가지고있는 도구일 시 
        {
            toolButtonImage.color = Color.gray;
            toolButtonText.text = "구매 완료";
        }
        else
        {
            if(DataManager.instance.gameData.price >= data.price) //돈 있으면
            {
                toolButtonImage.color = Color.white;
                toolButtonText.text = "구매";
            }
            else //돈 없으면
            {
                toolButtonImage.color = Color.gray;
                toolButtonText.text = "코인 부족";
            }
        }

        toolPopupPanel.SetActive(true);
    }
    public void HistoryButton(SO_Stage data)
    {
        SoundManager.instance.PLAY_SFX(0);

        history_stageNameText.text = data.stage_name;
        history_stageExText.text = data.ex;

        //미션 시간 표시
        history_missionText[0].text = ((int)data.missoin_1 / 60).ToString().PadLeft(2, '0')
            + ":" + (data.missoin_1 % 60).ToString().PadLeft(2, '0');

        history_missionText[1].text = ((int)data.missoin_2 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_2 % 60).ToString().PadLeft(2, '0');

        history_missionText[2].text = ((int)data.missoin_3 / 60).ToString().PadLeft(2, '0')
            + ":" + ((int)data.missoin_3 % 60).ToString().PadLeft(2, '0');

        //제한 도구 표시
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
            return; //데이터 없으면 리턴
        }
        if (DataManager.instance.gameData.tools[selectTool.id])
        {
            //Debug.Log("이미 있음");
            //이미 가지고있는 도구일 시 터치해도 리턴
            return;
        }
        if (DataManager.instance.gameData.price < selectTool.price)
        {
           // Debug.Log("코인 부족");
            return; //돈 없으면 터치해도 리턴
        }

        DataManager.instance.gameData.price -= selectTool.price; //코인 차감
        DataManager.instance.gameData.tools[selectTool.id] = true; //무기 지급

        DataManager.instance.SaveGameData(); //저장

        //UI 업데이트
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
