using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalLine : MonoBehaviour
{
    public GameObject CompeletePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Complete!!!");
            SceneManager.LoadScene("MainScene");
        }
    }

}
