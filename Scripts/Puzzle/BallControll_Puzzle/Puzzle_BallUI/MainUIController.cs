using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUIController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public int fadeNum;

    public float bgmAudio;
    public float sfxAudio;

    public void ONChangerBGM()
    {
        SoundManager.instance.SetBGMVolume(bgmSlider.value);
    }

    public void ONChangerSFX()
    {
        SoundManager.instance.SetSFXVolume(sfxSlider.value);
    }

    public void ONGameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void OnSetMainGameUI()
    {
        gameObject.SetActive(false);
    }
}
