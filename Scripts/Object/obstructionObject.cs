using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstructionObject : MonoBehaviour
{
    [SerializeField]
    SO_Cockrach enemyData;

    private float timer = 10f;

    private int id;
    private float hp;
    private int coin;

    private void Awake()
    {
        id = enemyData.id;
        hp = enemyData.hp;
        coin = enemyData.coin;
    }

    void Update()
    {
        if(timer >= 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void Damage(float damaged)
    {
        hp -= damaged;

        if (hp <= 0) //»ç¸Á
        {
            GameManager.instance.nowCoin += coin;
            gameObject.SetActive(false);
        }
    }
}
