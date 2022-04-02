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
        CUBE,                                           // 3��Ī ���� CUBE ���(Geometry Dash)
        UFO,                                            // 3��Ī ���� UFO ���(Geometry Dash)
        RACE,                                           // 3��Ī ���� ��� �Ʒ��� �������� RACAE ���(Beat Racer)
        SATELLITE_vertical,                             // 3��Ī ���� SATELLITE_vertical ���(Geometry Dash)
        SATELLITE_horizontal,                           // 3��Ī ���� ��� �Ʒ��� �����ٺ��� SATELLITE_horizontal ���
        ROCKET,                                         // 3��Ī ���� ROCEKT ���(Geometry Dash)
        FORWARD                                         // 3��Ī ���� ������������ ���� (VectorRush)
    }
    public ModeState Mode;
    bool changeMode = true;                             // Mode�� ��ȭ�� ���� �� ture

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionSATELLITE;
    public GameObject MotionRocket;
    public GameObject MotionForward;

    // �߷�(gravity) ���� ����
    float gravity = -9.8f;
    public float DefaultGravity = -9.8f;                 // Cube, UFO ����� �߷�
    public float RaceGravity = -9.8f;                    // Race ����� �߷�
    public float RocketGravity = -2f;                    // Rocket ����� �߷�  

    // ����������� �����̴� �ӵ�(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;                    // (Cube) ���� �̵��ӵ�
    public float ufoMoveSpeed = 20f;                     // (UFO) ���� �̵��ӵ�
    public float raceMoveSpeed = 20f;                    // (Race) ���� �̵��ӵ�
    public float raceSidemoveSpeed = 3f;                 // (Race) �¿���� �̵� �ӵ�
    public float SATELLITE_verticalMoveSpeed = 20f;      // (SATELLITE_vertical) ���� �̵��ӵ�
    public float SATELLITE_horizontalMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;                  // (Rocket) ���� �̵��ӵ�
    public float forwardMoveSpeed = 20f;                 // (Forwardk) ���� �̵� �ӵ�

    // Motion �ӵ� ���� ����(TureSpeed)
    public float cubeTurnSpeed = 500f;                   // MotionCube ȸ�� �ӵ�

    // Jump Power(y�� +�������� �������� ��)�� ���õ� ����
    float jumpPower = 2f;
    public float cubeJumpPower = 2f;                     // (Cube) jump power 
    public float ufoJumpPower = 2f;                      // (UFO) jump power
    public float raceJumpPower = 2f;
    public float rocketUpPower = 0.02f;                  // (Rocket) space ���� ��, ���� �ö󰡴� ��
    public float powerJumpPower = 2.2f;                  // PowerJump Object�� �������� ���� jumpPower
    public float airJumpPower = 2f;                      // AireJump object�� �������� ���� jumpPower

    // PowerJump, AirJump ���� ����                      
    bool isContactPowerJump = false;                     // PowerJump�� �������� �� ���¸� ��Ÿ��(true: ����, fasle: ����X)
    bool isContactAirJump = false;                       // AirJump�� �������� �� ���¸� ��Ÿ��(true: ����, flase: ����X)

    // ���� ����                                         
    float yVelocity = 0f;                                // dir�� y�� ���� velocity(gravity, jumPower �ۿ��� ���)
    float preZpos;                                       // Move ���� z��ǥ (Dead ���⿡ Ȱ��)    
    float currentZpos;                                   // Move ���� z��ǥ (Dead ���⿡ Ȱ��)
    float angle = 0f;                                    // dir�� Vector3.forward ������ ����
    bool isContactAB = false;                            // ��(Above), �Ʒ�(Below)���� ���˿���(true=����/false=���߿� ���ִ� ����)
    bool isContactSides = false;                         // �¿� ���˿���(true=����/flase=����X)

    // ������Ż ���� ����
    public bool reverseGravity = false;                  // �߷¹�������(true: �߷����� , false: reverse)

    // RACE ����� �̵���ǥ ���� ����(x��ġ ����, RacePos0~3)
    int Linenumber = 1;
    public GameObject[] racePos = new GameObject[4];     // Race Line 4
    Transform nowPos;                                    // ���� Player�� ��ġ�� Line�� ��ġ

    // dir, move�� ����� �Ǵ� ���� ����
    Vector3 dir;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        dir = Vector3.forward;
    }
    // Update is called once per frame
    void Update()
    {
        if (changeMode)
        {
            SetMotion();                                 // Mode�� Mode ��� GameObject Ȱ��ȭ
            SetVar();                                    // Mode�� ����(variable) ����
            changeMode = false;
        }

        switch (Mode)
        {
            case ModeState.CUBE: UpdateCube(); break;
            case ModeState.UFO: UpdateUFO(); break;
            case ModeState.RACE: UpdateRace(); break;
            case ModeState.SATELLITE_vertical: UpdateSATELLITE_vertical(); break;
            case ModeState.SATELLITE_horizontal: UpdateSATELLITE_horizontal(); break;
            case ModeState.ROCKET: UpdateRocket(); break;
            case ModeState.FORWARD: UpdateForward(); break;
        }
        Dead();
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
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.UFO:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(true);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.RACE:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(true);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.SATELLITE_vertical:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.SATELLITE_horizontal:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(true);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(false);
                break;
            case ModeState.ROCKET:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(true);
                MotionForward.SetActive(false);
                break;
            case ModeState.FORWARD:
                MotionCube.SetActive(false);
                MotionUFO.SetActive(false);
                MotionRace.SetActive(false);
                MotionSATELLITE.SetActive(false);
                MotionRocket.SetActive(false);
                MotionForward.SetActive(true);
                break;
        }
    }
    // (����) SetVar: ��庰 �� ������ ���� [�߷�(gravity), �̵��ӵ�(moveSpeed), jumpPower, �ʱ⼳����] 
    void SetVar()
    {
        switch (Mode)
        {
            case ModeState.CUBE:
                gravity = DefaultGravity;
                moveSpeed = cubeMoveSpeed;
                jumpPower = cubeJumpPower;
                break;
            case ModeState.UFO:
                gravity = DefaultGravity;
                moveSpeed = ufoMoveSpeed;
                jumpPower = ufoJumpPower;
                break;
            case ModeState.RACE:
                gravity = DefaultGravity;
                moveSpeed = raceMoveSpeed;
                jumpPower = raceJumpPower;
                nowPos = transform;
                SetRaceLine();
                break;
            case ModeState.SATELLITE_vertical:
                SATELLITE_verticalUpDownState = true;
                moveSpeed = SATELLITE_verticalMoveSpeed;
                break;
            case ModeState.SATELLITE_horizontal:
                gravity = DefaultGravity;
                SATELLITE_horizontalRightLeftState = true;
                moveSpeed = SATELLITE_horizontalMoveSpeed;
                break;
            case ModeState.ROCKET:
                gravity = RocketGravity;
                moveSpeed = rocketMoveSpeed;
                jumpPower = rocketUpPower;
                break;
            case ModeState.FORWARD:
                moveSpeed = forwardMoveSpeed;
                break;
        }
    }
    // (����) Player ���� ó�� �Լ�
    bool dead = false;                                    // daed ���� ǥ�� ����(true: ����)
    public void Dead()                                    // Player ���� ó�� �Լ� 
    {
        // Dead ����(����): ���� �ε����� ��(Vector3.forward �������� ��ǥ��ȭ�� ���� ��)
        if (currentZpos <= preZpos)
            dead = true;
        // Dead ����(�߰�, SATELLITE_vertical): ��/�Ʒ��� ���˵Ǿ��� ��
        if (Mode == ModeState.SATELLITE_vertical)
        {
            // ��/�Ʒ� ���� ���� �˻� 
            isContactAB = ((cc.collisionFlags & CollisionFlags.Below) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Above) != 0);
            // ���˽� dead = true
            if (isContactAB)
            {
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
                dead = true;
            }
        }
        // Dead ��� ����(dead == true�� ��): Play �ٽ� ����
        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(0);                   // �ٽý���
        }
    }
    // (����) ��ֹ� �浹�� dead true ó��
    public void CrashObstacle()                          // ��ֹ��� �ε����� ��
    {
        dead = true;
    }


    // [Mode: Cube]
    // Player(Cube) ����
    bool jumpState = false;                               // jump ����(ture: ������, false: ����X) - �̴� ���� ����
    bool jumpTurn = true;                                 // jump�� ȸ���� 180���� �����ϱ� ���� ����
    bool dropTurn = true;                                 // drop�� ȸ���� 90���� �����ϱ� ���� ����
    float rot = 0f;                                       // jump���� ����ȸ�� ���� ���� ����
    private void UpdateCube()
    {
        gravity = DefaultGravity;                         // Cube ����϶� �߷� ����

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
            yVelocity += gravity * Time.deltaTime;          // yVelocity�� �߷� ����

            if (jumpState && !jumpTurn)
            {
                JumpTurn();                                 // jump��, MotionCube 180�� ȸ��
            }
            if (!dropTurn)
            {
                DropTurn();                                 // ���Ͻ�, MotionCube 90�� ȸ��
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
            yVelocity = jumpPower;                          // yVeleocity�� cubeJumpPower ����
        }
        if (isContactAirJump)
        {
            AirJump();
        }
        dir.y = yVelocity;                                  // dir.y�� yVelocity ����

        // 3. cc�� ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
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
    // (Cube & RACE) ���� ���� Intaraction�� �������� ��, 2�� ���� ���
    void AirJump()
    {
        if (Input.GetButtonDown("Jump") && jumpState)
        {
            if (Mode == ModeState.CUBE)
            {
                jumpTurn = false;
                rot = 0;
            }
            yVelocity = airJumpPower;
        }
    }
    // (Cube & Race) AirJump Interaction�� �������� ��, isContactAirJump�� true�� �ٲ��ִ� �Լ�
    public void OnAirJump()
    {
        isContactAirJump = true;
    }
    // (Cube & Race) AirJump Instaraction�� ������ ������ ��, isContactAirJump�� false�� �ٲ��ִ� �Լ�
    public void OffAirJump()
    {
        isContactAirJump = false;
    }
    // (CUBE & UFO & RACE) �Ŀ������븦 ������ ��, ���� �������� 1.5�� ���̷� �����Ѵ�. 
    void PowerJump()
    {
        jumpState = true;
        yVelocity = powerJumpPower;
        isContactPowerJump = false;
    }
    // (Cube & UFO & Race) PowerJump Interaction�� �������� ��, isContactPowerJump�� true�� �ٲ��ִ� �Լ�
    public void ContactPowerJump()
    {
        isContactPowerJump = true;
    }
    // 

    // (Cube & UFO & Race) �߷¹��� ����
    void ReversGravity()
    {

    }


    // [Mode: UFO]
    private void UpdateUFO()
    {
        // 1. õ��(Above)�� �ٴ�(Below) ���˿��� �˻�
        // cc�� ��(Above), �Ʒ�(Below) �浹 ���� �˻�
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
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
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
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


    // [Mode: Race]
    private void UpdateRace()
    {
        //������ġ���� ���� ����� �������� �̵� 

        // 1. Race ��忡���� dir ���ϱ�(Line ��ġ�� �����ϱ�) 
        // 1) ����Ű �Է¿� ���� Line(racePos) �̵�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Linenumber > 0)
            {
                Linenumber--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Linenumber < 3)
            {
                Linenumber++;
            }
        }
        print(Linenumber);
        dir.x = racePos[Linenumber].transform.position.x - nowPos.transform.position.x;
        //  2) �߷� ����
        // õ��or����� ���˿��� Ȯ�� �� yVelocity ����
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        if (isContactAB)
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }
        dir.y = yVelocity;

        // PowerJump ���� ���� Ȯ�� �� ����
        if (isContactPowerJump)
        {
            PowerJump();
        }

        //  3) ��� ����
        RaceMotion();

        // 2. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // ������ �����ϱ� 
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
        MotionRace.transform.rotation = Quaternion.Euler(-90, -90, -90);
    }
    // (Race) ������ġ�� ���� ��� Race Line�� �ִ��� �˻�
    private void SetRaceLine()
    {
        float posX = nowPos.position.x;

        if ((posX >= -4.8f) && (posX < -2.4f))
        {
            Linenumber = 0;
        }
        else if ((posX >= -2.4f) && (posX < 0f))
        {
            Linenumber = 1;
        }
        else if ((posX >= 0f) && (posX < 2.4f))
        {
            Linenumber = 2;
        }
        else if ((posX >= -2.4f) && (posX <= 4.8f))
        {
            Linenumber = 3;
        }
    }


    // [Mode: SATELLITE_vertical]
    bool SATELLITE_verticalUpDownState = true;              // SATELLITE_vertical�� ��/�Ʒ� �������(true:�����, false:������) 
    private void UpdateSATELLITE_vertical()
    {
        // 1. ����� �Է�
        if (Input.GetKeyDown(KeyCode.Space))                // ������� SpaceŰ �Է¿� ���� ���� ��ȯ
        {
            SATELLITE_verticalUpDownState = !SATELLITE_verticalUpDownState;
        }
        // 2. dir ����
        if (SATELLITE_verticalUpDownState)
        {
            dir = new Vector3(0, 1, 1);                     // ����� 45�� ����
        }
        else
        {
            dir = new Vector3(0, -1, 1);                    // ������ 45�� ����
        }
        // 3. SATELLITE_vertical ��� ����
        SATELLITE_verticalMotion();
        // 4. ������ ����
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_vertical) SATELLITE_verticalMotion�� ��� ����
    void SATELLITE_verticalMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.y > 0)
        {
            angle = -angle;
        }
        Quaternion moveAngle = Quaternion.Euler(90 + angle, 0, 0);
        MotionSATELLITE.transform.rotation = Quaternion.Lerp(MotionSATELLITE.transform.rotation, moveAngle, 0.5f);
    }


    // [Mode: SATELLITE_horizontal]
    bool SATELLITE_horizontalRightLeftState;
    private void UpdateSATELLITE_horizontal()
    {
        // 1. ����� �Է�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SATELLITE_horizontalRightLeftState = !SATELLITE_horizontalRightLeftState;
        }

        // 2. dir ����
        // 2-1) ������� ����
        if (SATELLITE_horizontalRightLeftState)
        {
            dir = new Vector3(1, 0, 1);
        }
        else
        {
            dir = new Vector3(-1, 0, 1);
        }
        // 2-2) �߷� ����
        isContactAB = ((cc.collisionFlags & CollisionFlags.Above) != 0)
                      || ((cc.collisionFlags & CollisionFlags.Below) != 0);
        if (isContactAB)
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }
        dir.y = yVelocity;

        // 3. SATELLITE_horizontal ��� ����
        SATELLITE_horizontalMotion();

        // 4. ������ ����
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }
    // (SATELLITE_horizontal) SATELLITE_horizontalMotion ��� ����
    public void SATELLITE_horizontalMotion()
    {
        angle = Vector3.Angle(Vector3.forward, dir);
        if (dir.x < 0)
        {
            angle = -angle;
        }
        print(angle);
        Quaternion moveAngle = Quaternion.Euler(90, angle, 0);
        MotionSATELLITE.transform.rotation = Quaternion.Lerp(MotionSATELLITE.transform.rotation, moveAngle, 0.1f);
    }


    // [Mode: Rocket]
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
        dir.y = yVelocity;

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
        //print("Angle: " + angle);

        MotionRocket.transform.rotation = Quaternion.Euler(angle + 90, 0, 0);

        // 3. ������ ����(Move ����,���� z��ǥ�� �����ؼ� ��ȭ X�� Dead����)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime);          // ������ �����ϱ� 
        currentZpos = transform.position.z;
    }


    // [Mode: Forward]
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
