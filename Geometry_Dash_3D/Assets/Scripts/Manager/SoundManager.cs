using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public float bgSoundVolume = 0.7f;                         // 배경음악 볼륨 수치

    AudioSource readyCountDownSound;

    public Slider bgSoundSlideBar;                              // 배경음악 볼륨 제어 Slide


    // Start is called before the first frame update
    void Start()
    {
        readyCountDownSound = GetComponent<AudioSource>();      // ready Count Down의 Auido Source
        bgSoundSlideBar.value = 0.7f;                           // 초기 bgSound Volume
    }
    void Update()
    {
        bgSoundVolume = bgSoundSlideBar.value;
        bgSoundVolumeControlManager();
    }

    // Sound 일시중지
    public void SoundPause()
    {
            readyCountDownSound.Pause();
            bgSoundPauseinManager();
    }

    public void SoundUnPause()
    {
            readyCountDownSound.UnPause();
            bgSoundUnPauseinManager();
    }

    public void bgSoundPlayinManager() { bgSoundControl.Instance.bgSoundPlay(); }                                   // 배경음악 켜기
    public void bgSoundStopinManager() { bgSoundControl.Instance.bgSoundStop(); }                                   // 배경음악 끄기
    public void bgSoundPauseinManager() { bgSoundControl.Instance.bgSoundPause(); }
    public void bgSoundUnPauseinManager() { bgSoundControl.Instance.bgSoundUnPause(); }
    public void bgSoundVolumeControlManager() { bgSoundControl.Instance.bgSoundVolumeControl(bgSoundVolume); }      // 배경음악 volume 제어
    public void CountdownSoundPlay() { readyCountDownSound.Play(0); }                                               // 출발 준비 Countdown Sound 재생


}
