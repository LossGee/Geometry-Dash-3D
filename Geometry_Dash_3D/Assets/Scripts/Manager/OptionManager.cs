using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance;
    void Awake()
    {
        Instance = this;
    }

    public bool isPuase = false;
    public GameObject option;
    // ������ ���⿡ slide bar������ ����� �ް� SoundManager���� �Ѱ���� ��. �ϴ� ������ ���ϴ� ���Ѵ�� ����
    
    
    public void ClickContinueButton() 
    {
        print("continue Button");
        // PlayScene�� ���� ����
        SetInactiveOption();                                    // Option ȭ�� ����
        SetInactivePause();                                     // isPause = false
        SoundManager.Instance.SoundUnPause();                 // Sound �ٽ� ����
        PlayerMove.Instance.MyResume();
        Time.timeScale = 1;
    }


    public void ClickMainButton() 
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SetActiveOption() { option.SetActive(true); }
    public void SetInactiveOption() { option.SetActive(false); }

    public void SetActivePause() { 
        isPuase = true;
        option.SetActive(true);
        Time.timeScale = 0;
    }
    public void SetInactivePause() { isPuase=false; }


}
