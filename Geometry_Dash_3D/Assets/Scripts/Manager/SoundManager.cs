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

    public float bgSoundVolume = 0.7f;                         // ������� ���� ��ġ

    AudioSource readyCountDownSound;

    public Slider bgSoundSlideBar;                              // ������� ���� ���� Slide


    // Start is called before the first frame update
    void Start()
    {
        readyCountDownSound = GetComponent<AudioSource>();      // ready Count Down�� Auido Source
        bgSoundSlideBar.value = 0.7f;                           // �ʱ� bgSound Volume
    }
    void Update()
    {
        bgSoundVolume = bgSoundSlideBar.value;
        bgSoundVolumeControlManager();
    }

    // Sound �Ͻ�����
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

    public void bgSoundPlayinManager() { bgSoundControl.Instance.bgSoundPlay(); }                                   // ������� �ѱ�
    public void bgSoundStopinManager() { bgSoundControl.Instance.bgSoundStop(); }                                   // ������� ����
    public void bgSoundPauseinManager() { bgSoundControl.Instance.bgSoundPause(); }
    public void bgSoundUnPauseinManager() { bgSoundControl.Instance.bgSoundUnPause(); }
    public void bgSoundVolumeControlManager() { bgSoundControl.Instance.bgSoundVolumeControl(bgSoundVolume); }      // ������� volume ����
    public void CountdownSoundPlay() { readyCountDownSound.Play(0); }                                               // ��� �غ� Countdown Sound ���


}
