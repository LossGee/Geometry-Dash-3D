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
        // 1. ����� �Է�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SATELLITE_horizontalRightLeftState = !SATELLITE_horizontalRightLeftState;
        }

        // 2. dir ����
        // 2-1) ������� ����
        if (SATELLITE_horizontalRightLeftState)
        {
            dir = new Vector3(1, 0, 1);
        }
        else
        {
            dir = new Vector3(-1, 0, 1);
        }
        // 2-2) �߷� ����
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        dir.Normalize();
        yVelocity += gravity * Time.deltaTime;

        // reversGravityState�� ���� �߷� ���� ��ȯ
        ReverseGravity();

        // 3. SATELLITE_horizontal ��� ����
        SATELLITE_horizontalMotion();

        // 4. ������ ����
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_horizontal) SATELLITE_horizontalMotion ��� ����
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
