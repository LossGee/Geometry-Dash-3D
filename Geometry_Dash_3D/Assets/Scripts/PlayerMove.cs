using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 


public class PlayerMove : MonoBehaviour
{
    // Player mode
    enum ModeState { 
        CUBE,
        ROCKET
    }
    ModeState Mode;

    // 공통 변수
    public float moveSpeed = 20f;        
    float gravity = -9.8f;
    float yVelocity = 0f;
    public float jumpPower = 2f;

    Vector3 dir;
    CharacterController cc0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject CC1 = transform.GetChild(1).gameObject;
        GameObject CC2 = transform.GetChild(2).gameObject;

        cc0 = GetComponent<CharacterController>();
        cc1 = CC1.GetComponent<CharacterController>();
        cc2 = CC2.GetComponent<CharacterController>();
        dir = Vector3.forward;
        MotionCube = GameObject.Find("MotionCube");
        MotionRocket = GameObject.Find("MotionRocket");
        Mode = ModeState.CUBE;
        //Mode = ModeState.ROCKET;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mode)
        {
            case ModeState.CUBE: UpdateCube(); break;
            case ModeState.ROCKET: UpdateRocket(); break;
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

    GameObject MotionCube;
    CharacterController cc1;
    CharacterController cc2;

    private void UpdateCube()
    {
        gravity = CubeGravity;       // Cube 모드일때 중력 적용

        // 0. MotionCube 활성화 && MotionRocket 비활성화
        //MotionRocket.SetActive(false);
        //MotionCube.SetActive(true);

        // 1. 바닥과 접촉여부 검사
        // 1-1) 바닥과 접촉인 경우
        if (cc0.isGrounded)
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

        // 3. cc0에 움직입 적용
        cc0.Move(dir * moveSpeed * Time.deltaTime);
        //cc1.Move(dir * moveSpeed * Time.deltaTime);
        //cc2.Move(dir * moveSpeed * Time.deltaTime);

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
    public float RocketGravity = -5.0f;  // Rocket 모드의 중력  
    public float upPower = 1.0f;         // space 누를 때, 위로 올라가는 힘
    float angle = 0f;

    GameObject MotionRocket;

    private void UpdateRocket()
    {
        gravity = RocketGravity;        // RocketGravity 중력 적용

        // 0. MotionCube 비활성화 && MotionRocket 활성화
        //MotionCube.SetActive(false);  //MontionRocket Off & MotinCube On
        //MotionRocket.SetActive(true);

        // 1. Rocket 모드의 dir 구하기
        yVelocity += gravity * Time.deltaTime;      // yVelocity에 garvity 누적 
        if (Input.GetKey(KeyCode.Space))            // space 입력 시, yVelocitydp jumpPower 누적하기(IPointerDownHandler, IPointerUpHandler 사용)
        {
            yVelocity += upPower;
        }
        dir.y = yVelocity;


        // 2. MotionRocket 방향 설정
        //  : dir벡터와 Vector3의 사이각을 구하여 MotionRocket이 진행방향에 따라 방향을 틀도록 설정

        if (cc0.isGrounded)
        {
            angle = Mathf.Lerp(0, angle, 0.9f);         // 자연스럽게 바닥에 착지하는 모션
        }
        else
        {
            angle = Vector3.Angle(Vector3.forward, dir.normalized);
        }

        // Vector3.Angle()은 결과를 절댓값으로 반환하므로 dir.y에 따라 양수, 음수     
        if (dir.y > 0)      
        {
            angle = -angle;
        }
        //print("Angle: " + angle);

        MotionRocket.transform.rotation = Quaternion.Euler(angle+90, 0 ,0);

        // 3. 움직임 적용
        cc0.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 

        
    }

    // (공통) Player 죽음 
    public void Dead()
    {
        Destroy(gameObject);
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
