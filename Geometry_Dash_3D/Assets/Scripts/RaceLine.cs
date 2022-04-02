using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player의 z방향 위치만을 가져와 Race모드의 Line위치를 고정시켜두고 싶다. 
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
