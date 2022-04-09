using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBarManager : MonoBehaviour
{
    float progressPercent = 0;
    float totalLeght;
    float progressedLength = 0;

    public Transform startPos;
    public Transform playerPos;
    public Transform goalPos;

    public Text progressBarText;
    Slider progressdBar;


    // Start is called before the first frame update
    void Start()
    {
        totalLeght = goalPos.position.z - startPos.position.z;
        progressdBar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        progressedLength = playerPos.position.z - startPos.position.z;
        progressPercent = progressedLength / totalLeght;

        progressdBar.value = progressPercent;
        progressBarText.text = (int)(progressPercent*100) + " %";
    }
}
