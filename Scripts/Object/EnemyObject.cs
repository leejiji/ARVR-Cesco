using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyObject : MonoBehaviour
{
    [SerializeField]
    SO_Cockrach enemyData;
    [SerializeField]
    GameObject real_Model;
    [SerializeField]
    GameObject cute_Model;

    public int planesID = 0;

    private ARPlane plane;
    private Vector3 rotdir = new Vector3(0,0,0);

    private int id;
    private float hp;
    private float speed;
    private int coin;

    private bool isMove = false;
    private bool isReverse = false;
    private bool isVertical = false;

    private bool reSpawn = false;
    private float reSpawnTimer = 5f;

    private void Awake()
    {
        id = enemyData.id;
        plane = GameManager.instance.arPlanes[planesID];
        hp = enemyData.hp;
        speed = enemyData.speed;
        coin = enemyData.coin;

        //���� ���� �ָ� ���� �����̰�
        if(Mathf.Abs(this.gameObject.transform.eulerAngles.z) > 80f)
        {
            isVertical = true;
        }
        else
        {
            isVertical = false;
        }

        //�Ϳ�� ��� ��
        if(DataManager.instance.gameData.cuteMode == true)
        {
            real_Model.SetActive(false);
            cute_Model.SetActive(true);
        }
        else
        {
            real_Model.SetActive(true);
            cute_Model.SetActive(false);
        }

        StartCoroutine(MoveSwitch());
    }
    private void Update()
    {
        if(id != 1) //������ �����̰�
        {
            return;
        }
        if(reSpawn)
        {
            if(reSpawnTimer >= 0f)
            {
                reSpawnTimer -= Time.deltaTime;
            }
            else
            {
                this.gameObject.transform.position = plane.center;
                this.gameObject.transform.rotation = plane.transform.rotation;
                reSpawnTimer = 5f;
                reSpawn = false;
            }
        }
        if (isReverse)
        {
            Rotate();
        }
        if (isMove)
        {
            Move();
        }
        else
        {
            if(isVertical) //���� ��
            {
                float x = Random.Range(-90f, 90f);
                rotdir = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else //�ٴ��� ��
            {
                float y = Random.Range(-90f, 90f);
                rotdir = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
            }

            isMove = true;
        }
    }
    private void Move() //������
    {
        if (isVertical) //���� ��
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else //�ٴ��� ��
        {
            transform.position += transform.right * speed * Time.deltaTime;            
        }

        if(this.transform.rotation != Quaternion.Euler(rotdir) && isReverse == false) //ȸ��
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotdir), Time.deltaTime * 10.0f);
        }
    }
    private void Rotate() //���� ����� �� 180�� ȸ��
    {
        Vector3 dirVec = new Vector3(0, 0, 0);

        if (isVertical)
        {
            dirVec = new Vector3(180f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            rotdir = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(dirVec), Time.deltaTime * 10.0f);

        if (this.transform.rotation == Quaternion.Euler(dirVec))
        {
            isReverse = false;
        }
    }
    public void Damage(float damaged)
    {
        hp -= damaged;

        if (hp <= 0) //���
        {
            GameManager.instance.nowCoin += coin;
            if(id == 1) //�� ������ ��
            {
                GameManager.instance.nowBA += 1;
            }
            else if(id == 2) //�������� ��
            {
                GameManager.instance.nowBAs += 1;
            }
            Destroy(this.gameObject);
        }
    }
    
    IEnumerator MoveSwitch() //���� ��ȯ ����ġ
    {
        float sec = Random.Range(3f, 7f);

        yield return new WaitForSeconds(sec);
        isMove = false; //3~7�ʿ� �ѹ��� ���� ��ȯ

        StartCoroutine(MoveSwitch());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ARPlane>() == plane)
        {
            //cube.SetActive(true);
            reSpawn = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //cube.SetActive(false);
        isReverse = true;
        reSpawn = true;
    }
}
