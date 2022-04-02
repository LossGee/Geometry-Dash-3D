using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player�� z���� ��ġ���� ������ Race����� Line��ġ�� �������ѵΰ� �ʹ�. 
public class RaceLine : MonoBehaviour
{
    float playerPosZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosZ = PlayerMove.Instance.transform.position.z;
        transform.position = new Vector3(0, 0, playerPosZ);
    }
}
