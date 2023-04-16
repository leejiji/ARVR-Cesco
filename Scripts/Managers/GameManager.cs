using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public enum State
{
    MAP_GENERATE,
    READY,
    GAME,
    PAUSE,
    GAME_OVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public State state = State.MAP_GENERATE;

    public UIManager uiManager;

    //데이터 관련
    public SO_Stage stageData = null; //스테이지 데이터
    public float gameTime = 0f; //게임(스테이지) 플레이 시간
    public int nowCoin = 0;
    public int nowBA = 0; //지금까지 죽인 바퀴
    public int nowBAs = 0; //지금까지 죽인 바퀴 떼

    //맵 스캔 관련
    public List<ARPlane> arPlanes; //맵 인식 후의 arPlane 리스트

    [SerializeField] private GameObject startButton; //시작 버튼
    [SerializeField] private GameObject scanPanel; //스캔 패널
    [SerializeField] private Material planeMaterial; //투명 material
    private GameObject arSessionOrigin;
    private float readyTimer = 1f; //준비 시간

    //게임관련
    [SerializeField] private GameObject gamePanel; //게임 패널
    [SerializeField] private GameObject pausePanel; //정지 패널

    //스폰 관련
    public bool isSpawnOver = false; //스폰끝남
    public bool isFlySpawn = false; //파리 스폰
    public bool isCaterpillarSpawn = false; //송충이 스폰

    //게임 오버 관련
    public bool isGameOver = false;
    [SerializeField] private GameObject gameOverPanel; //게임오버 패널

    //무기 관련
    public bool isToolChange = false;
    public int nowTool = 0; //현재 선택한 무기 아이디
    public GameObject[] nowWeaponObject; //현재 선택한 무기 오브젝트
    public GameObject toolPanel; //툴 패널
    [SerializeField] private GameObject toolContentsPanel; //툴 콘텐츠 패널
    [SerializeField] private GameObject toolButton; //툴 버튼 프리팹


    void Awake()
    {
        instance = this;
        arSessionOrigin = GameObject.Find("AR Session Origin");

        stageData = FindObjectOfType<StageManager>().stageData;
        nowTool = 301;
        nowWeaponObject[nowTool - 300].SetActive(true);

        uiManager = FindObjectOfType<UIManager>();
    }
    private void Update()
    {
        UpdateState();
    }

    public void EnterState(State _state)
    {
        switch (_state)
        {
            case State.MAP_GENERATE:
                SoundManager.instance.PLAY_BGM(1);

                if (stageData.tool_1_id > 400) //따로 제한 무기 없을 시
                {
                    for (int i = 0; i < DataManager.instance.gameData.tools.Count; i++)
                    {
                        if (DataManager.instance.gameData.tools.ContainsKey(300 + i))
                        {
                            if (DataManager.instance.gameData.tools[300 + i]) //구매한 아이템 버튼 표시
                            {
                                GameObject toolBtn = Instantiate(toolButton, Vector3.zero, Quaternion.identity);
                                toolBtn.transform.parent = toolContentsPanel.transform;
                                toolBtn.GetComponent<ToolSelectButtonObject>().data = DataManager.instance.gameData.weapons[i];
                                toolBtn.GetComponent<ToolSelectButtonObject>().Init();
                            }
                        }
                    }
                }
                else //무기 제한 있을 시
                {
                    if (DataManager.instance.gameData.tools.ContainsKey(stageData.tool_1_id))
                    {
                        if (DataManager.instance.gameData.tools[stageData.tool_1_id]) //제한 무기 있으면
                        {
                            GameObject toolBtn = Instantiate(toolButton, Vector3.zero, Quaternion.identity);
                            toolBtn.transform.parent = toolContentsPanel.transform;
                            toolBtn.GetComponent<ToolSelectButtonObject>().data = DataManager.instance.gameData.weapons[stageData.tool_1_id - 300];
                            toolBtn.GetComponent<ToolSelectButtonObject>().Init();
                        }
                    }
                    if (DataManager.instance.gameData.tools.ContainsKey(stageData.tool_2_id))
                    {
                        if (DataManager.instance.gameData.tools[stageData.tool_2_id]) //제한 무기 2번도 있으면
                        {
                            GameObject toolBtn = Instantiate(toolButton, Vector3.zero, Quaternion.identity);
                            toolBtn.transform.parent = toolContentsPanel.transform;
                            toolBtn.GetComponent<ToolSelectButtonObject>().data = DataManager.instance.gameData.weapons[stageData.tool_2_id - 300];
                            toolBtn.GetComponent<ToolSelectButtonObject>().Init();
                        }
                    }
                }
                state = State.MAP_GENERATE;
                break;
            case State.READY:
                state = State.READY;
                break;
            case State.GAME:
                SoundManager.instance.PLAY_BGM(2);

                state = State.GAME;
                break;
            case State.PAUSE:
                pausePanel.SetActive(true);
                state = State.PAUSE;
                break;
            case State.GAME_OVER:
                state = State.GAME_OVER;
                DataManager.instance.StageClear(); //현재 스테이지 클리어 정보 저장
                uiManager.GameOverUI();
                gameOverPanel.SetActive(true);
                break;
        }
    }
    public void UpdateState()
    {
        switch (state)
        {
            case State.MAP_GENERATE:
                if (arPlanes.Count <= 2) //plane 너무 적으면 시작 버튼 안 나옴
                {
                    startButton.SetActive(false);
                }
                else
                {
                    startButton.SetActive(true);
                }
                break;
            case State.READY:
                if (readyTimer > 0f)
                {
                    readyTimer -= Time.deltaTime;
                }
                else
                {
                    ExitState();
                }
                break;
            case State.GAME:
                gameTime += Time.deltaTime; //게임 시간 카운트

                if (isSpawnOver && nowBA == stageData.ba_amount && nowBAs == stageData.bas_amount)
                {
                    isGameOver = true;
                    ExitState();
                }
                break;
            case State.PAUSE:

                break;
            case State.GAME_OVER:

                break;
        }
    }
    public void ExitState()
    {
        switch (state)
        {
            case State.MAP_GENERATE:
                scanPanel.SetActive(false); //스캔패널 안보이게
                gamePanel.SetActive(true); //게임패널 보이기

                // arSessionOrigin.GetComponent<ARPlaneManager>().enabled = false; //ARPlaneManger 비활성화
                arSessionOrigin.GetComponent<ARPlaneManager>().requestedDetectionMode = PlaneDetectionMode.None; //인식 안하게
                //arSessionOrigin.GetComponent<ARPlaneManager>().planePrefab.GetComponent<MeshRenderer>().material = planeMaterial; //material 변경

                foreach (ARPlane plane in arPlanes)
                {
                    plane.GetComponent<Renderer>().material = planeMaterial; //모든 arPlnae material 변경
                    plane.GetComponent<Renderer>().enabled = false;
                    plane.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case State.READY:
                EnterState(State.GAME);
                break;
            case State.GAME:
                if (isGameOver) //게임 오버라면 게임오버로
                {
                    EnterState(State.GAME_OVER);
                }
                else //게임오버가 아니라면 일시정지로
                {
                    EnterState(State.PAUSE);
                }
                break;
            case State.PAUSE:
                pausePanel.SetActive(false);
                EnterState(State.GAME);
                break;
            case State.GAME_OVER:
                SoundManager.instance.PLAY_BGM(0);

                break;
        }
    }

}
