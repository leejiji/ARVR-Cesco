using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCaemra;
    [SerializeField]
    private LayerMask enemyLayerMask;
    [SerializeField]
    private LayerMask obstructionLayerMask;
    [SerializeField]
    private GameObject Muzzle;
    [SerializeField]
    private GameObject Spray_effect;
    [SerializeField]
    private GameObject fireGun_effect;

    private Vector2 touchPosition;
    private Ray ray;
    private RaycastHit hit;
       
    [SerializeField] private List<SO_Weapon> weapons;
    [SerializeField] private Animator weaponAni;

    private void Update()
    {
        if(GameManager.instance.state != State.GAME) //���ӻ��� �ƴϸ� ����
        {
            return;
        }
        if (!Utility.TryGetInputPosition(out touchPosition)) //��ġ ���ϰ� �ִ� ������ �� ����
        {
            return;
        }
        else
        {           
            ray = arCaemra.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask)) //����
            {
                if(weapons[GameManager.instance.nowTool - 300].tool_Name == "ȭ�� ����")
                {
                    EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, EffectManager.EffectType.Fire);
                }
                else if(weapons[GameManager.instance.nowTool - 300].tool_Name == "����")
                {
                    EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, EffectManager.EffectType.Water);
                }
                else
                {
                    EffectManager.Instance.PlayHitEffect(hit.point, hit.normal);
                }
                hit.transform.GetComponent<EnemyObject>().Damage(weapons[GameManager.instance.nowTool - 300].power); //���� ���ݷ¸�ŭ ����
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstructionLayerMask)) //�׿�
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal);
                hit.transform.GetComponent<obstructionObject>().Damage(weapons[GameManager.instance.nowTool - 300].power); //���� ���ݷ¸�ŭ ����
            }
            else
            {
                return;
            }

            //�ѱ� ����Ʈ
            Muzzle.transform.LookAt(hit.transform);

            //�ִ�,����Ʈ, ����
            StartCoroutine(Shot());
        }

    }

    IEnumerator Shot()
    {
        if (weapons[GameManager.instance.nowTool - 300].isgun == true) //���̸� �� �ִ�
        {
            weaponAni.SetTrigger("Shot");
        }
        else
        {
            weaponAni.SetTrigger("Attack");
        }

        SoundManager.instance.PLAY_SFX(GameManager.instance.nowTool); //ȿ����

        if (GameManager.instance.nowTool == 301) //��������
        {
            Spray_effect.SetActive(true);            
        }
        else if (GameManager.instance.nowTool == 302) //ȭ������
        {
            fireGun_effect.SetActive(true);
        }
        yield return new WaitForSeconds(1f);

        Spray_effect.SetActive(false);
        fireGun_effect.SetActive(false);
        weaponAni.SetTrigger("Idle");
    }
}
