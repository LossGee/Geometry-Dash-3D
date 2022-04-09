using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownManager : MonoBehaviour
{
    public static CountDownManager Instance;
    private void Awake()
    {
        Instance = this;
        textCountDown.gameObject.SetActive(true);
    }

    float currentTime;
    public float readyTime = 2.7f;
    public UnityEngine.UI.Text textCountDown;

    public void ReadyCountDown()
    {
        // Count Down
        textCountDown.gameObject.SetActive(true);

        if (currentTime >= readyTime)
        {
            currentTime = 0;
            textCountDown.gameObject.SetActive(false);
            PlayerMove.Instance.ChangeToPlayMode();

        }
        currentTime += Time.deltaTime;
        textCountDown.text = ((int)(readyTime - currentTime + 1.1f)).ToString();
    }
}
