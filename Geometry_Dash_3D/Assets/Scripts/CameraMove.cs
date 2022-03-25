using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zPos = target.transform.position.z;

        Vector3 pos = transform.position;
        pos.z = zPos;
        transform.position = pos;

    }
}
