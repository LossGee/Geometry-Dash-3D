using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseSidePotal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CameraMove.Instance.ReverseSide();
    }
}
