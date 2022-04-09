using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public void QuitButtonDown()
    {
        Application.Quit();
        print("Quit Game");
    }

    public void StartButtonDown()
    {
        SceneManager.LoadScene("PlayScene");
        print("Start Game");
    }
}
