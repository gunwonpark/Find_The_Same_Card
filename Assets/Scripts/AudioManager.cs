using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgMusic;

    void Start()
    {
        audioSource.clip = bgMusic;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    void Update()
    {
        
    }
}
