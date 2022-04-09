using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TryCountManager : MonoBehaviour
{
    public static TryCountManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    int tryCount = 0;
    public Text tryCountText;
    public int TryCount
    {
        get { return tryCount; }
        set {
            tryCount = value;
            tryCountText.text = "YOUR TRY : " + tryCount;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        tryCountText.text = "YOUR TRY : " + tryCount;
    }

    private void Update()
    {
        
    }

    public void AddTryCount()
    {
        TryCount++;
    }
}
