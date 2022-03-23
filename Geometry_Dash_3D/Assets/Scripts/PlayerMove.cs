using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
<구현 기능>
step1) Player를 Vector3.forward 방향으로 전진
step2) Character Controller를 활용하여 jump 기능 구현
step3) Jump시 MotionCube의 
    
 */
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

    Vector3 dir;
    CharacterController cc;

    // Palyer(Cube) 변수
    public float jumpPower = 2f;
    bool jumpState = false;         // jump 상태(ture: 점프중 // false: 점프X) - 이단 점프 방지
    bool jumpTurn = true;           // jump시 회전을 180도로 제한하기 위한 변수
    bool dropTurn = true;           // drop시 회전을 90도로 제한하기 위한 변수
    public float turnSpeed = 500f;  // 회전 모션 속도
    float rot = 0f;                 // jump에서 공중회전 각도 누적 변수
    
    GameObject MotionCube;

    // Player(Locket) 변수
    float AntiGravity = 8.0f;       // Rocket Moded에서 중력에 반대방향으로 동작하는 힘 (일단 public으로 하고 조절하며 값 알아보기)

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;
        MotionCube = GameObject.Find("MotionCube");
        Mode = ModeState.CUBE;
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

    // Mode: Cube일 때
    private void UpdateCube()
    {
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

        // 3. cc에 움직입 적용
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // Mode: Player일때 동작
    private void UpdateRocket()
    {
        MotionCube.SetActive(false);


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

    public void Dead()
    {
        Destroy(gameObject);
    }

    // yVelocity 시각화 code
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + Vector3.up * yVelocity;
        Gizmos.DrawLine(from, to);
    }*/


}
