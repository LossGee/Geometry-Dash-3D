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
        // 1. õ��(Above)�� �ٴ�(Below) ���˿��� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        yVelocity += gravity * Time.deltaTime;                                  // yVelocity�� �߷� ����

        // 2. jump ��� ����
        // 2-1) Normal JUMP: ����ڷκ��� spaceŰ�� ������ ��
        // ���߷� ����(ReversGravity)�� ��
        if (reversGravityState)
        {
            if (Input.GetKeyDown(KeyCode.Space)
                && ((cc.collisionFlags & CollisionFlags.Below) == 0))
            {
                yVelocity = jumpPower;
            }
        }
        // �Ϲ� ����(NormalGravity)�� ��
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

        // 3. Motion ���� 
        UFOMotion();

        // 4. ������ ���� 
        preYpos = transform.position.y;
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);                              // ������ �����ϱ� 
        currentYpos = transform.position.y;
        currentZpos = transform.position.z;
    }
    // (UFO) jump�� UFO Motino ���� ��ȭ ���
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
