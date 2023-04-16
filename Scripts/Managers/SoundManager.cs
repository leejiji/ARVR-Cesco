using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] List<AudioClip> BGM_Clips = new List<AudioClip>();
    [SerializeField] List<AudioClip> Tool_Clips = new List<AudioClip>();
    [SerializeField] List<AudioClip> SFX_Clips = new List<AudioClip>();

    public AudioSource SFX_Player = null;
    public AudioSource BGM_Player = null;

    public void Awake()
    {
        instance = this;
    }

    public void PLAY_SFX(int iInAudioKey)
    {
        if(iInAudioKey >= 300)
        {
            SFX_Player.PlayOneShot(Tool_Clips[iInAudioKey - 300]);
        }
        else
        {
            SFX_Player.PlayOneShot(SFX_Clips[iInAudioKey]);
        }
    }

    public void PLAY_BGM(int iInAudioKey)
    {
        BGM_Player.Stop();
        BGM_Player.clip = BGM_Clips[iInAudioKey];
        BGM_Player.Play();
    }
}