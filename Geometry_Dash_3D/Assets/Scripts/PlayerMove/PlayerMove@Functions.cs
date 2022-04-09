using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    // (공통) State를 Play 모드로 바꾸는 함수(CountDownManager에서 활용)
    public void ChangeToPlayMode()
    {
        SoundManager.Instance.bgSoundPlayinManager();
        state = State.PLAY;                                           // 시간이 지나면 Play 모드로 전환(객체지향 준수하는 코드로 수정)
    }

    // (공통) Mode 전환 Potal을 만날 경우, changePlayMode를 true로 설정해주는 함수 
    public void ChangePlayMode()
    {
        changePlayMode = true;
    }

    // (공통) Player 죽음 처리 함수
    bool dead = false;                                          // daed 상태 표시 변수(true: 죽음)
    public void Dead()                                          // Player 죽음 처리 함수 
    {
        // Dead 조건(공통): 벽에 부딪혔을 때(Vector3.forward 방향으로 좌표변화가 없을 때)
        if (currentZpos <= preZpos)
        {
            print("1111111111");
            dead = true;
        }
        
        // Dead 조건(추가, SATELLITE_vertical): 위/아래가 접촉되었을 때
        if (Mode == ModeState.SATELLITE_vertical)
        {
            // 위/아래 접촉 여부 검사 
            isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
            // 접촉시 dead = true
            if (isContactAB)
            {
                print("22222222222");

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
                print("333333333333");

                dead = true;
            }
        }
    }
    // (공통) 장애물 충돌시 dead true 처리
    public void CrashObstacle()                                 // 장애물에 부딪혔을 때
    {
        print("44444444444");

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
