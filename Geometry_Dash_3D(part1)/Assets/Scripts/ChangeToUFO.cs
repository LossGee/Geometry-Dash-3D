using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToUFO : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove.Instance.Mode = PlayerMove.ModeState.UFO;
            PlayerMove.Instance.ChangeMode();
        }
    }
}

