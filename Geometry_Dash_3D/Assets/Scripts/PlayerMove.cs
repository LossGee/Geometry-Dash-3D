using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
<���� ���>
step1) Player�� Vector3.forward �������� ����
step2) Character Controller�� Ȱ���Ͽ� jump ��� ����
step3) Jump�� MotionCube�� 
    
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

        // �ٴڰ� �´�� �ִ��� �˻�
        if (cc.isGrounded)      // �ٴڿ� �پ����� ��
        {
            yVelocity = 0;
            jumpState = false;
        }
        else                    // �ٴڰ� ������ ������
        {
            // yVelocity�� �߷��� �����Ѵ�.
            yVelocity += gravity * Time.deltaTime;
            // jumpState�� true��� ���ִ� ���� MotinoCube ȸ����Ű�� 
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

        // space �Է� �� jumpPower�� yVelocity��  ����
        if (Input.GetButtonDown("Jump") && !jumpState)
        {
            jumpState = true;
            yVelocity = jumpPower;
        }

        // dir.y�� yVelocity�� �����Ѵ�
        dir.y = yVelocity;

        // �����̱�
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
