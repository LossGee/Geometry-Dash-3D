using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [Mode: UFO]
public partial class PlayerMove : MonoBehaviour
{
    private void UpdateUFO()
    {
        // 1. 천장(Above)과 바닥(Below) 접촉여부 검사
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        yVelocity += gravity * Time.deltaTime;                                  // yVelocity에 중력 적용

        // 2. jump 기능 구현
        // 2-1) Normal JUMP: 사용자로부터 space키를 눌러을 때
        // 역중력 상태(ReversGravity)일 때
        if (reversGravityState)
        {
            if (Input.GetKeyDown(KeyCode.Space)
                && ((cc.collisionFlags & CollisionFlags.Below) == 0))
            {
                yVelocity = jumpPower;
            }
        }
        // 일반 상태(NormalGravity)일 때
        else
        {
            if (Input.GetKeyDown(KeyCode.Space)
                && ((cc.collisionFlags & CollisionFlags.Above) == 0))
            {
                yVelocity = jumpPower;
            }
        }
        // PowerJump
        if (isContactPowerJump)
        {
            PowerJump();
        }
        ReverseGravity();

        // 3. Motion 적용 
        UFOMotion();

        // 4. 움직임 적용 
        preYpos = transform.position.y;
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);                              // 움직임 적용하기 
        currentYpos = transform.position.y;
        currentZpos = transform.position.z;
    }
    // (UFO) jump시 UFO Motino 기울기 변화 모션
    void UFOMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir) / 3;
        if (!reversGravityState)
        {
            if (dir.y >= 0)
            {
                MotionUFO.transform.rotation = Quaternion.Euler(angle, 0, 0);
            }
        }
        else
        {
            if (dir.y <= 0)
            {
                MotionUFO.transform.rotation = Quaternion.Euler(-angle, 0, 0);
            }
        }
    }
}
