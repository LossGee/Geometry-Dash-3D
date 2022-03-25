using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_Rig : MonoBehaviour
{
    // ����
    public float moveSpeed = 20f;
    float gravity;

    enum ModeState
    {
        CUBE,
        ROCKET
    }
    ModeState Mode;
    Vector3 dir;
    Rigidbody rig;


    // Player(Cube) ���� ����
    public float jumpPower = 1000f;
    public float GravityCube = -9.8f;
    bool jumpState = false;         // jump ����(ture: ������ // false: ����X) - �̴� ���� ����
    bool jumpTurn = true;           // jump�� ȸ���� 180���� �����ϱ� ���� ����
    bool dropTurn = true;           // drop�� ȸ���� 90���� �����ϱ� ���� ����
    public float turnSpeed = 500f;  // ȸ�� ��� �ӵ�
    float rot = 0f;                 // jump���� ����ȸ�� ���� ���� ����

    GameObject MotionCube;


    //Player(Rocket) ���� ����
    public float GravityRocket = -5f;

    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.forward;                          // �������
        rig = GetComponent<Rigidbody>();                // Rigidbody
        MotionCube = GameObject.Find("MotionCube");     // CUBE ��忡�� ���ư��� ��� ���
        Mode = ModeState.CUBE;                          // MODE�� FSM���� ����
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mode)
        {
            case ModeState.CUBE: UpdateCube(); break;
            case ModeState.ROCKET: UpdateRocket(); break;
        }
    }


    // Mode: Cube�� ��
    private void UpdateCube()
    {
        // 1. �ٴڰ� ���˿��� �˻�
        // 1-1) �ٴڰ� ������ ���
        /*
        OnCollisionEnter���� �Ʒ����� ó�� 
        - jumpState = false 
        - jumpTurn = false 
        - dropTurn = false 
        - rot = 0, MotionCube�� Rotation�� (0,0,0)���� ó��
        */

        // 1-2) �ٴڰ� �������� ���� ���
        if (jumpState && !jumpTurn) JumpTurn();
        if (!dropTurn) DropTurn();

        // 2. jump ��� ����
        // : spaceŰ �Է� && �����ϰ� �ִ� ��Ȳ�� �ƴ϶��
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            //rig.AddForce(Vector3.up * jumpPower);
            //rig.velocity =/ 
            jumpState = true;
        }

        // 3. P = P0+vt ����
        transform.position += dir * moveSpeed * Time.deltaTime;
        rig.MovePosition(transform.position);
        //rig.AddForce(dir * moveSpeed);

    }

    // Mode: Rocket�϶� ����
    private void UpdateRocket()
    {
        throw new NotImplementedException();
    }

    // OnCollisionEnter�� ���Ե� ���� 
    /*
    1. Player(Cube)�� ��
        - �ٴڰ� �������� ��, ����ó�� 
        - ��ֹ��� �浹���� ��, Destroyó�� 
    2. Player(Rocket)�� ��
        - 
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (Mode == ModeState.CUBE)
        {
            // �ٴڰ� ������ ���, jumpState = false
            jumpState = false;
            jumpTurn = false;
            dropTurn = false;
            rot = 0;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);      //  �ٴڿ� ������� ���� rotation ����

            // ��ֹ��� ��Ҿ��� ��, Player�� Destroy ó��

        }
    }

    // (Cube) jump�� MotinoCube 180�� ȸ�� ��� �Լ�
    public void JumpTurn()
    {
        rot += turnSpeed * Time.deltaTime;
        if (rot < 180) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        else jumpTurn = true;
    }

    // (Cube) drop�� MotinoCube 90�� ȸ�� ��� �Լ�
    public void DropTurn()
    {
        rot += turnSpeed * Time.deltaTime;
        if (rot < 90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        else dropTurn = true;
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
