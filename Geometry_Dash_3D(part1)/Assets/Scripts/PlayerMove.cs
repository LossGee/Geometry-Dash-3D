using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

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
    bool changeMode = true;                                      // Mode가 변화가 있을 때 ture

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionSATELLITE;
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

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        // Dead 결과 실행(dead == true일 때): Play 다시 시작
        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(0);                        // 다시시작
        }
        if (changeMode)
        {
            SetMotion();                                             // Mode별 Mode 담당 GameObject 활성화
            SetVar();                                                // Mode별 변수(variable) 설정
            changeMode = false;
        }
        Dead();                                                      // Player의 죽음 조건 검사

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

    }
    // (공통) Mode 전환 Potal을 만날 경우, changeMode를 true로 설정해주는 함수 
    public void ChangeMode()
    {
        changeMode = true;
    }
    // (공통) Mode 전환 함수: Motion을 담당하는 객체를 활성화/비활성화
    void SetMotion()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                MotionCube.SetActive(true);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.UFO:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(true);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(true);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.SATELLITE_vertical:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.SATELLITE_horizontal:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
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
    // (공통) Player 죽음 처리 함수
    bool dead = false;                                          // daed 상태 표시 변수(true: 죽음)
    public void Dead()                                          // Player 죽음 처리 함수 
    {
        // Dead 조건(공통): 벽에 부딪혔을 때(Vector3.forward 방향으로 좌표변화가 없을 때)
        if (currentZpos <= preZpos)
            dead = true;
        // Dead 조건(추가, SATELLITE_vertical): 위/아래가 접촉되었을 때
        if (Mode == ModeState.SATELLITE_vertical)
        {
            // 위/아래 접촉 여부 검사 
            isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
            // 접촉시 dead = true
            if (isContactAB)
            {
                dead = true;
            }
        }
        // Dead 조건(추가, SATELLITE_horizotal): 좌우 벽에 접촉되었을 때
        if (Mode == ModeState.SATELLITE_horizontal)
        {
            // 좌우 접촉 여부 확인
            isContactSides = (cc.collisionFlags & CollisionFlags.Sides) != 0;
            // 접촉시 dead = true
            if (isContactSides)
            {
                dead = true;
            }
        }
    }
    // (공통) 장애물 충돌시 dead true 처리
    public void CrashObstacle()                                 // 장애물에 부딪혔을 때
    {
        dead = true;
    }
    // (Cube) AirJump Interaction과 접촉했을 때, isContactAirJump를 true로 바꿔주는 함수
    public void OnAirJump()
    {
        isContactAirJump = true;
    }
    // (Cube) AirJump Instaraction과 접촉이 끝났을 때, isContactAirJump를 false로 바꿔주는 함수
    public void OffAirJump()
    {
        isContactAirJump = false;
    }
    // (CUBE & UFO & RACE) 파워점프대를 만났을 때, 보통 점프보다 1.5배 높이로 점프한다. 
    void PowerJump()
    {
        print("PowerJump");
        yVelocity = powerJumpPower;
        isContactPowerJump = false;
    }
    // (Cube & UFO & Race) PowerJump Interaction과 접촉했을 때, isContactPowerJump를 true로 바꿔주는 함수
    public void ContactPowerJump()
    {
        isContactPowerJump = true;
    }
    // (ube & UFO & Race & Satellite & Rocket) 중력반전 포탈에 접촉했을 때 중력반전상태(ReversGravityState)를 반전시켜주는 함수
    public void ChangeReverseGravityState()
    {
        reversGravityState = !reversGravityState;
    }
    // (Cube & UFO & Race & Satellite & Rocket) 중력반전 실행
    void ReverseGravity()
    {
        if (reversGravityState)
        {
            dir.y = -yVelocity;
        }
        else
        {
            dir.y = yVelocity;
        }
    }

    // Vector 시각화 code
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + (reversGravityState ? Vector3.down : Vector3.up) * yVelocity;
        Gizmos.DrawLine(from, to);
    }
}
