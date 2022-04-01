using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    // 카메라 앵글 좌표(position, rotation 정보까지 넣어두기 
    GameObject nowAngle;
    public GameObject RightSideAnlge;      // [Cube, UFO, Rocket] default 
    public GameObject LeftSideAngle;       // [Cube, UFO, Rocket] 좌우반전
    public GameObject RaceAngle;           // [Race] default
    public GameObject ForwardAngle;        // [Forward] default

    // Position 좌표
    float xPos;
    float yPos;
    float zPos;

    Vector3 pos;
    Quaternion rot;

    void SetDefaultMode()
    {
        switch (PlayerMove.Instance.Mode)
        {
            case PlayerMove.ModeState.RACE:
                nowAngle = RaceAngle;
                break;
            case PlayerMove.ModeState.FORWARD:
                nowAngle = ForwardAngle;
                break;
            default:
                nowAngle = RightSideAnlge;
                break;
        }
    }

    void Update()
    {
        // (ver2) Player 하위의 ~Angle로 이름 지어진 Object들로 카메라 위치 바꾸기 
        // 1. Mode에 따라 nowAngle 설정 
        SetDefaultMode();
        if (nowAngle == RightSideAnlge && PlayerMove.Instance.reverseLeftRight)
        {
            nowAngle = LeftSideAngle;
        }

        // 2. nowAngle의 position, rotation 가져오기
        pos = nowAngle.transform.position;
        rot = nowAngle.transform.rotation;

        // 3. Camera 이동
        transform.position = pos;
        transform.rotation = rot;


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
