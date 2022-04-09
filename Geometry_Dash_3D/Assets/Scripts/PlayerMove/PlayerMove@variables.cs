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
        CUBE,                                                    // 3인칭 측면 CUBE 모드(Geometry Dash)
        UFO,                                                     // 3인칭 측면 UFO 모드(Geometry Dash)
        RACE,                                                    // 3인칭 정면 상단 아래로 내려보는 RACAE 모드(Beat Racer)
        SATELLITE_vertical,                                      // 3인칭 측면 SATELLITE_vertical 모드(Geometry Dash)
        SATELLITE_horizontal,                                    // 3인칭 정면 상단 아래로 내려다보는 SATELLITE_horizontal 모드
        ROCKET,                                                  // 3인칭 측면 ROCEKT 모드(Geometry Dash)
        FORWARD                                                  // 3인칭 정면 직선방향으로 보는 (VectorRush)
    }
    public ModeState Mode;
    bool changePlayMode = true;                                      // Mode가 변화가 있을 때 ture

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

    // 중력(gravity) 관련 변수
    float gravity;
    public float DefaultGravity = -9.8f;                            // Cube, UFO 모드의 중력
    public float RocketGravity = -2f;                               // Rocket 모드의 중력  

    // 진행방향으로 움직이는 속도(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;                               // (Cube) 전진 이동속도
    public float ufoMoveSpeed = 20f;                                // (UFO) 전진 이동속도
    public float raceMoveSpeed = 20f;                               // (Race) 전진 이동속도
    public float raceSidemoveSpeed = 3f;                            // (Race) 좌우방향 이동 속도
    public float SATELLITE_verticalMoveSpeed = 20f;                 // (SATELLITE_vertical) 전진 이동속도
    public float SATELLITE_horizontalMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;                             // (Rocket) 전진 이동속도
    public float forwardMoveSpeed = 20f;                            // (Forwardk) 전진 이동 속도

    // Motion 속도 관련 변수(TureSpeed)
    public float cubeTurnSpeed = 500f;                              // MotionCube 회전 속도

    // Jump Power(y축 +방향으로 가해지는 힘)에 관련된 변수
    float jumpPower = 2f;
    public float cubeJumpPower = 2f;                                // (Cube) jump power 
    public float ufoJumpPower = 2f;                                 // (UFO) jump power
    public float rocketUpPower = 0.02f;                             // (Rocket) space 누를 때, 위로 올라가는 힘
    public float powerJumpPower = 2.2f;                             // PowerJump Object와 접촉했을 때의 jumpPower
    public float airJumpPower = 2f;                                 // AireJump object와 접촉헀을 때의 jumpPower

    // PowerJump, AirJump 관련 변수                                 
    bool isContactPowerJump = false;                                // PowerJump와 접촉했을 때 상태를 나타냄(true: 접촉, fasle: 접촉X)
    bool isContactAirJump = false;                                  // AirJump와 접촉했을 때 상태를 나타냄(true: 접촉, flase: 접촉X)

    // 공통 변수                                                    
    float _yVelocity = 0f;                                          // dir의 y축 방향 velocity(gravity, jumPower 작용이 결과)
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
    float angle = 0f;                                               // dir과 Vector3.forward 사이의 각도
    bool isContactAB = false;                                       // 위(Above), 아래(Below)와의 접촉여부(true=접촉/false=공중에 떠있는 상태)
    bool isContactSides = false;                                    // 좌우 접촉여부(true=접촉/flase=접촉X)

    // 반전포탈 관련 변수
    public bool reversGravityState = false;                         // 중력 반전 여부(true: 역중력(reverse Gravity) , false: 중력(noraml Gravity)

    // X,Y,Z 좌표 변화 감지를 위한 변수
    bool isChangeYpos;
    float currentXpos;
    float preYpos = 0;
    float currentYpos = 0;
    float preZpos = -1;                                             // Move 이전 z좌표 (Dead 검출에 활용)    
    float currentZpos = 0;                                          // Move 이후 z좌표 (Dead 검출에 활용)

    // RACE 모드의 X축 이동좌표 관련 변수(x위치를 Line별로 고정)
    int LineNumber = 1;
    float posX;
    float nextXpos;

    // dir, move의 대상이 되는 변수 모음
    Vector3 dir;
    CharacterController cc;
}
