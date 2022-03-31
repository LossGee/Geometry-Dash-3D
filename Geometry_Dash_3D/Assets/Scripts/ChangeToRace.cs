using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToRace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove.Instance.Mode = PlayerMove.ModeState.RACE;
            PlayerMove.Instance.ChangeMode();
        }
    }
}
