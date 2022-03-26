using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 


public class PlayerMove : MonoBehaviour
{
    // Player mode
    public enum ModeState { 
        CUBE,
        ROCKET
    }
    public ModeState Mode;

    // ���� ����
    public float moveSpeed = 20f;        
    float gravity = -9.8f;
    float yVelocity = 0f;
    public float jumpPower = 2f;
    float preZpos;                  // Move ���� z��ǥ    
    float currentZpos;              // Move ���� z��ǥ

    Vector3 dir;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;

        SetMode();

        //print("MotionCube" + MotionCube.name);
        //print("MotionRocket" + MotionRocket.name);
    }

    // (����) Mode ��ȯ �Լ�: Motion�� ����ϴ� ��ü�� Ȱ��ȭ/��Ȱ��ȭ
    void SetMode()
    {
        if (Mode == ModeState.CUBE)
        {
            MotionCube.SetActive(true);
            MotionRocket.SetActive(false);
        }
        else if (Mode == ModeState.ROCKET)
        {
            MotionCube.SetActive(false);
            MotionRocket.SetActive(true);
        }

    }
    // (����) Player ���� 
    public void Dead()
    {
        if (currentZpos <= preZpos)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switch (Mode)
        {
            case ModeState.CUBE: 
                UpdateCube();
                Dead();
                break;
            case ModeState.ROCKET: 
                UpdateRocket();
                Dead();
                break;
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

    public GameObject MotionCube;

    private void UpdateCube()
    {
        SetMode();

        gravity = CubeGravity;       // Cube ����϶� �߷� ����

        // 0. MotionCube Ȱ��ȭ && MotionRocket ��Ȱ��ȭ
        //MotionRocket.SetActive(false);
        //MotionCube.SetActive(true);

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
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;

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
    public float RocketGravity = -2f;   // Rocket ����� �߷�  
    public float upPower = 0.02f;       // space ���� ��, ���� �ö󰡴� ��
    float angle = 0f;                   // dir�� Vector3.forward ������ ����
    bool isContactAB = false;           // ��(Above), �Ʒ�(Below)���� ���˿���(true=����/false=���߿� ���ִ� ����)

    public GameObject MotionRocket;

    private void UpdateRocket()
    {

        // �߷�����
        gravity = RocketGravity;                    // RocketGravity �߷��� gravity�� ����

        // 1. Rocket ����� dir ���ϱ�
        yVelocity += gravity * Time.deltaTime;      // yVelocity�� garvity ���� 
        if (Input.GetKey(KeyCode.Space))            // space �Է� ��, yVelocitydp jumpPower �����ϱ�(IPointerDownHandler, IPointerUpHandler ���)
        { 
            yVelocity += upPower;
        }
        dir.y = yVelocity;


        // 2. MotionRocket ���� ����
        //  : dir���Ϳ� Vector3�� ���̰��� ���Ͽ� MotionRocket�� ������⿡ ���� ������ ���ϵ��� ����
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)     // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
        
        if (isContactAB)                            // õ�� or �ٴڿ� ������ ���
        {
            angle = Mathf.Lerp(0, angle, 0.7f);     // �ڿ������� �ٴڿ� �����ϴ� ����� ���� ��������
        }
        else                                        // ���߿� ��ġ�� ���
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
        preZpos = transform.position.z;        
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;

        // 4. Dead
        //    : Player�� z��ǥ�� ������ġ�� ����ġ���� ���ų� ������ Dead
        
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
