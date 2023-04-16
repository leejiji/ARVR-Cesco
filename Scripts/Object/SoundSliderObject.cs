using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderObject : MonoBehaviour
{
    [SerializeField] private string sound_type;

    public void Start()
    {
        Init();

        GetComponent<Slider>().onValueChanged.AddListener(delegate
        {
            VolumeChange();
        });
    }
    private void Init()
    {
        if (sound_type == "BGM")
        {
            GetComponent<Slider>().value = DataManager.instance.gameData.BGMSound;

        }
        else
        {
            GetComponent<Slider>().value = DataManager.instance.gameData.SFXSound;
        }
    }

    void VolumeChange()
    {
        if(sound_type == "BGM")
        {
            SoundManager.instance.BGM_Player.volume = GetComponent<Slider>().value;
            DataManager.instance.gameData.BGMSound = GetComponent<Slider>().value;

        }
        else
        {
            SoundManager.instance.SFX_Player.volume = GetComponent<Slider>().value;
            DataManager.instance.gameData.SFXSound = GetComponent<Slider>().value;
        }
    }
}
