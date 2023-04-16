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

    //������ ����
    public SO_Stage stageData = null; //�������� ������
    public float gameTime = 0f; //����(��������) �÷��� �ð�
    public int nowCoin = 0;
    public int nowBA = 0; //���ݱ��� ���� ����
    public int nowBAs = 0; //���ݱ��� ���� ���� ��

    //�� ��ĵ ����
    public List<ARPlane> arPlanes; //�� �ν� ���� arPlane ����Ʈ

    [SerializeField] private GameObject startButton; //���� ��ư
    [SerializeField] private GameObject scanPanel; //��ĵ �г�
    [SerializeField] private Material planeMaterial; //���� material
    private GameObject arSessionOrigin;
    private float readyTimer = 1f; //�غ� �ð�

    //���Ӱ���
    [SerializeField] private GameObject gamePanel; //���� �г�
    [SerializeField] private GameObject pausePanel; //���� �г�

    //���� ����
    public bool isSpawnOver = false; //��������
    public bool isFlySpawn = false; //�ĸ� ����
    public bool isCaterpillarSpawn = false; //������ ����

    //���� ���� ����
    public bool isGameOver = false;
    [SerializeField] private GameObject gameOverPanel; //���ӿ��� �г�

    //���� ����
    public bool isToolChange = false;
    public int nowTool = 0; //���� ������ ���� ���̵�
    public GameObject[] nowWeaponObject; //���� ������ ���� ������Ʈ
    public GameObject toolPanel; //�� �г�
    [SerializeField] private GameObject toolContentsPanel; //�� ������ �г�
    [SerializeField] private GameObject toolButton; //�� ��ư ������


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

                if (stageData.tool_1_id > 400) //���� ���� ���� ���� ��
                {
                    for (int i = 0; i < DataManager.instance.gameData.tools.Count; i++)
                    {
                        if (DataManager.instance.gameData.tools.ContainsKey(300 + i))
                        {
                            if (DataManager.instance.gameData.tools[300 + i]) //������ ������ ��ư ǥ��
                            {
                                GameObject toolBtn = Instantiate(toolButton, Vector3.zero, Quaternion.identity);
                                toolBtn.transform.parent = toolContentsPanel.transform;
                                toolBtn.GetComponent<ToolSelectButtonObject>().data = DataManager.instance.gameData.weapons[i];
                                toolBtn.GetComponent<ToolSelectButtonObject>().Init();
                            }
                        }
                    }
                }
                else //���� ���� ���� ��
                {
                    if (DataManager.instance.gameData.tools.ContainsKey(stageData.tool_1_id))
                    {
                        if (DataManager.instance.gameData.tools[stageData.tool_1_id]) //���� ���� ������
                        {
                            GameObject toolBtn = Instantiate(toolButton, Vector3.zero, Quaternion.identity);
                            toolBtn.transform.parent = toolContentsPanel.transform;
                            toolBtn.GetComponent<ToolSelectButtonObject>().data = DataManager.instance.gameData.weapons[stageData.tool_1_id - 300];
                            toolBtn.GetComponent<ToolSelectButtonObject>().Init();
                        }
                    }
                    if (DataManager.instance.gameData.tools.ContainsKey(stageData.tool_2_id))
                    {
                        if (DataManager.instance.gameData.tools[stageData.tool_2_id]) //���� ���� 2���� ������
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
                DataManager.instance.StageClear(); //���� �������� Ŭ���� ���� ����
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
                if (arPlanes.Count <= 2) //plane �ʹ� ������ ���� ��ư �� ����
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
                gameTime += Time.deltaTime; //���� �ð� ī��Ʈ

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
                scanPanel.SetActive(false); //��ĵ�г� �Ⱥ��̰�
                gamePanel.SetActive(true); //�����г� ���̱�

                // arSessionOrigin.GetComponent<ARPlaneManager>().enabled = false; //ARPlaneManger ��Ȱ��ȭ
                arSessionOrigin.GetComponent<ARPlaneManager>().requestedDetectionMode = PlaneDetectionMode.None; //�ν� ���ϰ�
                //arSessionOrigin.GetComponent<ARPlaneManager>().planePrefab.GetComponent<MeshRenderer>().material = planeMaterial; //material ����

                foreach (ARPlane plane in arPlanes)
                {
                    plane.GetComponent<Renderer>().material = planeMaterial; //��� arPlnae material ����
                    plane.GetComponent<Renderer>().enabled = false;
                    plane.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case State.READY:
                EnterState(State.GAME);
                break;
            case State.GAME:
                if (isGameOver) //���� ������� ���ӿ�����
                {
                    EnterState(State.GAME_OVER);
                }
                else //���ӿ����� �ƴ϶�� �Ͻ�������
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
