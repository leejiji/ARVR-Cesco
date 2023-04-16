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
        if(GameManager.instance.state != State.GAME) //게임상태 아니면 리턴
        {
            return;
        }
        if (!Utility.TryGetInputPosition(out touchPosition)) //터치 안하고 있는 상태일 시 리턴
        {
            return;
        }
        else
        {           
            ray = arCaemra.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayerMask)) //바퀴
            {
                if(weapons[GameManager.instance.nowTool - 300].tool_Name == "화염 방사기")
                {
                    EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, EffectManager.EffectType.Fire);
                }
                else if(weapons[GameManager.instance.nowTool - 300].tool_Name == "물총")
                {
                    EffectManager.Instance.PlayHitEffect(hit.point, hit.normal, EffectManager.EffectType.Water);
                }
                else
                {
                    EffectManager.Instance.PlayHitEffect(hit.point, hit.normal);
                }
                hit.transform.GetComponent<EnemyObject>().Damage(weapons[GameManager.instance.nowTool - 300].power); //무기 공격력만큼 때찌
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstructionLayerMask)) //그외
            {
                EffectManager.Instance.PlayHitEffect(hit.point, hit.normal);
                hit.transform.GetComponent<obstructionObject>().Damage(weapons[GameManager.instance.nowTool - 300].power); //무기 공격력만큼 때찌
            }
            else
            {
                return;
            }

            //총구 이펙트
            Muzzle.transform.LookAt(hit.transform);

            //애니,이펙트, 사운드
            StartCoroutine(Shot());
        }

    }

    IEnumerator Shot()
    {
        if (weapons[GameManager.instance.nowTool - 300].isgun == true) //총이면 총 애니
        {
            weaponAni.SetTrigger("Shot");
        }
        else
        {
            weaponAni.SetTrigger("Attack");
        }

        SoundManager.instance.PLAY_SFX(GameManager.instance.nowTool); //효과음

        if (GameManager.instance.nowTool == 301) //스프레이
        {
            Spray_effect.SetActive(true);            
        }
        else if (GameManager.instance.nowTool == 302) //화염방사기
        {
            fireGun_effect.SetActive(true);
        }
        yield return new WaitForSeconds(1f);

        Spray_effect.SetActive(false);
        fireGun_effect.SetActive(false);
        weaponAni.SetTrigger("Idle");
    }
}
