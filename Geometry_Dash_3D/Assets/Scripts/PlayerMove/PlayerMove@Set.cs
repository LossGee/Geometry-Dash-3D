using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    // Ready, Play State(FSM)

    // (공통) Mode 전환 함수: Motion을 담당하는 객체를 활성화/비활성화
    void SetMotion()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                MotionCube.SetActive(true);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE_vertical.SetActive(false);
                MotionSATELLITE_horizontal.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.UFO:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(true);
                MotionRace.SetActive(false);
                MotionSATELLITE_vertical.SetActive(false);
                MotionSATELLITE_horizontal.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(true);
                MotionSATELLITE_vertical.SetActive(false);
                MotionSATELLITE_horizontal.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.SATELLITE_vertical:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE_vertical.SetActive(true);
                MotionSATELLITE_horizontal.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.SATELLITE_horizontal:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE_vertical.SetActive(false);
                MotionSATELLITE_horizontal.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE_vertical.SetActive(false);
                MotionSATELLITE_horizontal.SetActive(false);
                MotionRocket.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE_vertical.SetActive(false);
                MotionSATELLITE_horizontal.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(true);
                break;
        }
    }
    // (공통) SetVar: 모드별 각 변수값 설정 [중력(gravity), 이동속도(moveSpeed), jumpPower, 초기설정값] 
    void SetVar()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                gravity = DefaultGravity;
                moveSpeed = cubeMoveSpeed;
                jumpPower = cubeJumpPower;
                break;
            case ModeState.UFO:
                gravity = DefaultGravity;
                moveSpeed = ufoMoveSpeed;
                jumpPower = ufoJumpPower;
                break;
            case ModeState.RACE:
                gravity = DefaultGravity;
                moveSpeed = raceMoveSpeed;
                CheckLineNumber();
                break;
            case ModeState.SATELLITE_vertical:
                SATELLITE_verticalUpDownState = true;
                moveSpeed = SATELLITE_verticalMoveSpeed;
                break;
            case ModeState.SATELLITE_horizontal:
                gravity = DefaultGravity;
                SATELLITE_horizontalRightLeftState = true;
                moveSpeed = SATELLITE_horizontalMoveSpeed;
                break;
            case ModeState.ROCKET:
                gravity = RocketGravity;
                moveSpeed = rocketMoveSpeed;
                jumpPower = rocketUpPower;
                break;
            case ModeState.FORWARD:
                moveSpeed = forwardMoveSpeed;
                break;
        }
    }

    // StartPosition에서의 초기값을 설정해주는 함수
    void SetStartPositionSetting()
    { 
        // startPosition의 Transform 및 Setting 값 설정해주기 
        Mode = ModeState.CUBE;
        dir = Vector3.forward;
        jumpState = false;
        jumpTurn = true;
        dropTurn = true;

        //transform.position = startPosition.position;
        //transform.rotation = startPosition.rotation;
        reversGravityState = false;
        yVelocity = 0;
        CameraMove.Instance.reverseLeftRightAtStartPoint();
        
    }
}
