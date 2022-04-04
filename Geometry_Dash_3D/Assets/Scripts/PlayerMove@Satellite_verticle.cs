using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: SATELLIsTE_vertical]
public partial class PlayerMove : MonoBehaviour
{
    bool SATELLITE_verticalUpDownState = true;              // SATELLITE_vertical�� ��/�Ʒ� �������(true:�����, false:������) 
    private void UpdateSATELLITE_vertical()
    {
        // 1. ����� �Է�
        if (Input.GetKeyDown(KeyCode.Space))                // ������� SpaceŰ �Է¿� ���� ���� ��ȯ
        {
            SATELLITE_verticalUpDownState = !SATELLITE_verticalUpDownState;
        }
        // 2. dir ����
        if (SATELLITE_verticalUpDownState)
        {
            dir = new Vector3(0, 1, 1);                     // ����� 45�� ����
        }
        else
        {
            dir = new Vector3(0, -1, 1);                    // ������ 45�� ����
        }

        // 3. SATELLITE_vertical ��� ����
        SATELLITE_verticalMotion();
        // 4. ������ ����
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_vertical) SATELLITE_verticalMotion�� ��� ����
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
