using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Player mode
    public enum ModeState
    {
        CUBE,           // 3��Ī ���� CUBE ���(Geometry Dash)
        ROCKET,         // 3��Ī ���� ROCEKT ���(Geometry Dash)
        RACE,           // 3��Ī ���� ��� �Ʒ��� 45�� �������� RACAE ���(Beat Racer)
        FORWARD         // 3��Ī ���� ������������ ���� (VectorRush)
    }
    public ModeState Mode;
    public bool changeMode = false;     // Mode�� ��ȭ�� ���� �� ture

    // ���� ����
    public float moveSpeed = 20f;
    float gravity = -9.8f;
    float yVelocity = 0f;
    public float jumpPower = 2f;
    float preZpos;                  // Move ���� z��ǥ    
    float currentZpos;              // Move ���� z��ǥ
    float xPos = 0;
    float yPos = 0;
    float zPos = 0;


    Vector3 dir;
    CharacterController cc;

    // Start is called before the first frame update

    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;


        SetMode();
    }

    // (����) Mode ��ȯ �Լ�: Motion�� ����ϴ� ��ü�� Ȱ��ȭ/��Ȱ��ȭ
    void SetMode()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                MotionCube.SetActive(true);
                MotionRocket.SetActive(false);
                MotionRace.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionRocket.SetActive(true);
                MotionRace.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionRocket.SetActive(false);
                MotionRace.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionRocket.SetActive(false);
                MotionRace.SetActive(false);
                MotionForward.SetActive(true);
                break;

        }
        changeMode = true;
    }
    // (����) Player ���� 
    public void Dead()
    {
        if (currentZpos <= preZpos)
            SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMode)
        {
            SetMode();
        }

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
            case ModeState.RACE:
                UpdateRace();
                Dead();
                break;
            case ModeState.FORWARD:
                UpdateForward();
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

        // 3. cc�� ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
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
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)     
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
        // 1. Rocket ����� dir ���ϱ�
        yVelocity += gravity * Time.deltaTime;      // yVelocity�� garvity ���� 
        if (Input.GetKey(KeyCode.Space))            // space �Է� ��, yVelocitydp jumpPower �����ϱ�(IPointerDownHandler, IPointerUpHandler ���)
        {
            yVelocity += upPower;
            if (isContactAB)
            {
                yVelocity = Mathf.Clamp(yVelocity, -0.1f, 0.1f);
            }
        }
        dir.y = yVelocity;


        // 2. MotionRocket ���� ����
        //  : dir���Ϳ� Vector3�� ���̰��� ���Ͽ� MotionRocket�� ������⿡ ���� ������ ���ϵ��� ����
        

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

        MotionRocket.transform.rotation = Quaternion.Euler(angle + 90, 0, 0);

        // 3. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;

        // 4. Dead
        //    : Player�� z��ǥ�� ������ġ�� ����ġ���� ���ų� ������ Dead

    }

    // [Mode: Race�� ��]
    public float sidemoveSpeed = 3f;

    public GameObject MotionRace;
    private void UpdateRace()
    {
        // �߷�����
        gravity = CubeGravity;

        // 1. Race ��忡���� dir �ϱ�
        // 1) ����Ű�� ���� �¿� �̵�
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x -= sidemoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir.x += sidemoveSpeed * Time.deltaTime;
        }
        else
        {
            dir.x = Mathf.Lerp(dir.x, 0, 0.1f);
        }

        //  2) �߷� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 2. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }

    // [Mode: Forward�� ��]
    public GameObject MotionForward;
    private void UpdateForward()
    {
        // 1. Forward ��忡���� dir ����
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir += Vector3.up * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir -= Vector3.up * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir -= Vector3.right * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector3.right * Time.deltaTime;
        }
        else
        {
            dir = Vector3.forward;
        }
        dir.Normalize();

        // 2. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;

    }


    // Vector �ð�ȭ code
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Vector3 from = transform.position;
    //    Vector3 to = transform.position + dir;
    //    Gizmos.DrawLine(from, to);
    //}


}
