using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 


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
    public float jumpPower = 2f;

    Vector3 dir;
    CharacterController cc0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject CC1 = transform.GetChild(1).gameObject;
        GameObject CC2 = transform.GetChild(2).gameObject;

        cc0 = GetComponent<CharacterController>();
        cc1 = CC1.GetComponent<CharacterController>();
        cc2 = CC2.GetComponent<CharacterController>();
        dir = Vector3.forward;
        MotionCube = GameObject.Find("MotionCube");
        MotionRocket = GameObject.Find("MotionRocket");
        Mode = ModeState.CUBE;
        //Mode = ModeState.ROCKET;
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

    // [Mode: Cube�� ��]
    // Player(Cube) ����
    public float CubeGravity = -9.8f;    // Cube ����� �߷�
    bool jumpState = false;         // jump ����(ture: ������ // false: ����X) - �̴� ���� ����
    bool jumpTurn = true;           // jump�� ȸ���� 180���� �����ϱ� ���� ����
    bool dropTurn = true;           // drop�� ȸ���� 90���� �����ϱ� ���� ����
    public float turnSpeed = 500f;  // ȸ�� ��� �ӵ�
    float rot = 0f;                 // jump���� ����ȸ�� ���� ���� ����

    GameObject MotionCube;
    CharacterController cc1;
    CharacterController cc2;

    private void UpdateCube()
    {
        gravity = CubeGravity;       // Cube ����϶� �߷� ����

        // 0. MotionCube Ȱ��ȭ && MotionRocket ��Ȱ��ȭ
        //MotionRocket.SetActive(false);
        //MotionCube.SetActive(true);

        // 1. �ٴڰ� ���˿��� �˻�
        // 1-1) �ٴڰ� ������ ���
        if (cc0.isGrounded)
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

        // 3. cc0�� ������ ����
        cc0.Move(dir * moveSpeed * Time.deltaTime);
        //cc1.Move(dir * moveSpeed * Time.deltaTime);
        //cc2.Move(dir * moveSpeed * Time.deltaTime);

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


    // [Mode: Rocket�� ��]
    // Player(Locket) ����
    public float RocketGravity = -5.0f;  // Rocket ����� �߷�  
    public float upPower = 1.0f;         // space ���� ��, ���� �ö󰡴� ��
    float angle = 0f;

    GameObject MotionRocket;

    private void UpdateRocket()
    {
        gravity = RocketGravity;        // RocketGravity �߷� ����

        // 0. MotionCube ��Ȱ��ȭ && MotionRocket Ȱ��ȭ
        //MotionCube.SetActive(false);  //MontionRocket Off & MotinCube On
        //MotionRocket.SetActive(true);

        // 1. Rocket ����� dir ���ϱ�
        yVelocity += gravity * Time.deltaTime;      // yVelocity�� garvity ���� 
        if (Input.GetKey(KeyCode.Space))            // space �Է� ��, yVelocitydp jumpPower �����ϱ�(IPointerDownHandler, IPointerUpHandler ���)
        {
            yVelocity += upPower;
        }
        dir.y = yVelocity;


        // 2. MotionRocket ���� ����
        //  : dir���Ϳ� Vector3�� ���̰��� ���Ͽ� MotionRocket�� ������⿡ ���� ������ Ʋ���� ����

        if (cc0.isGrounded)
        {
            angle = Mathf.Lerp(0, angle, 0.9f);         // �ڿ������� �ٴڿ� �����ϴ� ���
        }
        else
        {
            angle = Vector3.Angle(Vector3.forward, dir.normalized);
        }

        // Vector3.Angle()�� ����� �������� ��ȯ�ϹǷ� dir.y�� ���� ���, ����     
        if (dir.y > 0)      
        {
            angle = -angle;
        }
        //print("Angle: " + angle);

        MotionRocket.transform.rotation = Quaternion.Euler(angle+90, 0 ,0);

        // 3. ������ ����
        cc0.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 

        
    }

    // (����) Player ���� 
    public void Dead()
    {
        Destroy(gameObject);
    }

    // Vector �ð�ȭ code
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + dir;
        Gizmos.DrawLine(from, to);
    }


}
