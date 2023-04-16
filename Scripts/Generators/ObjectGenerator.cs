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
        if(GameManager.instance.state != State.GAME || GameManager.instance.isSpawnOver == true) //게임 상태 아니거나 스폰 끝났으면 리턴
        {
            return;
        }

        if (spawnBACount != 0 && spawnCater < 10000 && spawnBACount == (spawnCater * spawnCaterCount)) //송충이 스폰
        {
            caterpillarObject.SetActive(true);
            Vibe();
            spawnCaterCount++;
        }

        if (spawnFly < 10000) //날파리 스폰
        {
            arCameraManager.frameReceived += FlySpawn;
        }

        if (timer < maxTimer) //바퀴 스폰
        {
            timer += Time.deltaTime;
        }
        else
        {
            BaSpawn();
        }
    }
    private void Vibe() //진동
    {
        Handheld.Vibrate();
    }
    private void BaSpawn()//바퀴 스폰 
    {
        int randPlace = Random.Range(0, GameManager.instance.arPlanes.Count);

        if (spawnBACount == (spawnBA / spawnBAs) * (spawnBAsCount + 1) && spawnBACount != 0) //바퀴 떼 스폰
        {
            var enemy = Instantiate(BAsObject, GameManager.instance.arPlanes[randPlace].center, GameManager.instance.arPlanes[randPlace].transform.rotation);
            enemy.GetComponent<EnemyObject>().planesID = randPlace;
            spawnBAsCount++;
        }
        else //바퀴벌레 스폰
        {
            var enemy = Instantiate(enemyObject, GameManager.instance.arPlanes[randPlace].center, GameManager.instance.arPlanes[randPlace].transform.rotation);
            enemy.GetComponent<EnemyObject>().planesID = randPlace;
            spawnBACount++;
        }

        Vibe();

        if (spawnBACount == spawnBA && spawnBAsCount == spawnBAs && GameManager.instance.isSpawnOver == false) //바퀴랑 바퀴떼 다 나왔으면
        {
            GameManager.instance.isSpawnOver = true; //스폰 끝
        }

        timer = Random.Range(0f, 2f);
    }

    private void FlySpawn(ARCameraFrameEventArgs args) //밝기검사 및 날파리 스폰
    {
        var brightness = args.lightEstimation.averageBrightness;

        if(brightness.Value > 0.35f)
        {
            if (spawnBACount != 0 && spawnBACount == (spawnFly * spawnFlyCount)) //날파리 스폰
            {
                int randPlace = Random.Range(0, GameManager.instance.arPlanes.Count);
                var enemy = Instantiate(flyObject, GameManager.instance.arPlanes[randPlace].center, GameManager.instance.arPlanes[randPlace].transform.rotation);
                Vibe();
                spawnFlyCount++;
            }
        }
    }
}
