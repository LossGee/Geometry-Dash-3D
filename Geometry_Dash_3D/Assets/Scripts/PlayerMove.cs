using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
<���� ���>
step1) Player�� Vector3.forward �������� ����
step2) Character Controller�� Ȱ���Ͽ� jump ��� ����
step3) Jump�� MotionCube�� 
    
 */
public class PlayerMove : MonoBehaviour
{
    // Player mode
    enum ModeState { 
        CUBE,
        ROCKET
    }
    ModeState Mode;


    // ���� ����
    public float moveSpeed = 20f;        
    float gravity = -9.8f;
    float yVelocity = 0f;

    Vector3 dir;
    CharacterController cc;

    // Palyer(Cube) ����
    public float jumpPower = 2f;
    bool jumpState = false;         // jump ����(ture: ������ // false: ����X) - �̴� ���� ����
    bool jumpTurn = true;           // jump�� ȸ���� 180���� �����ϱ� ���� ����
    bool dropTurn = true;           // drop�� ȸ���� 90���� �����ϱ� ���� ����
    public float turnSpeed = 500f;  // ȸ�� ��� �ӵ�
    float rot = 0f;                 // jump���� ����ȸ�� ���� ���� ����
    
    GameObject MotionCube;

    // Player(Locket) ����
    float AntiGravity = 8.0f;       // Rocket Moded���� �߷¿� �ݴ�������� �����ϴ� �� (�ϴ� public���� �ϰ� �����ϸ� �� �˾ƺ���)

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;
        MotionCube = GameObject.Find("MotionCube");
        Mode = ModeState.CUBE;
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
        if (cc.isGrounded)
        {
            yVelocity = 0;      // �߷� ���� X
            jumpState = false;
            jumpTurn = false;
            dropTurn = false;
            rot = 0;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);      //  �ٴڿ� ������� ���� rotation ����
        }
        // 1-2) �ٴڰ� �������� ���� ���
        else
        {
            yVelocity += gravity * Time.deltaTime;      // yVelocity�� �߷� ����

            if (jumpState && !jumpTurn)
            {
                JumpTurn();     // jump��, MotionCube 180�� ȸ��
            }
            if (!dropTurn)
            {
                DropTurn();     // ���Ͻ�, MotionCube 90�� ȸ��
            }
        }

        // 2. jump ��� ����
        // : spaceŰ �Է� && �����ϰ� �ִ� ��Ȳ�� �ƴ϶��
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumpState = true;
            yVelocity = jumpPower;      // yVeleocity�� jumpPower ����
        }
        dir.y = yVelocity;              // dir.y�� yVelocity ����

        // 3. cc�� ������ ����
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // Mode: Player�϶� ����
    private void UpdateRocket()
    {
        MotionCube.SetActive(false);


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

    // yVelocity �ð�ȭ code
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + Vector3.up * yVelocity;
        Gizmos.DrawLine(from, to);
    }*/


}
