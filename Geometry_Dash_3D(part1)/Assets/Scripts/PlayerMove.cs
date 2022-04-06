using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class PlayerMove : MonoBehaviour
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
        CUBE,                                                    // 3��Ī ���� CUBE ���(Geometry Dash)
        UFO,                                                     // 3��Ī ���� UFO ���(Geometry Dash)
        RACE,                                                    // 3��Ī ���� ��� �Ʒ��� �������� RACAE ���(Beat Racer)
        SATELLITE_vertical,                                      // 3��Ī ���� SATELLITE_vertical ���(Geometry Dash)
        SATELLITE_horizontal,                                    // 3��Ī ���� ��� �Ʒ��� �����ٺ��� SATELLITE_horizontal ���
        ROCKET,                                                  // 3��Ī ���� ROCEKT ���(Geometry Dash)
        FORWARD                                                  // 3��Ī ���� ������������ ���� (VectorRush)
    }
    public ModeState Mode;
    bool changeMode = true;                                      // Mode�� ��ȭ�� ���� �� ture

    // Motion Object
    public GameObject MotionCube;
    public GameObject MotionUFO;
    public GameObject MotionRace;
    public GameObject MotionSATELLITE;
    public GameObject MotionRocket;
    public GameObject MotionForward;

    // �߷�(gravity) ���� ����
    float gravity;
    public float DefaultGravity = -9.8f;                            // Cube, UFO ����� �߷�
    public float RocketGravity = -2f;                               // Rocket ����� �߷�  

    // ����������� �����̴� �ӵ�(moveSpeed) 
    float moveSpeed = 20f;
    public float cubeMoveSpeed = 20f;                               // (Cube) ���� �̵��ӵ�
    public float ufoMoveSpeed = 20f;                                // (UFO) ���� �̵��ӵ�
    public float raceMoveSpeed = 20f;                               // (Race) ���� �̵��ӵ�
    public float raceSidemoveSpeed = 3f;                            // (Race) �¿���� �̵� �ӵ�
    public float SATELLITE_verticalMoveSpeed = 20f;                 // (SATELLITE_vertical) ���� �̵��ӵ�
    public float SATELLITE_horizontalMoveSpeed = 20f;
    public float rocketMoveSpeed = 20f;                             // (Rocket) ���� �̵��ӵ�
    public float forwardMoveSpeed = 20f;                            // (Forwardk) ���� �̵� �ӵ�

    // Motion �ӵ� ���� ����(TureSpeed)
    public float cubeTurnSpeed = 500f;                              // MotionCube ȸ�� �ӵ�

    // Jump Power(y�� +�������� �������� ��)�� ���õ� ����
    float jumpPower = 2f;
    public float cubeJumpPower = 2f;                                // (Cube) jump power 
    public float ufoJumpPower = 2f;                                 // (UFO) jump power
    public float rocketUpPower = 0.02f;                             // (Rocket) space ���� ��, ���� �ö󰡴� ��
    public float powerJumpPower = 2.2f;                             // PowerJump Object�� �������� ���� jumpPower
    public float airJumpPower = 2f;                                 // AireJump object�� �������� ���� jumpPower

    // PowerJump, AirJump ���� ����                                 
    bool isContactPowerJump = false;                                // PowerJump�� �������� �� ���¸� ��Ÿ��(true: ����, fasle: ����X)
    bool isContactAirJump = false;                                  // AirJump�� �������� �� ���¸� ��Ÿ��(true: ����, flase: ����X)

    // ���� ����                                                    
    float _yVelocity = 0f;                                          // dir�� y�� ���� velocity(gravity, jumPower �ۿ��� ���)
    float yVelocity
    {
        get { return _yVelocity; }
        set
        {
            //print("changeYVelocity : " + _yVelocity);
            if (Mode == ModeState.CUBE || Mode == ModeState.RACE ||
                Mode == ModeState.SATELLITE_horizontal)
            {
                value = Mathf.Clamp(value, -powerJumpPower, powerJumpPower);
            }
            _yVelocity = value;
        }
    }
    float angle = 0f;                                               // dir�� Vector3.forward ������ ����
    bool isContactAB = false;                                       // ��(Above), �Ʒ�(Below)���� ���˿���(true=����/false=���߿� ���ִ� ����)
    bool isContactSides = false;                                    // �¿� ���˿���(true=����/flase=����X)

    // ������Ż ���� ����
    public bool reversGravityState = false;                         // �߷� ���� ����(true: ���߷�(reverse Gravity) , false: �߷�(noraml Gravity)

    // X,Y,Z ��ǥ ��ȭ ������ ���� ����
    bool isChangeYpos;
    float currentXpos;
    float preYpos = 0;
    float currentYpos = 0;
    float preZpos = -1;                                             // Move ���� z��ǥ (Dead ���⿡ Ȱ��)    
    float currentZpos = 0;                                          // Move ���� z��ǥ (Dead ���⿡ Ȱ��)

    // RACE ����� X�� �̵���ǥ ���� ����(x��ġ�� Line���� ����)
    int LineNumber = 1;
    float posX;
    float nextXpos;

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
        // Dead ��� ����(dead == true�� ��): Play �ٽ� ����
        if (dead)
        {
            dead = false;
            SceneManager.LoadScene(0);                        // �ٽý���
        }
        if (changeMode)
        {
            SetMotion();                                             // Mode�� Mode ��� GameObject Ȱ��ȭ
            SetVar();                                                // Mode�� ����(variable) ����
            changeMode = false;
        }
        Dead();                                                      // Player�� ���� ���� �˻�

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
                CheckLineNumber();
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
    bool dead = false;                                          // daed ���� ǥ�� ����(true: ����)
    public void Dead()                                          // Player ���� ó�� �Լ� 
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
    }
    // (����) ��ֹ� �浹�� dead true ó��
    public void CrashObstacle()                                 // ��ֹ��� �ε����� ��
    {
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
        print("PowerJump");
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
