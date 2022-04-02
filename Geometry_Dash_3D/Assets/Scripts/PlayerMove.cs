using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
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
        CUBE,                                           // 3인칭 측면 CUBE 모드(Geometry Dash)
        UFO,                                            // 3인칭 측면 UFO 모드(Geometry Dash)
        RACE,                                           // 3인칭 정면 상단 아래로 내려보는 RACAE 모드(Beat Racer)
        SATELLITE_vertical,                             // 3인칭 측면 SATELLITE_vertical 모드(Geometry Dash)
        SATELLITE_horizontal,                           // 3인칭 정면 상단 아래로 내려다보는 SATELLITE_horizontal 모드
        ROCKET,                                         // 3인칭 측면 ROCEKT 모드(Geometry Dash)
        FORWARD                                         // 3인칭 정면 직선방향으로 보는 (VectorRush)
    }
    public ModeState Mode;
    bool changeMode = true;                             // Mode가 변화가 있을 때 ture

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionSATELLITE;
    public GameObject MotionRocket;
    public GameObject MotionForward;

    // 중력(gravity) 관련 변수
    float gravity = -9.8f;
    public float DefaultGravity = -9.8f;                 // Cube, UFO 모드의 중력
    public float RaceGravity = -9.8f;                    // Race 모드의 중력
    public float RocketGravity = -2f;                    // Rocket 모드의 중력  

    // 진행방향으로 움직이는 속도(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;                    // (Cube) 전진 이동속도
    public float ufoMoveSpeed = 20f;                     // (UFO) 전진 이동속도
    public float raceMoveSpeed = 20f;                    // (Race) 전진 이동속도
    public float raceSidemoveSpeed = 3f;                 // (Race) 좌우방향 이동 속도
    public float SATELLITE_verticalMoveSpeed = 20f;      // (SATELLITE_vertical) 전진 이동속도
    public float SATELLITE_horizontalMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;                  // (Rocket) 전진 이동속도
    public float forwardMoveSpeed = 20f;                 // (Forwardk) 전진 이동 속도

    // Motion 속도 관련 변수(TureSpeed)
    public float cubeTurnSpeed = 500f;                   // MotionCube 회전 속도

    // Jump Power(y축 +방향으로 가해지는 힘)에 관련된 변수
    float jumpPower = 2f;
    public float cubeJumpPower = 2f;                     // (Cube) jump power 
    public float ufoJumpPower = 2f;                      // (UFO) jump power
    public float raceJumpPower = 2f;
    public float rocketUpPower = 0.02f;                  // (Rocket) space 누를 때, 위로 올라가는 힘
    public float powerJumpPower = 2.2f;                  // PowerJump Object와 접촉했을 때의 jumpPower
    public float airJumpPower = 2f;                      // AireJump object와 접촉헀을 때의 jumpPower

    // PowerJump, AirJump 관련 변수                      
    bool isContactPowerJump = false;                     // PowerJump와 접촉했을 때 상태를 나타냄(true: 접촉, fasle: 접촉X)
    bool isContactAirJump = false;                       // AirJump와 접촉했을 때 상태를 나타냄(true: 접촉, flase: 접촉X)

    // 공통 변수                                         
    float yVelocity = 0f;                                // dir의 y축 방향 velocity(gravity, jumPower 작용이 결과)
    float preZpos;                                       // Move 이전 z좌표 (Dead 검출에 활용)    
    float currentZpos;                                   // Move 이후 z좌표 (Dead 검출에 활용)
    float angle = 0f;                                    // dir과 Vector3.forward 사이의 각도
    bool isContactAB = false;                            // 위(Above), 아래(Below)와의 접촉여부(true=접촉/false=공중에 떠있는 상태)
    bool isContactSides = false;                         // 좌우 접촉여부(true=접촉/flase=접촉X)

    // 반전포탈 관련 변수
    public bool reverseGravity = false;                  // 중력반전여부(true: 중력적용 , false: reverse)

    // RACE 모드의 이동좌표 관련 변수(x위치 고정, RacePos0~3)
    int Linenumber = 1;
    public GameObject[] racePos = new GameObject[4];     // Race Line 4
    Transform nowPos;                                    // 현재 Player가 위치한 Line의 위치

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
        if (changeMode)
        {
            SetMotion();                                 // Mode별 Mode 담당 GameObject 활성화
            SetVar();                                    // Mode별 변수(variable) 설정
            changeMode = false;
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
        Dead();
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
                jumpPower = raceJumpPower;
                nowPos = transform;
                SetRaceLine();
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
    bool dead = false;                                    // daed 상태 표시 변수(true: 죽음)
    public void Dead()                                    // Player 죽음 처리 함수 
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
        // Dead 결과 실행(dead == true일 때): Play 다시 시작
        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(0);                   // 다시시작
        }
    }
    // (공통) 장애물 충돌시 dead true 처리
    public void CrashObstacle()                          // 장애물에 부딪혔을 때
    {
        dead = true;
    }


    // [Mode: Cube]
    // Player(Cube) 변수
    bool jumpState = false;                               // jump 상태(ture: 점프중, false: 점프X) - 이단 점프 방지
    bool jumpTurn = true;                                 // jump시 회전을 180도로 제한하기 위한 변수
    bool dropTurn = true;                                 // drop시 회전을 90도로 제한하기 위한 변수
    float rot = 0f;                                       // jump에서 공중회전 각도 누적 변수
    private void UpdateCube()
    {
        gravity = DefaultGravity;                         // Cube 모드일때 중력 적용

        // 1. 천장 or 바닥과 접촉여부 검사(상하중력 전환인 경우 포함)
        // cc의 위(Above), 아래(Below) 충돌 여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
        // 1-1) 바닥과 접촉인 경우
        if (isContactAB)
        {
            yVelocity = 0;      // 중력 누적 X
            jumpState = false;
            jumpTurn = false;
            dropTurn = false;
            rot = 0;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);      //  바닥에 닿아있을 때는 rotation 고정
        }
        // 1-2) 바닥과 접촉하지 않은 경우
        else
        {
            yVelocity += gravity * Time.deltaTime;          // yVelocity에 중력 적용

            if (jumpState && !jumpTurn)
            {
                JumpTurn();                                 // jump시, MotionCube 180도 회전
            }
            if (!dropTurn)
            {
                DropTurn();                                 // 낙하시, MotionCube 90도 회전
            }
        }
        // 2. jump 기능 구현
        // : space키 입력 && 점프하고 있는 상황이 아니라면
        if (isContactPowerJump)
        {
            PowerJump();
        }
        else if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumpState = true;
            yVelocity = jumpPower;                          // yVeleocity에 cubeJumpPower 적용
        }
        if (isContactAirJump)
        {
            AirJump();
        }
        dir.y = yVelocity;                                  // dir.y에 yVelocity 적용

        // 3. cc에 움직입 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;

    }
    // (Cube) jump시 MotinoCube 180도 회전 모션 함수
    void JumpTurn()
    {
        rot += cubeTurnSpeed * Time.deltaTime;
        if (rot < 180)
        {
            MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        }
        else
        {
            jumpTurn = true;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    // (Cube) drop시 MotinoCube 90도 회전 모션 함수
    void DropTurn()
    {
        rot += cubeTurnSpeed * Time.deltaTime;
        if (rot < 90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        else dropTurn = true;
    }
    // (Cube & RACE) 공중 점프 Intaraction과 접촉했을 때, 2단 점프 허용
    void AirJump()
    {
        if (Input.GetButtonDown("Jump") && jumpState)
        {
            if (Mode == ModeState.CUBE)
            {
                jumpTurn = false;
                rot = 0;
            }
            yVelocity = airJumpPower;
        }
    }
    // (Cube & Race) AirJump Interaction과 접촉했을 때, isContactAirJump를 true로 바꿔주는 함수
    public void OnAirJump()
    {
        isContactAirJump = true;
    }
    // (Cube & Race) AirJump Instaraction과 접촉이 끝났을 때, isContactAirJump를 false로 바꿔주는 함수
    public void OffAirJump()
    {
        isContactAirJump = false;
    }
    // (CUBE & UFO & RACE) 파워점프대를 만났을 때, 보통 점프보다 1.5배 높이로 점프한다. 
    void PowerJump()
    {
        jumpState = true;
        yVelocity = powerJumpPower;
        isContactPowerJump = false;
    }
    // (Cube & UFO & Race) PowerJump Interaction과 접촉했을 때, isContactPowerJump를 true로 바꿔주는 함수
    public void ContactPowerJump()
    {
        isContactPowerJump = true;
    }
    // 

    // (Cube & UFO & Race) 중력반전 실행
    void ReversGravity()
    {

    }


    // [Mode: UFO]
    private void UpdateUFO()
    {
        // 1. 천장(Above)과 바닥(Below) 접촉여부 검사
        // cc의 위(Above), 아래(Below) 충돌 여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        // 1-1) 바닥(Below)과 접촉한 경우 
        if (((cc.collisionFlags & CollisionFlags.Below) != 0))
        {
            yVelocity = 0;
        }
        yVelocity += gravity * Time.deltaTime;
        // 2. jump 기능 구현
        // 2-1) PowereJump 발판을 밟았을 때
        if (isContactPowerJump)
        {
            PowerJump();
        }
        // 2-2) Space 키를 입력했을 때 (공중 연속점프 가능)
        if (Input.GetKeyDown(KeyCode.Space)
            && ((cc.collisionFlags & CollisionFlags.Above) == 0))
        {
            yVelocity = ufoJumpPower;
        }
        dir.y = yVelocity;
        // 3. Motion 적용 
        UFOMotion();
        // 4. 움직임 적용 
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
    // (UFO) jump시 UFO Motino 기울기 변화 모션
    void UFOMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir) / 3;
        if (dir.y >= 0)
        {
            MotionUFO.transform.rotation = Quaternion.Euler(angle, 0, 0);
        }
    }


    // [Mode: Race]
    private void UpdateRace()
    {
        //현재위치에서 가장 가까운 라인으로 이동 

        // 1. Race 모드에서의 dir 정하기(Line 위치로 제어하기) 
        // 1) 방향키 입력에 따른 Line(racePos) 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Linenumber > 0)
            {
                Linenumber--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Linenumber < 3)
            {
                Linenumber++;
            }
        }
        print(Linenumber);
        dir.x = racePos[Linenumber].transform.position.x - nowPos.transform.position.x;
        //  2) 중력 적용
        // 천장or지면과 접촉여부 확인 및 yVelocity 적용
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        if (isContactAB)
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }
        dir.y = yVelocity;

        // PowerJump 접촉 여부 확인 및 실행
        if (isContactPowerJump)
        {
            PowerJump();
        }

        //  3) 모션 적용
        RaceMotion();

        // 2. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
    // (Race) 좌우이동시 Race Motion의 좌우 방향 모션
    private void RaceMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.x > 0)
        {
            angle = -angle;
        }
        MotionRace.transform.rotation = Quaternion.Euler(-90, -90, -90);
    }
    // (Race) 현재위치에 따라 어느 Race Line에 있는지 검사
    private void SetRaceLine()
    {
        float posX = nowPos.position.x;

        if ((posX >= -4.8f) && (posX < -2.4f))
        {
            Linenumber = 0;
        }
        else if ((posX >= -2.4f) && (posX < 0f))
        {
            Linenumber = 1;
        }
        else if ((posX >= 0f) && (posX < 2.4f))
        {
            Linenumber = 2;
        }
        else if ((posX >= -2.4f) && (posX <= 4.8f))
        {
            Linenumber = 3;
        }
    }


    // [Mode: SATELLITE_vertical]
    bool SATELLITE_verticalUpDownState = true;              // SATELLITE_vertical의 위/아래 진행상태(true:우상향, false:우하향) 
    private void UpdateSATELLITE_vertical()
    {
        // 1. 사용자 입력
        if (Input.GetKeyDown(KeyCode.Space))                // 사용자의 Space키 입력에 따라 방향 전환
        {
            SATELLITE_verticalUpDownState = !SATELLITE_verticalUpDownState;
        }
        // 2. dir 적용
        if (SATELLITE_verticalUpDownState)
        {
            dir = new Vector3(0, 1, 1);                     // 우상향 45도 방향
        }
        else
        {
            dir = new Vector3(0, -1, 1);                    // 우하향 45도 방향
        }
        // 3. SATELLITE_vertical 모션 적용
        SATELLITE_verticalMotion();
        // 4. 움직임 적용
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_vertical) SATELLITE_verticalMotion의 모션 구현
    void SATELLITE_verticalMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.y > 0)
        {
            angle = -angle;
        }
        Quaternion moveAngle = Quaternion.Euler(90 + angle, 0, 0);
        MotionSATELLITE.transform.rotation = Quaternion.Lerp(MotionSATELLITE.transform.rotation, moveAngle, 0.5f);
    }


    // [Mode: SATELLITE_horizontal]
    bool SATELLITE_horizontalRightLeftState;
    private void UpdateSATELLITE_horizontal()
    {
        // 1. 사용자 입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SATELLITE_horizontalRightLeftState = !SATELLITE_horizontalRightLeftState;
        }

        // 2. dir 적용
        // 2-1) 진행방향 결정
        if (SATELLITE_horizontalRightLeftState)
        {
            dir = new Vector3(1, 0, 1);
        }
        else
        {
            dir = new Vector3(-1, 0, 1);
        }
        // 2-2) 중력 적용
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        if (isContactAB)
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }
        dir.y = yVelocity;

        // 3. SATELLITE_horizontal 모션 적용
        SATELLITE_horizontalMotion();

        // 4. 움직임 적용
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_horizontal) SATELLITE_horizontalMotion 모션 구현
    public void SATELLITE_horizontalMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.x < 0)
        {
            angle = -angle;
        }
        print(angle);
        Quaternion moveAngle = Quaternion.Euler(90, angle, 0);
        MotionSATELLITE.transform.rotation = Quaternion.Lerp(MotionSATELLITE.transform.rotation, moveAngle, 0.1f);
    }


    // [Mode: Rocket]
    // Player(Locket) 변수
    private void UpdateRocket()
    {
        // 중력적용
        gravity = RocketGravity;                            // RocketGravity 중력을 gravity에 적용
        // cc의 위(Above), 아래(Below) 충돌 여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);

        // 1. Rocket 모드의 dir 구하기
        yVelocity += gravity * Time.deltaTime;              // yVelocity에 garvity 누적 
        if (Input.GetKey(KeyCode.Space))                    // space 입력 시, yVelocitydp rocketUpPower 누적하기
        {
            yVelocity += rocketUpPower;
            if (isContactAB)
            {
                yVelocity = Mathf.Clamp(yVelocity, -0.1f, 0.1f);
            }
        }
        dir.y = yVelocity;

        // 2. MotionRocket 방향 설정
        //  : dir벡터와 Vector3의 사이각을 구하여 MotionRocket이 진행방향에 따라 방향을 향하도록 설정
        if (isContactAB)                                    // 천장 or 바닥에 접촉한 경우
        {
            angle = Mathf.Lerp(0, angle, 0.7f);             // 자연스럽게 바닥에 착지하는 모션을 위한 선형보간
        }
        else                                                // 공중에 위치한 경우
        {
            angle = Vector3.Angle(Vector3.forward, dir.normalized);
        }
        // Vector3.Angle()은 결과를 절댓값으로 반환하므로 dir.y에 따라 양수, 음수     
        if (dir.y > 0)
        {
            angle = -angle;
        }
        //print("Angle: " + angle);

        MotionRocket.transform.rotation = Quaternion.Euler(angle + 90, 0, 0);

        // 3. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }


    // [Mode: Forward]
    private void UpdateForward()
    {
        // 1. Forward 모드에서의 dir 결정
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir += Vector3.up * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir -= Vector3.up * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir -= Vector3.right * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector3.right * Time.deltaTime;
        }
        else
        {
            dir = Vector3.forward;
        }
        dir.Normalize();

        // 2. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
        currentZpos = transform.position.z;
    }


    // Vector 시각화 code
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + dir;
        Gizmos.DrawLine(from, to);
    }
}
