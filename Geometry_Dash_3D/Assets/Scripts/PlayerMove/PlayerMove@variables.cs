using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    // Ready, Play State(FSM)
    public enum State
    {
        READY,
        PLAY,
        PAUSE
    }
    public State state;
    public State prevState;

    
    // Player mode
    public enum ModeState
    {
        CUBE,                                                    // 3��Ī ���� CUBE ���(Geometry Dash)
        UFO,                                                     // 3��Ī ���� UFO ���(Geometry Dash)
        RACE,                                                    // 3��Ī ���� ��� �Ʒ��� �������� RACAE ���(Beat Racer)
        SATELLITE_vertical,                                      // 3��Ī ���� SATELLITE_vertical ���(Geometry Dash)
        SATELLITE_horizontal,                                    // 3��Ī ���� ��� �Ʒ��� �����ٺ��� SATELLITE_horizontal ���
        ROCKET,                                                  // 3��Ī ���� ROCEKT ���(Geometry Dash)
        FORWARD                                                  // 3��Ī ���� ������������ ���� (VectorRush)
    }
    public ModeState Mode;
    bool changePlayMode = true;                                      // Mode�� ��ȭ�� ���� �� ture

    // Start Potion
    public Transform startPosition;

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionSATELLITE_vertical;
    public GameObject MotionSATELLITE_horizontal;
    public GameObject MotionRocket;
    public GameObject MotionForward;

    // �߷�(gravity) ���� ����
    float gravity;
    public float DefaultGravity = -9.8f;                            // Cube, UFO ����� �߷�
    public float RocketGravity = -2f;                               // Rocket ����� �߷�  

    // ����������� �����̴� �ӵ�(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;                               // (Cube) ���� �̵��ӵ�
    public float ufoMoveSpeed = 20f;                                // (UFO) ���� �̵��ӵ�
    public float raceMoveSpeed = 20f;                               // (Race) ���� �̵��ӵ�
    public float raceSidemoveSpeed = 3f;                            // (Race) �¿���� �̵� �ӵ�
    public float SATELLITE_verticalMoveSpeed = 20f;                 // (SATELLITE_vertical) ���� �̵��ӵ�
    public float SATELLITE_horizontalMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;                             // (Rocket) ���� �̵��ӵ�
    public float forwardMoveSpeed = 20f;                            // (Forwardk) ���� �̵� �ӵ�

    // Motion �ӵ� ���� ����(TureSpeed)
    public float cubeTurnSpeed = 500f;                              // MotionCube ȸ�� �ӵ�

    // Jump Power(y�� +�������� �������� ��)�� ���õ� ����
    float jumpPower = 2f;
    public float cubeJumpPower = 2f;                                // (Cube) jump power 
    public float ufoJumpPower = 2f;                                 // (UFO) jump power
    public float rocketUpPower = 0.02f;                             // (Rocket) space ���� ��, ���� �ö󰡴� ��
    public float powerJumpPower = 2.2f;                             // PowerJump Object�� �������� ���� jumpPower
    public float airJumpPower = 2f;                                 // AireJump object�� �������� ���� jumpPower

    // PowerJump, AirJump ���� ����                                 
    bool isContactPowerJump = false;                                // PowerJump�� �������� �� ���¸� ��Ÿ��(true: ����, fasle: ����X)
    bool isContactAirJump = false;                                  // AirJump�� �������� �� ���¸� ��Ÿ��(true: ����, flase: ����X)

    // ���� ����                                                    
    float _yVelocity = 0f;                                          // dir�� y�� ���� velocity(gravity, jumPower �ۿ��� ���)
    float yVelocity
    {
        get { return _yVelocity; }
        set
        {
            //print("changeYVelocity : " + _yVelocity);
            if (Mode == ModeState.CUBE || Mode == ModeState.RACE ||
                Mode == ModeState.SATELLITE_horizontal)
            {
                value = Mathf.Clamp(value, -powerJumpPower, powerJumpPower);
            }
            _yVelocity = value;
        }
    }
    float angle = 0f;                                               // dir�� Vector3.forward ������ ����
    bool isContactAB = false;                                       // ��(Above), �Ʒ�(Below)���� ���˿���(true=����/false=���߿� ���ִ� ����)
    bool isContactSides = false;                                    // �¿� ���˿���(true=����/flase=����X)

    // ������Ż ���� ����
    public bool reversGravityState = false;                         // �߷� ���� ����(true: ���߷�(reverse Gravity) , false: �߷�(noraml Gravity)

    // X,Y,Z ��ǥ ��ȭ ������ ���� ����
    bool isChangeYpos;
    float currentXpos;
    float preYpos = 0;
    float currentYpos = 0;
    float preZpos = -1;                                             // Move ���� z��ǥ (Dead ���⿡ Ȱ��)    
    float currentZpos = 0;                                          // Move ���� z��ǥ (Dead ���⿡ Ȱ��)

    // RACE ����� X�� �̵���ǥ ���� ����(x��ġ�� Line���� ����)
    int LineNumber = 1;
    float posX;
    float nextXpos;

    // dir, move�� ����� �Ǵ� ���� ����
    Vector3 dir;
    CharacterController cc;
}
