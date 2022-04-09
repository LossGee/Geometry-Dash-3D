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
        transform.position = startPosition.position;                      // startPoint에서 시작 
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

        if (Input.GetKeyDown(KeyCode.Escape))                            // ESC 누를 시, 일시정지
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
            SetStartPositionSetting();                                     // StartPoint에서의 Setting 값으로 전환 
            SetMotion();
            SetVar();
            SoundManager.Instance.CountdownSoundPlay();
            changePlayMode = false;
        }

        // R eady 상태에서 Count Down
        CountDownManager.Instance.ReadyCountDown();
    }

    void UpdatePlay()
    {
        // Dead 결과 실행(dead == true일 때): Play 다시 시작
        Dead();                                                             // Player의 죽음 조건 검사
        if (changePlayMode)
        {
            SetMotion();                                                    // Mode별 Mode 담당 GameObject 활성화
            SetVar();                                                       // Mode별 변수(variable) 설정
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
        if (dead)                                                            // dead = true일 때 실행
        {                                                                    
            dead = false;                                                    
            TryCountManager.Instance.AddTryCount();                          // TryCountManger에게 죽었음을 알리기
                                                                             
            cc.enabled = false;                                              // Player 위치를 처음 위치로 이동
            cc.transform.position = startPosition.position;
            cc.enabled = true;

            SoundManager.Instance.bgSoundStopinManager();                    // 배경음악 끄기

            changePlayMode = true;
            state = State.READY;                                             // State: Play -> Ready
        }

    }
}
