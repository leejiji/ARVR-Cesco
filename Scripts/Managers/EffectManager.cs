using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    public enum EffectType
    {
        Common,
        Fire,
        Water
    }

    public ParticleSystem HitEffectPrefab;
    public ParticleSystem commonHitEffectPrefab;
    public ParticleSystem fireHitEffectPrefab;
    public ParticleSystem waterHitEffectPrefab;

    public void PlayHitEffect(Vector3 pos, Vector3 normal, EffectType effectType = EffectType.Common)
    {
        var hitPrefab = HitEffectPrefab;
        var targetPrefab = commonHitEffectPrefab;

        if (effectType == EffectType.Fire)
        {
            targetPrefab = fireHitEffectPrefab;
        }
        else if(effectType == EffectType.Water)
        {
            targetPrefab = waterHitEffectPrefab;
        }

        var effect = Instantiate(hitPrefab, pos, Quaternion.LookRotation(normal));
        var seceffect = Instantiate(targetPrefab, pos, Quaternion.LookRotation(normal));

        effect.Play();
        seceffect.Play();
    }
}