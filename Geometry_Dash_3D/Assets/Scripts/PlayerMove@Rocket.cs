using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: Rocket]
public partial class PlayerMove : MonoBehaviour
{
    // Player(Locket) ����
    private void UpdateRocket()
    {
        // �߷�����
        gravity = RocketGravity;                            // RocketGravity �߷��� gravity�� ����
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);

        // 1. Rocket ����� dir ���ϱ�
        yVelocity += gravity * Time.deltaTime;              // yVelocity�� garvity ���� 
        if (Input.GetKey(KeyCode.Space))                    // space �Է� ��, yVelocitydp rocketUpPower �����ϱ�
        {
            yVelocity += rocketUpPower;
            if (isContactAB)
            {
                yVelocity = Mathf.Clamp(yVelocity, -0.1f, 0.1f);
            }
        }
        // reversGravityState�� ���� �߷� ���� ��ȯ
        ReverseGravity();

        // 2. MotionRocket ���� ����
        //  : dir���Ϳ� Vector3�� ���̰��� ���Ͽ� MotionRocket�� ������⿡ ���� ������ ���ϵ��� ����
        if (isContactAB)                                    // õ�� or �ٴڿ� ������ ���
        {
            angle = Mathf.Lerp(0, angle, 0.7f);             // �ڿ������� �ٴڿ� �����ϴ� ����� ���� ��������
        }
        else                                                // ���߿� ��ġ�� ���
        {
            angle = Vector3.Angle(Vector3.forward, dir.normalized);
        }
        // Vector3.Angle()�� ����� �������� ��ȯ�ϹǷ� dir.y�� ���� ���, ����     
        if (dir.y > 0)
        {
            angle = -angle;
        }
        MotionRocket.transform.rotation = Quaternion.Euler(angle + 90, 0, 0);

        // 3. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
}
