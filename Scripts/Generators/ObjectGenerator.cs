using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;
    
    [SerializeField]
    private GameObject enemyObject;
    [SerializeField]
    private GameObject BAsObject;
    [SerializeField]
    private GameObject caterpillarObject;
    [SerializeField]
    private GameObject flyObject;

    private int spawnBACount = 0;
    private int spawnBAsCount = 0;
    private int spawnCaterCount = 1;
    private int spawnFlyCount = 1;

    private int spawnBA = 0;
    private int spawnBAs = 0;
    private int spawnFly = 0;
    private int spawnCater = 0;

    private float maxTimer = 5f;
    private float timer = 0f;

    private void Start()
    {
        spawnBA = GameManager.instance.stageData.ba_amount;
        spawnBAs = GameManager.instance.stageData.bas_amount;
        spawnFly = GameManager.instance.stageData.fly_count;
        spawnCater = GameManager.instance.stageData.caterpillar_count;

    }
    void Update()
    {      
        if(GameManager.instance.state != State.GAME || GameManager.instance.isSpawnOver == true) //���� ���� �ƴϰų� ���� �������� ����
        {
            return;
        }

        if (spawnBACount != 0 && spawnCater < 10000 && spawnBACount == (spawnCater * spawnCaterCount)) //������ ����
        {
            caterpillarObject.SetActive(true);
            Vibe();
            spawnCaterCount++;
        }

        if (spawnFly < 10000) //���ĸ� ����
        {
            arCameraManager.frameReceived += FlySpawn;
        }

        if (timer < maxTimer) //���� ����
        {
            timer += Time.deltaTime;
        }
        else
        {
            BaSpawn();
        }
    }
    private void Vibe() //����
    {
        Handheld.Vibrate();
    }
    private void BaSpawn()//���� ���� 
    {
        int randPlace = Random.Range(0, GameManager.instance.arPlanes.Count);

        if (spawnBACount == (spawnBA / spawnBAs) * (spawnBAsCount + 1) && spawnBACount != 0) //���� �� ����
        {
            var enemy = Instantiate(BAsObject, GameManager.instance.arPlanes[randPlace].center, GameManager.instance.arPlanes[randPlace].transform.rotation);
            enemy.GetComponent<EnemyObject>().planesID = randPlace;
            spawnBAsCount++;
        }
        else //�������� ����
        {
            var enemy = Instantiate(enemyObject, GameManager.instance.arPlanes[randPlace].center, GameManager.instance.arPlanes[randPlace].transform.rotation);
            enemy.GetComponent<EnemyObject>().planesID = randPlace;
            spawnBACount++;
        }

        Vibe();

        if (spawnBACount == spawnBA && spawnBAsCount == spawnBAs && GameManager.instance.isSpawnOver == false) //������ ������ �� ��������
        {
            GameManager.instance.isSpawnOver = true; //���� ��
        }

        timer = Random.Range(0f, 2f);
    }

    private void FlySpawn(ARCameraFrameEventArgs args) //���˻� �� ���ĸ� ����
    {
        var brightness = args.lightEstimation.averageBrightness;

        if(brightness.Value > 0.35f)
        {
            if (spawnBACount != 0 && spawnBACount == (spawnFly * spawnFlyCount)) //���ĸ� ����
            {
                int randPlace = Random.Range(0, GameManager.instance.arPlanes.Count);
                var enemy = Instantiate(flyObject, GameManager.instance.arPlanes[randPlace].center, GameManager.instance.arPlanes[randPlace].transform.rotation);
                Vibe();
                spawnFlyCount++;
            }
        }
    }
}
