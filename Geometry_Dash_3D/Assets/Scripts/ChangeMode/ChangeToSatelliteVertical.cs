using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToSatelliteVertical : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove.Instance.Mode = PlayerMove.ModeState.SATELLITE_vertical;
            PlayerMove.Instance.ChangePlayMode();
        }
    }
}
