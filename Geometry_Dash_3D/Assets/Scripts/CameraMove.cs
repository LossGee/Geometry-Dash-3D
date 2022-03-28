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
        zPos = PlayerMove.Instance.transform.position.z;     // Player�� z��ǥ�� �����´�. 

        // CUBE ��� �϶�
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.CUBE)
        {
            // Mode�� ó�� �ٲ���� ��, 
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            //}
            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // ROCKET ��� �϶�
        if (PlayerMove.Instance.Mode == PlayerMove.ModeState.ROCKET)
        {
            // Mode�� ó�� �ٲ���� ��, 
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 20f;
            yPos = PlayerMove.Instance.transform.position.y + 2.5f;
            zPos = PlayerMove.Instance.transform.position.z + 11f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            //}
            pos.z = PlayerMove.Instance.transform.position.z;
        }

        // RACE ��� �� ��
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.RACE)
        {
            // Mode�� ó�� �ٲ���� ��,
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // ī�޶� ������ �ޱ� ����
            xPos = PlayerMove.Instance.transform.position.x + 17f;
            yPos = PlayerMove.Instance.transform.position.y + 17f;
            zPos = PlayerMove.Instance.transform.position.z - 10f;
            pos = new Vector3(xPos, yPos, zPos);
            transform.rotation = Quaternion.Euler(30, -37, -11);
            //}
            pos.z = PlayerMove.Instance.transform.position.z - 10;
        }

        // FORWARD ��� �϶�
        else if (PlayerMove.Instance.Mode == PlayerMove.ModeState.FORWARD)
        {
            // Mode�� ó�� �ٲ���� ��,
            //if (PlayerMove.Instance.changeMode == true)
            //{
            // ī�޶� ������ �ޱ� ����
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
