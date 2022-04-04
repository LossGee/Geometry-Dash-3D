using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// [Mode: Forward]
public partial class PlayerMove : MonoBehaviour
{
    private void UpdateForward()
    {
        // 1. Forward 모드에서의 dir 결정
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir += Vector3.up * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir -= Vector3.up * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir -= Vector3.right * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir += Vector3.right * Time.deltaTime;
        }
        else
        {
            dir = Vector3.forward;
        }
        dir.Normalize();

        // 2. 움직임 적용(Move 이전,이후 z좌표를 조사해서 변화 X시 Dead실행)
        preZpos = transform.position.z;
        cc.Move(dir * moveSpeed * Time.deltaTime); // 움직임 적용하기 
        currentZpos = transform.position.z;
    }
}
