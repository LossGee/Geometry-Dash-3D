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
        UFO,            // 3��Ī ���� UFO ���(Geometry Dash)
        RACE,           // 3��Ī ���� ��� �Ʒ��� 45�� �������� RACAE ���(Beat Racer)
        ROCKET,         // 3��Ī ���� ROCEKT ���(Geometry Dash)
        FORWARD         // 3��Ī ���� ������������ ���� (VectorRush)
    }
    public ModeState Mode;
    bool changeMode = false;     // Mode�� ��ȭ�� ���� �� ture

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionRocket;
    public GameObject MotionForward;
    public float cubeTurnSpeed = 500f;      // ȸ�� ��� �ӵ�

    // �߷�(gravity) ���� ����
    float gravity = -9.8f;
    public float DefaultGravity = -9.8f;     // Cube, UFO ����� �߷�
    public float RaceGravity = -9.8f;        // Race ����� �߷�
    public float RocketGravity = -2f;        // Rocket ����� �߷�  

    // ����������� �����̴� �ӵ�(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;
    public float ufoMoveSpeed = 20f;
    public float raceMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;
    public float forwardMoveSpeed = 20f;

    // y�� +�������� �������� ���� ���õ� ����
    public float cubeJumpPower = 2f;        // (Cube) jump power
    public float ufoJumpPower = 2f;         // (UFO) jump power
    public float rocketUpPower = 0.02f;     // (Rocket) space ���� ��, ���� �ö󰡴� ��
    public float powerJumpPower = 2.2f;          // PowerJump Object�� �������� ���� jump power
    public float airJumpPower = 2f;

    // PowerJump, AirJump ���� ����
    bool isContactPowerJump = false;        // PowerJump�� �������� �� ���¸� ��Ÿ��(true: ����, fasle: ����X)
    bool isContactAirJump = false;          // AirJump�� �������� �� ���¸� ��Ÿ��(true: ����, flase: ����X)

    // x�� �������� �������� ���� ���õ� ����
    public float RaceSidemoveSpeed = 3f;    // (Race) �¿���� �̵� �ӵ�

    // ���� ����
    float yVelocity = 0f;
    float preZpos;                          // Move ���� z��ǥ    
    float currentZpos;                      // Move ���� z��ǥ
    float angle = 0f;                       // dir�� Vector3.forward ������ ����
    bool isContactAB = false;               // ��(Above), �Ʒ�(Below)���� ���˿���(true=����/false=���߿� ���ִ� ����)

    // ������Ż ���� ����
    public bool reverseGravity = false;            // �߷¹�������(true: �߷����� , false: reverse)
    

    Vector3 dir;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;

        SetMotion();
    }

    // (����) Mode ��ȯ Potal�� ���� ���, changeMode�� true�� �������ִ� �Լ� 
    public void ChangeMode()
    {
        changeMode = true;
    }

    // (����) Mode ��ȯ �Լ�: Motion�� ����ϴ� ��ü�� Ȱ��ȭ/��Ȱ��ȭ
    void SetMotion()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                MotionCube.SetActive(true);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.UFO:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(true);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(true);
                break;
        }
    }
    // (����) SetVar: ��庰 �� ������ ���� [�߷�(gravity), ����ӵ�(Speed), 
    //void SetVar()
    //{
    //    switch (Mode)
    //    {
    //        case ModeState.CUBE:

    //            break;
    //        case ModeState.UFO:

    //            break;
    //        case ModeState.RACE:

    //            break;
    //        case ModeState.ROCKET:

    //            break;
    //        case ModeState.FORWARD:

    //            break;
    //    }
    //}

    // (����) ��ֹ� �浹�� dead true ó��
    bool dead = false;               // daed ���� ǥ�� ����(true: ����)
    public void CrashObstacle()      // ��ֹ��� �ε����� ��
    {
        dead = true;
    }
    // (����) Player ���� 
    public void Dead()               // Player ���� ó�� �Լ� 
    {
        if (currentZpos <= preZpos)         // ���� �ε����� �� Dead
            dead = true;

        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(0);       // �ٽý���
        }
    }

    // (����) PowerJump Interaction�� �������� ��, isContactPowerJump�� true�� �ٲ��ִ� �Լ�
    public void ContactPowerJump()
    {
        isContactPowerJump = true;
    }
    // (����) AirJump Interaction�� �������� ��, isContactAirJump�� true�� �ٲ��ִ� �Լ�
    public void OnAirJump()
    {
        isContactAirJump = true;
    }
    public void OffAirJump()
    {
        isContactAirJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMode)
        {
            SetMotion();                        // Mode�� Mode ��� GameObject Ȱ��ȭ
            //SetVar();                           // Mode�� ����(variable) ����
            changeMode = false;
        }

        switch (Mode)
        {
            case ModeState.CUBE:UpdateCube(); break;
            case ModeState.UFO:UpdateUFO(); break;
            case ModeState.RACE:UpdateRace(); break;
            case ModeState.ROCKET:UpdateRocket(); break;
            case ModeState.FORWARD:UpdateForward(); break;
        }
        Dead();
    }

    // [Mode: Cube�� ��]
    // Player(Cube) ����
    bool jumpState = false;             // jump ����(ture: ������ // false: ����X) - �̴� ���� ����
    bool jumpTurn = true;               // jump�� ȸ���� 180���� �����ϱ� ���� ����
    bool dropTurn = true;               // drop�� ȸ���� 90���� �����ϱ� ���� ����
    float rot = 0f;                     // jump���� ����ȸ�� ���� ���� ����

    private void UpdateCube()
    {
        gravity = DefaultGravity;       // Cube ����϶� �߷� ����

        // 1. õ�� or �ٴڰ� ���˿��� �˻�(�����߷� ��ȯ�� ��� ����)
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
        // 1-1) �ٴڰ� ������ ���
        if (isContactAB)
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
        if (isContactPowerJump)
        {
            PowerJump();
        }
        else if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumpState = true;
            yVelocity = cubeJumpPower;      // yVeleocity�� cubeJumpPower ����
        }
        if (isContactAirJump)
        {
            AirJump();
        }
        dir.y = yVelocity;              // dir.y�� yVelocity ����

        // 3. cc�� ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;

    }
    // (Cube) jump�� MotinoCube 180�� ȸ�� ��� �Լ�
    void JumpTurn()
    {
        rot += cubeTurnSpeed * Time.deltaTime;
        if (rot < 180)
        {
            MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        }
        else
        { 
            jumpTurn = true;
            MotionCube.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    // (Cube) drop�� MotinoCube 90�� ȸ�� ��� �Լ�
    void DropTurn()
    {
        rot += cubeTurnSpeed * Time.deltaTime;
        if (rot < 90) MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
        else dropTurn = true;
    }

    // (CUBE & UFO & RACE) �Ŀ������븦 ������ ��, ���� �������� 1.5�� ���̷� �����Ѵ�. 
    void PowerJump()
    {
        jumpState = true;
        yVelocity = powerJumpPower;
        isContactPowerJump = false;
    }
    // (Cube & UFO) ���� ���� Intaraction�� �������� ��, 2�� ���� ���
    void AirJump()
    {
        if (Input.GetButtonDown("Jump") && jumpState)
        {
            jumpTurn = false;
            yVelocity = airJumpPower;
            rot = 0;
        }
    }
    // (Cube & UFO) �߷� ���� Potal�� �������� ��, �߷¹��� 
    void ReversGravity()
    {
        
    }


    // [Mode: UFO�� ��]
    private void UpdateUFO()
    {
        // �߷� ���� 
        gravity = DefaultGravity;

        // 1. õ��(Above)�� �ٴ�(Below) ���˿��� �˻�
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        //isYposChange = 
        
        // 1-1) �ٴ�(Below)�� ������ ��� 
        if (((cc.collisionFlags & CollisionFlags.Below) != 0))
        {
            yVelocity = 0;
        }
        yVelocity += gravity * Time.deltaTime;

        // 2. jump ��� ����
        // 2-1) PowereJump ������ ����� ��
        if (isContactPowerJump)
        {
            PowerJump();
        }
        // 2-2) Space Ű�� �Է����� �� (���� �������� ����)
        if (Input.GetKeyDown(KeyCode.Space) 
            && ((cc.collisionFlags & CollisionFlags.Above) == 0))
        {
            yVelocity = ufoJumpPower;
        }
        dir.y = yVelocity;

        // 3. Motion ���� 
        UFOMotion();

        // 4. ������ ���� 
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
    // (UFO) jump�� UFO Motino ���� ��ȭ ���
    void UFOMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir) / 3;
        if (dir.y >= 0)
        {
            MotionUFO.transform.rotation = Quaternion.Euler(angle, 0, 0);
        } 
    }


    // [Mode: Race�� ��]
    private void UpdateRace()
    {
        // �߷�����
        gravity = RaceGravity;

        // 1. Race ��忡���� dir ���ϱ�
        // 0) PowerJump ���� ���� Ȯ�� �� ����
        if (isContactPowerJump)
        {
            PowerJump();
        }
        // 1) ����Ű�� ���� �¿� �̵�
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x -= RaceSidemoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir.x += RaceSidemoveSpeed * Time.deltaTime;
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


    // [Mode: Rocket�� ��]
    // Player(Locket) ����
    private void UpdateRocket()
    {

        // �߷�����
        gravity = RocketGravity;                    // RocketGravity �߷��� gravity�� ����
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);

        // 1. Rocket ����� dir ���ϱ�
        yVelocity += gravity * Time.deltaTime;      // yVelocity�� garvity ���� 
        if (Input.GetKey(KeyCode.Space))            // space �Է� ��, yVelocitydp rocketUpPower �����ϱ�
        {
            yVelocity += rocketUpPower;
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


    // [Mode: Forward�� ��]
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + dir;
        Gizmos.DrawLine(from, to);
    }


}
