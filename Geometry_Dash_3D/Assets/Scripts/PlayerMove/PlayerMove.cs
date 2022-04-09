using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPosition.position;                      // startPoint���� ���� 
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;

        SetMotion();
        SetVar();
        OptionManager.Instance.SetInactiveOption();
    }
    // Update is called once per frame

    public void MyPause()
    {
        OptionManager.Instance.SetActivePause();
        prevState = state;
        state = State.PAUSE;
    }

    public void MyResume()
    {
        state = prevState;
        preZpos = currentZpos - 10f;
    }

    private void Update()
    {
        switch (state)
        {
            case State.READY: UpdateReady(); break;
            case State.PLAY: UpdatePlay(); break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))                            // ESC ���� ��, �Ͻ�����
        {
            if (OptionManager.Instance.isPuase)
            {
                OptionManager.Instance.ClickContinueButton();
                SoundManager.Instance.SoundPause();
            }
            else
            {
                MyPause();
                SoundManager.Instance.SoundUnPause();
            }
        }
    }

    private void UpdateReady()
    {
        if (changePlayMode)
        {
            SetStartPositionSetting();                                     // StartPoint������ Setting ������ ��ȯ 
            SetMotion();
            SetVar();
            SoundManager.Instance.CountdownSoundPlay();
            changePlayMode = false;
        }

        // R eady ���¿��� Count Down
        CountDownManager.Instance.ReadyCountDown();
    }

    void UpdatePlay()
    {
        // Dead ��� ����(dead == true�� ��): Play �ٽ� ����
        Dead();                                                             // Player�� ���� ���� �˻�
        if (changePlayMode)
        {
            SetMotion();                                                    // Mode�� Mode ��� GameObject Ȱ��ȭ
            SetVar();                                                       // Mode�� ����(variable) ����
            changePlayMode = false;
        }

        switch (Mode)
        {
            case ModeState.CUBE: UpdateCube(); break;
            case ModeState.UFO: UpdateUFO(); break;
            case ModeState.RACE: UpdateRace(); break;
            case ModeState.SATELLITE_vertical: UpdateSATELLITE_vertical(); break;
            case ModeState.SATELLITE_horizontal: UpdateSATELLITE_horizontal(); break;
            case ModeState.ROCKET: UpdateRocket(); break;
            case ModeState.FORWARD: UpdateForward(); break;
        }
        if (dead)                                                            // dead = true�� �� ����
        {                                                                    
            dead = false;                                                    
            TryCountManager.Instance.AddTryCount();                          // TryCountManger���� �׾����� �˸���
                                                                             
            cc.enabled = false;                                              // Player ��ġ�� ó�� ��ġ�� �̵�
            cc.transform.position = startPosition.position;
            cc.enabled = true;

            SoundManager.Instance.bgSoundStopinManager();                    // ������� ����

            changePlayMode = true;
            state = State.READY;                                             // State: Play -> Ready
        }

    }
}
