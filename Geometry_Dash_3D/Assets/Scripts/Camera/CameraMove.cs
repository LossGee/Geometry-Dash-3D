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

    // 카메라 전환 속도 (0 ~ 1 사이 설정 가능)
    public float angleChangeSpeed = 0.01f;

    // 카메라 앵글 좌표(position, rotation 정보까지 넣어두기)
    GameObject nowAngle;
    public GameObject RightSideAnlge;                           // [Cube, UFO, Rocket] default 
    public GameObject LeftSideAngle;                            // [Cube, UFO, Rocket] 좌우반전
    public GameObject RaceAngle;                                // [Race] default
    public GameObject ForwardAngle;                             // [Forward] default
    public GameObject Satellite_verticalRightSideAngle;         // [Satellite_vertical default
    public GameObject Satellite_verticalLeftSideAngle;          // [Satellite_vertical 좌우반전
    public GameObject Satellite_horizontal;                     // [Satellite_horizontal] default

    // Revers Gravity모드이 카메라 앵글 좌표(position, rotation 정보까지 넣어두기)
    public GameObject RG_RightSideAnlge;                        // [Cube, UFO, Rocket] Revers Gravity default 
    public GameObject RG_LeftSideAngle;                         // [Cube, UFO, Rocket] Revers Gravity 좌우반전
    public GameObject RG_RaceAngle;                             // [Race] Reverse Gravity Agnle
    public GameObject RG_Satellite_horizontal;                  // [RG_Satellite_horizontal] Reverse Gravity Angle

    // 좌우반전 앵글 관련 변수
    public bool reverseLeftRight = false;                       // 좌우반전여부(true: LeftSideAngle, false: RightSide Angle)

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
        // (ver2) Player 하위의 ~Angle로 이름 지어진 Object들로 카메라 위치 바꾸기 
        // 1. Mode에 따라 nowAngle 설정 
        SetDefaultMode();

        // 2. 좌우반전 적용
        // 2-1) Cube, UFO, Rocket 좌우반전 
        if (nowAngle == RightSideAnlge && reverseLeftRight)
        {
            nowAngle = LeftSideAngle;
        }
        if (nowAngle == RG_RightSideAnlge && reverseLeftRight)
        {
            nowAngle = RG_LeftSideAngle;
        }
        // 2-2) Satellite_vertical 좌우반전
        if (nowAngle == Satellite_verticalRightSideAngle && reverseLeftRight)
        {
            nowAngle = Satellite_verticalLeftSideAngle;
        }

        // 3. nowAngle의 position, rotation 가져오기
        pos = nowAngle.transform.position;
        rot = nowAngle.transform.rotation;

        // 4. Camera 이동
        transform.position = Vector3.Lerp(transform.position, pos, angleChangeSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, angleChangeSpeed);
    }
}
