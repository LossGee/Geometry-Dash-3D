using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_Rig : MonoBehaviour
{
    // 공통
    public float moveSpeed = 20f;
    float gravity;

    enum ModeState
    {
        CUBE,
        ROCKET
    }
    ModeState Mode;
    Vector3 dir;
    Rigidbody rig;


    // Player(Cube) 관련 변수
    public float jumpPower = 1000f;
    public float GravityCube = -9.8f;
    bool jumpState = false;         // jump 상태(ture: 점프중 // false: 점프X) - 이단 점프 방지
    bool jumpTurn = true;           // jump시 회전을 180도로 제한하기 위한 변수
    bool dropTurn = true;           // drop시 회전을 90도로 제한하기 위한 변수
    public float turnSpeed = 500f;  // 회전 모션 속도
    float rot = 0f;                 // jump에서 공중회전 각도 누적 변수

    GameObject MotionCube;


    //Player(Rocket) 관련 변수
    public float GravityRocket = -5f;

    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.forward;                          // 진행방향
        rig = GetComponent<Rigidbody>();                // Rigidbody
        MotionCube = GameObject.Find("MotionCube");     // CUBE 모드에서 돌아가는 모션 대상
        Mode = ModeState.CUBE;                          // MODE를 FSM으로 관리
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
        /*
        OnCollisionEnter에서 아래사항 처리 
        - jumpState = false 
        - jumpTurn = false 
        - dropTurn = false 
        - rot = 0, MotionCube의 Rotation을 (0,0,0)으로 처리
        */

        // 1-2) 바닥과 접촉하지 않은 경우
        if (jumpState && !jumpTurn) JumpTurn();
        if (!dropTurn) DropTurn();

        // 2. jump 기능 구현
        // : space키 입력 && 점프하고 있는 상황이 아니라면
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            //rig.AddForce(Vector3.up * jumpPower);
            //rig.velocity =/ 
            jumpState = true;
        }

        // 3. P = P0+vt 적용
        transform.position += dir * moveSpeed * Time.deltaTime;
        rig.MovePosition(transform.position);
        //rig.AddForce(dir * moveSpeed);

    }

    // Mode: Rocket일때 동작
    private void UpdateRocket()
    {
        throw new NotImplementedException();
    }

    // OnCollisionEnter에 포함된 내용 
    /*
    1. Player(Cube)일 때
        - 바닥과 접촉했을 때, 상태처리 
        - 장애물과 충돌했을 때, Destroy처리 
    2. Player(Rocket)일 때
        - 
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (Mode == ModeState.CUBE)
        {
            // 바닥과 접족한 경우, jumpState = false
            jumpState = false;
            jumpTurn = false;
            dropTurn = false;
            rot = 0;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);      //  바닥에 닿아있을 때는 rotation 고정

            // 장애물과 닿았았을 때, Player를 Destroy 처리

        }
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
}
