using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerJump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMove.Instance.ContactPowerJump();
    }
}
