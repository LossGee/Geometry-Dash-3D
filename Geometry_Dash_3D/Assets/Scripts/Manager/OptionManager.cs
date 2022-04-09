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
    // 원래는 여기에 slide bar에서의 제어값을 받고 SoundManager에게 넘겨줘야 함. 일단 구현이 급하니 급한대로 진행
    
    
    public void ClickContinueButton() 
    {
        print("continue Button");
        // PlayScene이 멈춘 상태
        SetInactiveOption();                                    // Option 화면 끄기
        SetInactivePause();                                     // isPause = false
        SoundManager.Instance.SoundUnPause();                 // Sound 다시 제개
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
