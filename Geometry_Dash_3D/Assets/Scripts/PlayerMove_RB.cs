using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_RB : MonoBehaviour
{
    public float moveSpeed = 11f;
    public float jumpPower = 3f;
    Vector3 dir = Vector3.forward;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();


        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        
    }
}
