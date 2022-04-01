using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJump : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        PlayerMove.Instance.OnAirJump();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMove.Instance.OffAirJump();
    }
}
