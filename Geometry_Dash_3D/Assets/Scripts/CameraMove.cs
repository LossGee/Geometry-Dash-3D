using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    float xPos;
    float yPos;
    float zPos;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        zPos = PlayerMove.Instance.transform.position.z;     // Player의 z좌표를 가져온다. 

        // CUBE 모드 일때
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.CUBE)
        {
            // Mode가 처음 바뀌었을 때, 
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            //}
            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // ROCKET 모드 일때
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.ROCKET)
        {
            // Mode가 처음 바뀌었을 때, 
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            //}
            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // RACE 모드 일 때
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.RACE)
        {
            // Mode가 처음 바뀌었을 때,
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // 카메라 각도와 앵글 설정
            xPos = PlayerMove.Instance.transform.position.x + 17f;
            yPos = PlayerMove.Instance.transform.position.y + 17f;
            zPos = PlayerMove.Instance.transform.position.z - 10f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(30, -37, -11);
            //}
            pos.z = PlayerMove.Instance.transform.position.z - 10;
        }

        // FORWARD 모드 일때
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.FORWARD)
        {
            // Mode가 처음 바뀌었을 때,
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // 카메라 각도와 앵글 설정
            xPos = 0;
            yPos = 10;
            zPos = PlayerMove.Instance.transform.position.z - 18f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //}
            pos.z = PlayerMove.Instance.transform.position.z - 18f;
            
        }
        transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
    }
}
