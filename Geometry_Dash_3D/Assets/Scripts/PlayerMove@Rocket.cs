using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: Rocket]
public partial class PlayerMove : MonoBehaviour
{
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
        // reversGravityState에 따른 중력 방향 전환
        ReverseGravity();

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
        MotionRocket.transform.rotation = Quaternion.Euler(angle + 90, 0, 0);

        // 3. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
}
