using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [Mode: Cube]
public partial class PlayerMove : MonoBehaviour
{
    // Player(Cube) 변수
    public bool jumpState = false;                                              // jump 상태(ture: 점프중, false: 점프X) - 이단 점프 방지
    public bool jumpTurn = true;                                                // jump시 회전을 180도로 제한하기 위한 변수
    public bool dropTurn = true;                                                // drop시 회전을 90도로 제한하기 위한 변수
    float rot = 0f;                                                      // jump에서 공중회전 각도 누적 변수
    private void UpdateCube()
    {
        // 1. 천장 or 바닥과 접촉여부 검사(상하중력 전환인 경우 포함)
        // cc의 위(Above), 아래(Below) 충돌 여부 검사(ture: 공중, false: 지면)
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);

        yVelocity += gravity * Time.deltaTime;                                // yVelocity에 중력 적용

        // 1-1) 바닥과 접촉인 경우
        if (isContactAB)
        {
            jumpState = false;
            jumpTurn = false;
            dropTurn = false;
            rot = 0;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);          //  바닥에 닿아있을 때는 rotation 고정
        }
        // 1-2) 바닥과 접촉하지 않은 경우(MotionCube 움직임 정의)
        else
        {
            if (!jumpState)
            {
                if (!dropTurn)
                {
                    DropTurn();                                                 // 낙하시, MotionCube 90도 회전
                }
            }
            else
            {
                JumpTurn();
            }
        }

        // 2. jump 기능 구현
        // 2-1) Normal JUMP: 사용자로부터 space키를 눌러을 때
        if (Input.GetKeyDown(KeyCode.Space) && isContactAB)
        {
            jumpState = true;
            yVelocity = jumpPower;                                              // yVeleocity에 cubeJumpPower 적용
            isContactAB = false;
            //print("Jump!!!");
        }
        // 2-2) POWER JUMP: Player가 powerjump Object 만났을 때
        if (isContactPowerJump)
        {
            jumpState = true;
            PowerJump();
        }
        // 2-3) AIR JUMP: Player가 jump 중에 Air jump Object와 접촉한 상태에서 space를 눌렀을 때
        if (isContactAirJump)
        {
            jumpTurn = false;
            AirJump();
        }

        // 3. dir에 yVelocity 적용
        // 3-1) reverse Gravity
        ReverseGravity();

        // 4. cc에 움직입 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);                              // 움직임 적용하기 
        currentZpos = transform.position.z;

    }

    // (Cube) jump시 MotinoCube 180도 회전 모션 함수
    void JumpTurn()
    {
        if (!reversGravityState)
        {
            rot += cubeTurnSpeed * Time.deltaTime;
            if (rot < 180) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else jumpTurn = true;
        }
        else
        {
            rot -= cubeTurnSpeed * Time.deltaTime;
            if (rot > -180) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else jumpTurn = true;
        }

    }

    // (Cube) drop시 MotinoCube 90도 회전 모션 함수
    void DropTurn()
    {
        if (!reversGravityState)
        {
            rot += cubeTurnSpeed * Time.deltaTime;
            if (rot < 90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else dropTurn = true;
        }
        else
        {
            rot -= cubeTurnSpeed * Time.deltaTime;
            if (rot > -90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else dropTurn = true;
        }

    }

    // (Cube & RACE) 공중 점프 Intaraction과 접촉했을 때, 2단 점프 허용
    void AirJump()
    {
        if (Input.GetButtonDown("Jump") && jumpState)
        {
            if (Mode == ModeState.CUBE)
            {
                rot = 0;
            }
            //print("AirJump");
            yVelocity = airJumpPower;
        }
    }
}
