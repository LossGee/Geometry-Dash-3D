using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obsctacle : MonoBehaviour
{
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.name == "Player")
    //    {
    //        PlayerMove.Instance.dead = true;
    //        PlayerMove.Instance.Dead();
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove.Instance.dead = true;
            PlayerMove.Instance.Dead();
        }
    }
}
