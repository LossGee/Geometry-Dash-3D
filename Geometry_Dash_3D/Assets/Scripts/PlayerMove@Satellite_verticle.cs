using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: SATELLIsTE_vertical]
public partial class PlayerMove : MonoBehaviour
{
    bool SATELLITE_verticalUpDownState = true;              // SATELLITE_vertical의 위/아래 진행상태(true:우상향, false:우하향) 
    private void UpdateSATELLITE_vertical()
    {
        // 1. 사용자 입력
        if (Input.GetKeyDown(KeyCode.Space))                // 사용자의 Space키 입력에 따라 방향 전환
        {
            SATELLITE_verticalUpDownState = !SATELLITE_verticalUpDownState;
        }
        // 2. dir 적용
        if (SATELLITE_verticalUpDownState)
        {
            dir = new Vector3(0, 1, 1);                     // 우상향 45도 방향
        }
        else
        {
            dir = new Vector3(0, -1, 1);                    // 우하향 45도 방향
        }

        // 3. SATELLITE_vertical 모션 적용
        SATELLITE_verticalMotion();
        // 4. 움직임 적용
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_vertical) SATELLITE_verticalMotion의 모션 구현
    void SATELLITE_verticalMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.y > 0)
        {
            angle = -angle;
        }
        Quaternion moveAngle = Quaternion.Euler(90 + angle, 0, 0);
        MotionSATELLITE.transform.rotation = Quaternion.Lerp(MotionSATELLITE.transform.rotation, moveAngle, 0.5f);
    }
}
