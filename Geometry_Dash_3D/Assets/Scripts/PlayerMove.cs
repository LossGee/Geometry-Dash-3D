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
        CUBE,           // 3인칭 측면 CUBE 모드(Geometry Dash)
        UFO,            // 3인칭 측면 UFO 모드(Geometry Dash)
        RACE,           // 3인칭 정면 상단 아래로 45도 내려보는 RACAE 모드(Beat Racer)
        ROCKET,         // 3인칭 측면 ROCEKT 모드(Geometry Dash)
        FORWARD         // 3인칭 정면 직선방향으로 보는 (VectorRush)
    }
    public ModeState Mode;
    bool changeMode = false;     // Mode가 변화가 있을 때 ture

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionRocket;
    public GameObject MotionForward;
    public float cubeTurnSpeed = 500f;      // 회전 모션 속도

    // 중력(gravity) 관련 변수
    float gravity = -9.8f;
    public float DefaultGravity = -9.8f;     // Cube, UFO 모드의 중력
    public float RaceGravity = -9.8f;        // Race 모드의 중력
    public float RocketGravity = -2f;        // Rocket 모드의 중력  

    // 진행방향으로 움직이는 속도(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;
    public float ufoMoveSpeed = 20f;
    public float raceMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;
    public float forwardMoveSpeed = 20f;

    // y축 +방향으로 가해지는 힘에 관련된 변수
    public float cubeJumpPower = 2f;        // (Cube) jump power
    public float ufoJumpPower = 2f;         // (UFO) jump power
    public float rocketUpPower = 0.02f;     // (Rocket) space 누를 때, 위로 올라가는 힘
    public float powerJumpPower = 2.2f;          // PowerJump Object에 접촉했을 때의 jump power
    public float airJumpPower = 2f;

    // PowerJump, AirJump 관련 변수
    bool isContactPowerJump = false;        // PowerJump와 접촉했을 때 상태를 나타냄(true: 접촉, fasle: 접촉X)
    bool isContactAirJump = false;          // AirJump와 접촉했을 때 상태를 나타냄(true: 접촉, flase: 접촉X)

    // x축 방향으로 가해지는 힘에 관련된 변수
    public float RaceSidemoveSpeed = 3f;    // (Race) 좌우방향 이동 속도

    // 공통 변수
    float yVelocity = 0f;
    float preZpos;                          // Move 이전 z좌표    
    float currentZpos;                      // Move 이후 z좌표
    float angle = 0f;                       // dir과 Vector3.forward 사이의 각도
    bool isContactAB = false;               // 위(Above), 아래(Below)와의 접촉여부(true=접촉/false=공중에 떠있는 상태)

    // 반전포탈 관련 변수
    public bool reverseGravity = false;            // 중력반전여부(true: 중력적용 , false: reverse)
    

    Vector3 dir;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;

        SetMotion();
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
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.UFO:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(true);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(true);
                break;
        }
    }
    // (공통) SetVar: 모드별 각 변수값 설정 [중력(gravity), 진행속도(Speed), 
    //void SetVar()
    //{
    //    switch (Mode)
    //    {
    //        case ModeState.CUBE:

    //            break;
    //        case ModeState.UFO:

    //            break;
    //        case ModeState.RACE:

    //            break;
    //        case ModeState.ROCKET:

    //            break;
    //        case ModeState.FORWARD:

    //            break;
    //    }
    //}

    // (공통) 장애물 충돌시 dead true 처리
    bool dead = false;               // daed 상태 표시 변수(true: 죽음)
    public void CrashObstacle()      // 장애물에 부딪혔을 때
    {
        dead = true;
    }
    // (공통) Player 죽음 
    public void Dead()               // Player 죽음 처리 함수 
    {
        if (currentZpos <= preZpos)         // 벽에 부딪혔을 때 Dead
            dead = true;

        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(0);       // 다시시작
        }
    }

    // (공통) PowerJump Interaction과 접촉했을 때, isContactPowerJump를 true로 바꿔주는 함수
    public void ContactPowerJump()
    {
        isContactPowerJump = true;
    }
    // (공통) AirJump Interaction과 접촉했을 때, isContactAirJump를 true로 바꿔주는 함수
    public void OnAirJump()
    {
        isContactAirJump = true;
    }
    public void OffAirJump()
    {
        isContactAirJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMode)
        {
            SetMotion();                        // Mode별 Mode 담당 GameObject 활성화
            //SetVar();                           // Mode별 변수(variable) 설정
            changeMode = false;
        }

        switch (Mode)
        {
            case ModeState.CUBE:UpdateCube(); break;
            case ModeState.UFO:UpdateUFO(); break;
            case ModeState.RACE:UpdateRace(); break;
            case ModeState.ROCKET:UpdateRocket(); break;
            case ModeState.FORWARD:UpdateForward(); break;
        }
        Dead();
    }

    // [Mode: Cube일 때]
    // Player(Cube) 변수
    bool jumpState = false;             // jump 상태(ture: 점프중 // false: 점프X) - 이단 점프 방지
    bool jumpTurn = true;               // jump시 회전을 180도로 제한하기 위한 변수
    bool dropTurn = true;               // drop시 회전을 90도로 제한하기 위한 변수
    float rot = 0f;                     // jump에서 공중회전 각도 누적 변수

    private void UpdateCube()
    {
        gravity = DefaultGravity;       // Cube 모드일때 중력 적용

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
            yVelocity += gravity * Time.deltaTime;      // yVelocity에 중력 적용

            if (jumpState && !jumpTurn)
            {
                JumpTurn();     // jump시, MotionCube 180도 회전
            }
            if (!dropTurn)
            {
                DropTurn();     // 낙하시, MotionCube 90도 회전
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
            yVelocity = cubeJumpPower;      // yVeleocity에 cubeJumpPower 적용
        }
        if (isContactAirJump)
        {
            AirJump();
        }
        dir.y = yVelocity;              // dir.y에 yVelocity 적용

        // 3. cc에 움직입 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
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

    // (CUBE & UFO & RACE) 파워점프대를 만났을 때, 보통 점프보다 1.5배 높이로 점프한다. 
    void PowerJump()
    {
        jumpState = true;
        yVelocity = powerJumpPower;
        isContactPowerJump = false;
    }
    // (Cube & UFO) 공중 점프 Intaraction과 접촉했을 때, 2단 점프 허용
    void AirJump()
    {
        if (Input.GetButtonDown("Jump") && jumpState)
        {
            jumpTurn = false;
            yVelocity = airJumpPower;
            rot = 0;
        }
    }
    // (Cube & UFO) 중력 반전 Potal과 접촉했을 때, 중력반전 
    void ReversGravity()
    {
        
    }


    // [Mode: UFO일 때]
    private void UpdateUFO()
    {
        // 중력 적용 
        gravity = DefaultGravity;

        // 1. 천장(Above)과 바닥(Below) 접촉여부 검사
        // cc의 위(Above), 아래(Below) 충돌 여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        //isYposChange = 
        
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
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
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


    // [Mode: Race일 때]
    private void UpdateRace()
    {
        // 중력적용
        gravity = RaceGravity;

        // 1. Race 모드에서의 dir 정하기
        // 0) PowerJump 접촉 여부 확인 및 실행
        if (isContactPowerJump)
        {
            PowerJump();
        }
        // 1) 방향키에 따른 좌우 이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x -= RaceSidemoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir.x += RaceSidemoveSpeed * Time.deltaTime;
        }
        else
        {
            dir.x = Mathf.Lerp(dir.x, 0, 0.1f);
        }

        //  2) 중력 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 2. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
        currentZpos = transform.position.z;
    }


    // [Mode: Rocket일 때]
    // Player(Locket) 변수
    private void UpdateRocket()
    {

        // 중력적용
        gravity = RocketGravity;                    // RocketGravity 중력을 gravity에 적용
        // cc의 위(Above), 아래(Below) 충돌 여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);

        // 1. Rocket 모드의 dir 구하기
        yVelocity += gravity * Time.deltaTime;      // yVelocity에 garvity 누적 
        if (Input.GetKey(KeyCode.Space))            // space 입력 시, yVelocitydp rocketUpPower 누적하기
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
        if (isContactAB)                            // 천장 or 바닥에 접촉한 경우
        {
            angle = Mathf.Lerp(0, angle, 0.7f);     // 자연스럽게 바닥에 착지하는 모션을 위한 선형보간
        }
        else                                        // 공중에 위치한 경우
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
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
        currentZpos = transform.position.z;

        // 4. Dead
        //    : Player의 z좌표가 현재위치가 전위치보다 같거나 작으면 Dead

    }


    // [Mode: Forward일 때]
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
