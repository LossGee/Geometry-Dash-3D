using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgSoundControl : MonoBehaviour
{
    public static bgSoundControl Instance;
    private void Awake() { Instance = this; }

    AudioSource bgSound;                // Background Music

    // Start is called before the first frame update
    void Start()
    {
        bgSound = gameObject.GetComponent<AudioSource>();
    }


    public void bgSoundPlay() { bgSound.Play(0); }
    public void bgSoundStop() { bgSound.Stop(); }
    public void bgSoundPause() { bgSound.Pause(); }
    public void bgSoundUnPause() { bgSound.UnPause(); }
    public void bgSoundVolumeControl(float volume){ bgSound.volume = volume; }
}
