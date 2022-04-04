using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: Race]
public partial class PlayerMove : MonoBehaviour
{
    private void UpdateRace()
    {
        //현재위치에서 가장 가까운 라인으로 이동 

        // 1. Race 모드에서의 dir 정하기(Line 위치로 제어하기) 
        // 1) 방향키 입력에 따른 Line(racePos) 이동
        // 사용자 좌우 방향키 입력
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (LineNumber > 0)
            {
                LineNumber--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (LineNumber < 3)
            {
                LineNumber++;
            }
        }
        // LineNumber에 따른 Player의 dir.x 설정
        currentXpos = transform.position.x;                                  // 현재 위치에서의 x좌표 구하기
        nextXpos = SetRaceLine(LineNumber);                                  // 다음 RaceLine의 x좌표 구하기 
        dir.x = nextXpos - currentXpos;                                      // dir.x = 현재위치에서의 x좌표 - 다음 Line의 x좌표

        //  2) 중력 적용
        // 천장or지면과 접촉여부 확인 및 yVelocity 적용
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);

        yVelocity += gravity * Time.deltaTime;

        // PowerJump 접촉 여부 확인 및 실행
        if (isContactPowerJump)
        {
            PowerJump();
        }
        // reversGravityState에 따른 중력 방향 전환
        ReverseGravity();

        //  3) 모션 적용
        RaceMotion();

        // 2. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);                            // 움직임 적용하기 
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
        Quaternion next = Quaternion.Euler(-90, -90 - angle, -90);
        MotionRace.transform.rotation = Quaternion.Lerp(MotionRace.transform.rotation, next, 0.1f);
    }
    // (Race) 현재위치에 따라 어느 Race Line에 있는지 검사
    private void CheckLineNumber()
    {
        if ((posX >= -4.8f) && (posX < -2.4f))
        {
            LineNumber = 0;
        }
        else if ((posX >= -2.4f) && (posX < 0f))
        {
            LineNumber = 1;
        }
        else if ((posX >= 0f) && (posX < 2.4f))
        {
            LineNumber = 2;
        }
        else if ((posX >= -2.4f) && (posX <= 4.8f))
        {
            LineNumber = 3;
        }
    }

    // (Race) LineNumber에 따라 Player의 위치 설정
    private float SetRaceLine(int LineNumber)
    {
        switch (LineNumber)
        {
            case 0: posX = -3.6f; break;
            case 1: posX = -1.2f; break;
            case 2: posX = 1.2f; break;
            case 3: posX = 3.6f; break;
        }
        return posX;
    }
}
