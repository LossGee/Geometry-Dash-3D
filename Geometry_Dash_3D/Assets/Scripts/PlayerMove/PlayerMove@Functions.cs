using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
{
    // (����) State�� Play ���� �ٲٴ� �Լ�(CountDownManager���� Ȱ��)
    public void ChangeToPlayMode()
    {
        SoundManager.Instance.bgSoundPlayinManager();
        state = State.PLAY;                                           // �ð��� ������ Play ���� ��ȯ(��ü���� �ؼ��ϴ� �ڵ�� ����)
    }

    // (����) Mode ��ȯ Potal�� ���� ���, changePlayMode�� true�� �������ִ� �Լ� 
    public void ChangePlayMode()
    {
        changePlayMode = true;
    }

    // (����) Player ���� ó�� �Լ�
    bool dead = false;                                          // daed ���� ǥ�� ����(true: ����)
    public void Dead()                                          // Player ���� ó�� �Լ� 
    {
        // Dead ����(����): ���� �ε����� ��(Vector3.forward �������� ��ǥ��ȭ�� ���� ��)
        if (currentZpos <= preZpos)
        {
            print("1111111111");
            dead = true;
        }
        
        // Dead ����(�߰�, SATELLITE_vertical): ��/�Ʒ��� ���˵Ǿ��� ��
        if (Mode == ModeState.SATELLITE_vertical)
        {
            // ��/�Ʒ� ���� ���� �˻� 
            isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
            // ���˽� dead = true
            if (isContactAB)
            {
                print("22222222222");

                dead = true;
            }
        }
        // Dead ����(�߰�, SATELLITE_horizotal): �¿� ���� ���˵Ǿ��� ��
        if (Mode == ModeState.SATELLITE_horizontal)
        {
            // �¿� ���� ���� Ȯ��
            isContactSides = (cc.collisionFlags & CollisionFlags.Sides) != 0;
            // ���˽� dead = true
            if (isContactSides)
            {
                print("333333333333");

                dead = true;
            }
        }
    }
    // (����) ��ֹ� �浹�� dead true ó��
    public void CrashObstacle()                                 // ��ֹ��� �ε����� ��
    {
        print("44444444444");

        dead = true;
    }
    // (Cube) AirJump Interaction�� �������� ��, isContactAirJump�� true�� �ٲ��ִ� �Լ�
    public void OnAirJump()
    {
        isContactAirJump = true;
    }
    // (Cube) AirJump Instaraction�� ������ ������ ��, isContactAirJump�� false�� �ٲ��ִ� �Լ�
    public void OffAirJump()
    {
        isContactAirJump = false;
    }
    // (CUBE & UFO & RACE) �Ŀ������븦 ������ ��, ���� �������� 1.5�� ���̷� �����Ѵ�. 
    void PowerJump()
    {
        yVelocity = powerJumpPower;
        isContactPowerJump = false;
    }
    // (Cube & UFO & Race) PowerJump Interaction�� �������� ��, isContactPowerJump�� true�� �ٲ��ִ� �Լ�
    public void ContactPowerJump()
    {
        isContactPowerJump = true;
    }
    // (ube & UFO & Race & Satellite & Rocket) �߷¹��� ��Ż�� �������� �� �߷¹�������(ReversGravityState)�� ���������ִ� �Լ�
    public void ChangeReverseGravityState()
    {
        reversGravityState = !reversGravityState;
    }
    // (Cube & UFO & Race & Satellite & Rocket) �߷¹��� ����
    void ReverseGravity()
    {
        if (reversGravityState)
        {
            dir.y = -yVelocity;
        }
        else
        {
            dir.y = yVelocity;
        }
    }


    // Vector �ð�ȭ code
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + (reversGravityState ? Vector3.down : Vector3.up) * yVelocity;
        Gizmos.DrawLine(from, to);
    }
}
