using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    // Ready, Play State(FSM)

    // (����) Mode ��ȯ �Լ�: Motion�� ����ϴ� ��ü�� Ȱ��ȭ/��Ȱ��ȭ
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
    // (����) SetVar: ��庰 �� ������ ���� [�߷�(gravity), �̵��ӵ�(moveSpeed), jumpPower, �ʱ⼳����] 
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

    // StartPosition������ �ʱⰪ�� �������ִ� �Լ�
    void SetStartPositionSetting()
    { 
        // startPosition�� Transform �� Setting �� �������ֱ� 
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
