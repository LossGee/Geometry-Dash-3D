using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: SATELLITE_horizontal]
public partial class PlayerMove : MonoBehaviour
{
    bool SATELLITE_horizontalRightLeftState;
    private void UpdateSATELLITE_horizontal()
    {
        // 1. 사용자 입력
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SATELLITE_horizontalRightLeftState = !SATELLITE_horizontalRightLeftState;
        }

        // 2. dir 적용
        // 2-1) 진행방향 결정
        if (SATELLITE_horizontalRightLeftState)
        {
            dir = new Vector3(1, 0, 1);
        }
        else
        {
            dir = new Vector3(-1, 0, 1);
        }
        // 2-2) 중력 적용
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        dir.Normalize();
        yVelocity += gravity * Time.deltaTime;

        // reversGravityState에 따른 중력 방향 전환
        ReverseGravity();

        // 3. SATELLITE_horizontal 모션 적용
        SATELLITE_horizontalMotion();

        // 4. 움직임 적용
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_horizontal) SATELLITE_horizontalMotion 모션 구현
    public void SATELLITE_horizontalMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir)*0.6f;
        if (dir.x < 0)
        {
            angle = -angle;
        }
        Quaternion moveAngle = Quaternion.Euler(0, angle, 0);
        MotionSATELLITE_horizontal.transform.rotation 
            = Quaternion.Lerp(MotionSATELLITE_horizontal.transform.rotation, moveAngle, 0.05f);
    }
}
