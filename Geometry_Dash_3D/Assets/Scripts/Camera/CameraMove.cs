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
    public GameObject RightSideAnlge;                           // [Cube, UFO, Rocket] default 
    public GameObject LeftSideAngle;                            // [Cube, UFO, Rocket] �¿����
    public GameObject RaceAngle;                                // [Race] default
    public GameObject ForwardAngle;                             // [Forward] default
    public GameObject Satellite_verticalRightSideAngle;         // [Satellite_vertical default
    public GameObject Satellite_verticalLeftSideAngle;          // [Satellite_vertical �¿����
    public GameObject Satellite_horizontal;                     // [Satellite_horizontal] default

    // Revers Gravity����� ī�޶� �ޱ� ��ǥ(position, rotation �������� �־�α�)
    public GameObject RG_RightSideAnlge;                        // [Cube, UFO, Rocket] Revers Gravity default 
    public GameObject RG_LeftSideAngle;                         // [Cube, UFO, Rocket] Revers Gravity �¿����
    public GameObject RG_RaceAngle;                             // [Race] Reverse Gravity Agnle
    public GameObject RG_Satellite_horizontal;                  // [RG_Satellite_horizontal] Reverse Gravity Angle

    // �¿���� �ޱ� ���� ����
    public bool reverseLeftRight = false;                       // �¿��������(true: LeftSideAngle, false: RightSide Angle)

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
                    nowAngle = RG_Satellite_horizontal;
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
                case PlayerMove.ModeState.SATELLITE_vertical:
                    nowAngle = Satellite_verticalRightSideAngle;
                    break;
                case PlayerMove.ModeState.SATELLITE_horizontal:
                    nowAngle = Satellite_horizontal;
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

    public void reverseLeftRightAtStartPoint()
    {
        reverseLeftRight = false;
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

        // 2. �¿���� ����
        // 2-1) Cube, UFO, Rocket �¿���� 
        if (nowAngle == RightSideAnlge && reverseLeftRight)
        {
            nowAngle = LeftSideAngle;
        }
        if (nowAngle == RG_RightSideAnlge && reverseLeftRight)
        {
            nowAngle = RG_LeftSideAngle;
        }
        // 2-2) Satellite_vertical �¿����
        if (nowAngle == Satellite_verticalRightSideAngle && reverseLeftRight)
        {
            nowAngle = Satellite_verticalLeftSideAngle;
        }

        // 3. nowAngle�� position, rotation ��������
        pos = nowAngle.transform.position;
        rot = nowAngle.transform.rotation;

        // 4. Camera �̵�
        transform.position = Vector3.Lerp(transform.position, pos, angleChangeSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, angleChangeSpeed);
    }
}
