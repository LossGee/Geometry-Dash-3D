using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: Race]
public partial class PlayerMove : MonoBehaviour
{
    private void UpdateRace()
    {
        //������ġ���� ���� ����� �������� �̵� 

        // 1. Race ��忡���� dir ���ϱ�(Line ��ġ�� �����ϱ�) 
        // 1) ����Ű �Է¿� ���� Line(racePos) �̵�
        // ����� �¿� ����Ű �Է�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (LineNumber > 0)
            {
                LineNumber--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (LineNumber < 3)
            {
                LineNumber++;
            }
        }
        // LineNumber�� ���� Player�� dir.x ����
        currentXpos = transform.position.x;                                  // ���� ��ġ������ x��ǥ ���ϱ�
        nextXpos = SetRaceLine(LineNumber);                                  // ���� RaceLine�� x��ǥ ���ϱ� 
        dir.x = nextXpos - currentXpos;                                      // dir.x = ������ġ������ x��ǥ - ���� Line�� x��ǥ

        //  2) �߷� ����
        // õ��or����� ���˿��� Ȯ�� �� yVelocity ����
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);

        yVelocity += gravity * Time.deltaTime;

        // PowerJump ���� ���� Ȯ�� �� ����
        if (isContactPowerJump)
        {
            PowerJump();
        }
        // reversGravityState�� ���� �߷� ���� ��ȯ
        ReverseGravity();

        //  3) ��� ����
        RaceMotion();

        // 2. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);                            // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
    // (Race) �¿��̵��� Race Motion�� �¿� ���� ���
    private void RaceMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.x > 0)
        {
            angle = -angle;
        }
        Quaternion next = Quaternion.Euler(-90, -90 - angle, -90);
        MotionRace.transform.rotation = Quaternion.Lerp(MotionRace.transform.rotation, next, 0.1f);
    }
    // (Race) ������ġ�� ���� ��� Race Line�� �ִ��� �˻�
    private void CheckLineNumber()
    {
        if ((posX >= -4.8f) && (posX < -2.4f))
        {
            LineNumber = 0;
        }
        else if ((posX >= -2.4f) && (posX < 0f))
        {
            LineNumber = 1;
        }
        else if ((posX >= 0f) && (posX < 2.4f))
        {
            LineNumber = 2;
        }
        else if ((posX >= -2.4f) && (posX <= 4.8f))
        {
            LineNumber = 3;
        }
    }

    // (Race) LineNumber�� ���� Player�� ��ġ ����
    private float SetRaceLine(int LineNumber)
    {
        switch (LineNumber)
        {
            case 0: posX = -3.6f; break;
            case 1: posX = -1.2f; break;
            case 2: posX = 1.2f; break;
            case 3: posX = 3.6f; break;
        }
        return posX;
    }
}
