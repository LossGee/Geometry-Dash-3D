using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance;
    private void Awake()
    {
        Instance = this;
    }

    // ī�޶� ��ȯ �ӵ� (0 ~ 1 ���� ���� ����)
    public float angleChangeSpeed = 0.01f;

    // ī�޶� �ޱ� ��ǥ(position, rotation �������� �־�α�)
    GameObject nowAngle;
    public GameObject RightSideAnlge;           // [Cube, UFO, Rocket] default 
    public GameObject LeftSideAngle;            // [Cube, UFO, Rocket] �¿����
    public GameObject RaceAngle;                // [Race] default
    public GameObject ForwardAngle;             // [Forward] default
    public GameObject Satellite_Horizontal;     // [Forward] default

    // Revers Gravity����� ī�޶� �ޱ� ��ǥ(position, rotation �������� �־�α�)
    public GameObject RG_RightSideAnlge;           // [Cube, UFO, Rocket] Revers Gravity default 
    public GameObject RG_LeftSideAngle;            // [Cube, UFO, Rocket] Revers Gravity �¿����
    public GameObject RG_RaceAngle;                // [Race] default
    public GameObject RG_Satellite_Horizontal;     // [Forward] default

    // �¿���� �ޱ� ���� ����
    public bool reverseLeftRight = false;  // �¿��������(true: LeftSideAngle, false: RightSide Angle)

    // Position ��ǥ
    //float xPos;
    //float yPos;
    //float zPos;

    Vector3 pos;
    Quaternion rot;

    void SetDefaultMode()
    {
        if (PlayerMove.Instance.reversGravityState)
        {
            switch (PlayerMove.Instance.Mode)
            {
                case PlayerMove.ModeState.RACE:
                    nowAngle = RG_RaceAngle;
                    break;
                case PlayerMove.ModeState.SATELLITE_horizontal:
                    nowAngle = RG_Satellite_Horizontal;
                    break;
                case PlayerMove.ModeState.FORWARD:
                    nowAngle = ForwardAngle;
                    break;
                default:
                    nowAngle = RG_RightSideAnlge;
                    break;
            }
        }
        else
        {
            switch (PlayerMove.Instance.Mode)
            {
                case PlayerMove.ModeState.RACE:
                    nowAngle = RaceAngle;
                    break;
                case PlayerMove.ModeState.SATELLITE_horizontal:
                    nowAngle = Satellite_Horizontal;
                    break;
                case PlayerMove.ModeState.FORWARD:
                    nowAngle = ForwardAngle;
                    break;
                default:
                    nowAngle = RightSideAnlge;
                    break;
            }
        }
    }

    public void ReverseSide()
    {
        reverseLeftRight = !reverseLeftRight;
    }

    void Update()
    {
        // (ver2) Player ������ ~Angle�� �̸� ������ Object��� ī�޶� ��ġ �ٲٱ� 
        // 1. Mode�� ���� nowAngle ���� 
        SetDefaultMode();
        if (nowAngle == RightSideAnlge && reverseLeftRight)
        {
            nowAngle = LeftSideAngle;
        }
        if (nowAngle == RG_RightSideAnlge && reverseLeftRight)
        {
            nowAngle = RG_LeftSideAngle;
        }

        // 2. nowAngle�� position, rotation ��������
        pos = nowAngle.transform.position;
        rot = nowAngle.transform.rotation;

        // 3. Camera �̵�
        transform.position = Vector3.Lerp(transform.position, pos, angleChangeSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, angleChangeSpeed);


        /* (ver1) ī�޶� �ޱ��� �ڵ�� ���� ����      
        zPos = PlayerMove.Instance.transform.position.z;     // Player�� z��ǥ�� �����´�. 

        // CUBE ��� �϶�
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.CUBE)
        {

            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // UFO ��� �϶�
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.UFO)
        {

            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // ROCKET ��� �϶�
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.ROCKET)
        {

            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // RACE ��� �� ��
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.RACE)
        {

            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 17f;
            yPos = PlayerMove.Instance.transform.position.y + 17f;
            zPos = PlayerMove.Instance.transform.position.z - 10f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(30, -37, -11);

            pos.z = PlayerMove.Instance.transform.position.z - 10;
        }

        // FORWARD ��� �϶�
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.FORWARD)
        {
            // ī�޶� ������ �ޱ� ����
            xPos = 0;
            yPos = 10;
            zPos = PlayerMove.Instance.transform.position.z - 18f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, 0, 0);

            pos.z = PlayerMove.Instance.transform.position.z - 18f;

        }
        transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
    */
    }
}
