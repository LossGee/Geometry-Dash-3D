using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [Mode: Cube]
public partial class PlayerMove : MonoBehaviour
{
    // Player(Cube) ����
    public bool jumpState = false;                                              // jump ����(ture: ������, false: ����X) - �̴� ���� ����
    public bool jumpTurn = true;                                                // jump�� ȸ���� 180���� �����ϱ� ���� ����
    public bool dropTurn = true;                                                // drop�� ȸ���� 90���� �����ϱ� ���� ����
    float rot = 0f;                                                      // jump���� ����ȸ�� ���� ���� ����
    private void UpdateCube()
    {
        // 1. õ�� or �ٴڰ� ���˿��� �˻�(�����߷� ��ȯ�� ��� ����)
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�(ture: ����, false: ����)
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);

        yVelocity += gravity * Time.deltaTime;                                // yVelocity�� �߷� ����

        // 1-1) �ٴڰ� ������ ���
        if (isContactAB)
        {
            jumpState = false;
            jumpTurn = false;
            dropTurn = false;
            rot = 0;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);          //  �ٴڿ� ������� ���� rotation ����
        }
        // 1-2) �ٴڰ� �������� ���� ���(MotionCube ������ ����)
        else
        {
            if (!jumpState)
            {
                if (!dropTurn)
                {
                    DropTurn();                                                 // ���Ͻ�, MotionCube 90�� ȸ��
                }
            }
            else
            {
                JumpTurn();
            }
        }

        // 2. jump ��� ����
        // 2-1) Normal JUMP: ����ڷκ��� spaceŰ�� ������ ��
        if (Input.GetKeyDown(KeyCode.Space) && isContactAB)
        {
            jumpState = true;
            yVelocity = jumpPower;                                              // yVeleocity�� cubeJumpPower ����
            isContactAB = false;
            //print("Jump!!!");
        }
        // 2-2) POWER JUMP: Player�� powerjump Object ������ ��
        if (isContactPowerJump)
        {
            jumpState = true;
            PowerJump();
        }
        // 2-3) AIR JUMP: Player�� jump �߿� Air jump Object�� ������ ���¿��� space�� ������ ��
        if (isContactAirJump)
        {
            jumpTurn = false;
            AirJump();
        }

        // 3. dir�� yVelocity ����
        // 3-1) reverse Gravity
        ReverseGravity();

        // 4. cc�� ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);                              // ������ �����ϱ� 
        currentZpos = transform.position.z;

    }

    // (Cube) jump�� MotinoCube 180�� ȸ�� ��� �Լ�
    void JumpTurn()
    {
        if (!reversGravityState)
        {
            rot += cubeTurnSpeed * Time.deltaTime;
            if (rot < 180) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else jumpTurn = true;
        }
        else
        {
            rot -= cubeTurnSpeed * Time.deltaTime;
            if (rot > -180) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else jumpTurn = true;
        }

    }

    // (Cube) drop�� MotinoCube 90�� ȸ�� ��� �Լ�
    void DropTurn()
    {
        if (!reversGravityState)
        {
            rot += cubeTurnSpeed * Time.deltaTime;
            if (rot < 90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else dropTurn = true;
        }
        else
        {
            rot -= cubeTurnSpeed * Time.deltaTime;
            if (rot > -90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            else dropTurn = true;
        }

    }

    // (Cube & RACE) ���� ���� Intaraction�� �������� ��, 2�� ���� ���
    void AirJump()
    {
        if (Input.GetButtonDown("Jump") && jumpState)
        {
            if (Mode == ModeState.CUBE)
            {
                rot = 0;
            }
            //print("AirJump");
            yVelocity = airJumpPower;
        }
    }
}
