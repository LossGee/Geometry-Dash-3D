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
    public GameObject RightSideAnlge;           // [Cube, UFO, Rocket] default 
    public GameObject LeftSideAngle;            // [Cube, UFO, Rocket] 좌우반전
    public GameObject RaceAngle;                // [Race] default
    public GameObject ForwardAngle;             // [Forward] default
    public GameObject Satellite_Horizontal;     // [Forward] default

    // Revers Gravity모드이 카메라 앵글 좌표(position, rotation 정보까지 넣어두기)
    public GameObject RG_RightSideAnlge;           // [Cube, UFO, Rocket] Revers Gravity default 
    public GameObject RG_LeftSideAngle;            // [Cube, UFO, Rocket] Revers Gravity 좌우반전
    public GameObject RG_RaceAngle;                // [Race] default
    public GameObject RG_Satellite_Horizontal;     // [Forward] default

    // 좌우반전 앵글 관련 변수
    public bool reverseLeftRight = false;  // 좌우반전여부(true: LeftSideAngle, false: RightSide Angle)

    // Position 좌표
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
        // (ver2) Player 하위의 ~Angle로 이름 지어진 Object들로 카메라 위치 바꾸기 
        // 1. Mode에 따라 nowAngle 설정 
        SetDefaultMode();
        if (nowAngle == RightSideAnlge && reverseLeftRight)
        {
            nowAngle = LeftSideAngle;
        }
        if (nowAngle == RG_RightSideAnlge && reverseLeftRight)
        {
            nowAngle = RG_LeftSideAngle;
        }

        // 2. nowAngle의 position, rotation 가져오기
        pos = nowAngle.transform.position;
        rot = nowAngle.transform.rotation;

        // 3. Camera 이동
        transform.position = Vector3.Lerp(transform.position, pos, angleChangeSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, angleChangeSpeed);


        /* (ver1) 카메라 앵글을 코드로 직접 설정      
        zPos = PlayerMove.Instance.transform.position.z;     // Player의 z좌표를 가져온다. 

        // CUBE 모드 일때
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.CUBE)
        {

            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // UFO 모드 일때
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.UFO)
        {

            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // ROCKET 모드 일때
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.ROCKET)
        {

            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);

            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // RACE 모드 일 때
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.RACE)
        {

            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 17f;
            yPos = PlayerMove.Instance.transform.position.y + 17f;
            zPos = PlayerMove.Instance.transform.position.z - 10f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(30, -37, -11);

            pos.z = PlayerMove.Instance.transform.position.z - 10;
        }

        // FORWARD 모드 일때
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.FORWARD)
        {
            // 카메라 각도와 앵글 설정
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
