using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove.Instance.Mode = PlayerMove.ModeState.CUBE;
            PlayerMove.Instance.ChangePlayMode();
        }
    }
}
