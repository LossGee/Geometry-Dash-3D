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
        ROCKET,         // 3인칭 측면 ROCEKT 모드(Geometry Dash)
        RACE,           // 3인칭 정면 상단 아래로 45도 내려보는 RACAE 모드(Beat Racer)
        FORWARD         // 3인칭 정면 직선방향으로 보는 (VectorRush)
    }
    public ModeState Mode;
    public bool changeMode = false;     // Mode가 변화가 있을 때 ture

    // 공통 변수
    public float moveSpeed = 20f;
    float gravity = -9.8f;
    float yVelocity = 0f;
    public float jumpPower = 2f;
    float preZpos;                  // Move 이전 z좌표    
    float currentZpos;              // Move 이후 z좌표
    float xPos = 0;
    float yPos = 0;
    float zPos = 0;


    Vector3 dir;
    CharacterController cc;

    // Start is called before the first frame update

    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;


        SetMode();
    }

    // (공통) Mode 전환 함수: Motion을 담당하는 객체를 활성화/비활성화
    void SetMode()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                MotionCube.SetActive(true);
                MotionRocket.SetActive(false);
                MotionRace.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionRocket.SetActive(true);
                MotionRace.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionRocket.SetActive(false);
                MotionRace.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionRocket.SetActive(false);
                MotionRace.SetActive(false);
                MotionForward.SetActive(true);
                break;

        }
        changeMode = true;
    }
    // (공통) Player 죽음 
    public void Dead()
    {
        if (currentZpos <= preZpos)
            SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMode)
        {
            SetMode();
        }

        switch (Mode)
        {
            case ModeState.CUBE:
                UpdateCube();
                Dead();
                break;
            case ModeState.ROCKET:
                UpdateRocket();
                Dead();
                break;
            case ModeState.RACE:
                UpdateRace();
                Dead();
                break;
            case ModeState.FORWARD:
                UpdateForward();
                Dead();
                break;
        }
    }

    // [Mode: Cube일 때]
    // Player(Cube) 변수
    public float CubeGravity = -9.8f;    // Cube 모드의 중력
    bool jumpState = false;         // jump 상태(ture: 점프중 // false: 점프X) - 이단 점프 방지
    bool jumpTurn = true;           // jump시 회전을 180도로 제한하기 위한 변수
    bool dropTurn = true;           // drop시 회전을 90도로 제한하기 위한 변수
    public float turnSpeed = 500f;  // 회전 모션 속도
    float rot = 0f;                 // jump에서 공중회전 각도 누적 변수

    public GameObject MotionCube;

    private void UpdateCube()
    {
        gravity = CubeGravity;       // Cube 모드일때 중력 적용

        // 0. MotionCube 활성화 && MotionRocket 비활성화
        //MotionRocket.SetActive(false);
        //MotionCube.SetActive(true);

        // 1. 바닥과 접촉여부 검사
        // 1-1) 바닥과 접촉인 경우
        if (cc.isGrounded)
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
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumpState = true;
            yVelocity = jumpPower;      // yVeleocity에 jumpPower 적용
        }
        dir.y = yVelocity;              // dir.y에 yVelocity 적용

        // 3. cc에 움직입 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
        currentZpos = transform.position.z;

    }
    // (Cube) jump시 MotinoCube 180도 회전 모션 함수
    public void JumpTurn()
    {
        rot += turnSpeed * Time.deltaTime;
        if (rot < 180) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        else jumpTurn = true;
    }
    // (Cube) drop시 MotinoCube 90도 회전 모션 함수
    public void DropTurn()
    {
        rot += turnSpeed * Time.deltaTime;
        if (rot < 90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        else dropTurn = true;
    }


    // [Mode: Rocket일 때]
    // Player(Locket) 변수
    public float RocketGravity = -2f;   // Rocket 모드의 중력  
    public float upPower = 0.02f;       // space 누를 때, 위로 올라가는 힘
    float angle = 0f;                   // dir과 Vector3.forward 사이의 각도
    bool isContactAB = false;           // 위(Above), 아래(Below)와의 접촉여부(true=접촉/false=공중에 떠있는 상태)

    public GameObject MotionRocket;

    private void UpdateRocket()
    {

        // 중력적용
        gravity = RocketGravity;                    // RocketGravity 중력을 gravity에 적용
        // cc의 위(Above), 아래(Below) 충돌 여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)     
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
        // 1. Rocket 모드의 dir 구하기
        yVelocity += gravity * Time.deltaTime;      // yVelocity에 garvity 누적 
        if (Input.GetKey(KeyCode.Space))            // space 입력 시, yVelocitydp jumpPower 누적하기(IPointerDownHandler, IPointerUpHandler 사용)
        {
            yVelocity += upPower;
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

    // [Mode: Race일 때]
    public float sidemoveSpeed = 3f;

    public GameObject MotionRace;
    private void UpdateRace()
    {
        // 중력적용
        gravity = CubeGravity;

        // 1. Race 모드에서의 dir 하기
        // 1) 방향키에 따른 좌우 이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x -= sidemoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir.x += sidemoveSpeed * Time.deltaTime;
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

    // [Mode: Forward일 때]
    public GameObject MotionForward;
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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Vector3 from = transform.position;
    //    Vector3 to = transform.position + dir;
    //    Gizmos.DrawLine(from, to);
    //}


}
