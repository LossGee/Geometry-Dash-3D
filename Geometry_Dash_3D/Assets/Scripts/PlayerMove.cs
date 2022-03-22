using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
<구현 기능>
step1) Player를 Vector3.forward 방향으로 전진
step2) Character Controller를 활용하여 jump 기능 구현
step3) Jump시 MotionCube의 
    
 */
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 8f;
    float gravity = -9.8f;
    float yVelocity = 0f;
    public float jumpPower = 4f;
    bool jumpState = false;
    int junmTurn = 180;
    float rot = 0f;

    Vector3 dir = new Vector3(0, 0, 0);
    CharacterController cc;
    GameObject MotionCube;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        //dir = Vector3.forward;
        MotionCube = GameObject.Find("MotionCube");
    }

    // Update is called once per frame
    void Update()
    {
        print("jumpState: "+ jumpState);

        // 바닥과 맞닿아 있는지 검사
        if (cc.isGrounded)      // 바닥에 붙어있을 때
        {
            yVelocity = 0;
            jumpState = false;
        }
        else                    // 바닥과 떨어져 있을때
        {
            // yVelocity에 중력을 누적한다.
            yVelocity += gravity * Time.deltaTime;
            // jumpState가 true라면 떠있는 동안 MotinoCube 회전시키기 
            if (jumpState)
            {
                /*
                Vector3 rot = MotionCube.transform.eulerAngles;
                rot += 180 * Vector3.right * Time.deltaTime;
                if (rot.x >= 90) rot = new Vector3(-90,0,0);
                MotionCube.transform.eulerAngles = rot;
                */

                float sep = 180 / Time.deltaTime;
                rot += sep;
                MotionCube.transform.rotation = Quaternion.Euler(rot, 0, 0);
            }
        }

        // space 입력 시 jumpPower를 yVelocity에  적용
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumpState = true;
            yVelocity = jumpPower;
        }

        // dir.y에 yVelocity를 적용한다
        dir.y = yVelocity;

        // 움직이기
        cc.Move(dir * moveSpeed * Time.deltaTime);

        
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = transform.position + Vector3.up * yVelocity;
        Gizmos.DrawLine(from, to);
    }*/
}
