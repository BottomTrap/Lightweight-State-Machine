using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip mainMenuAudio;
    public AudioClip levelAudio;

    private void Awake()
    {
        audioSource = (AudioSource)GetComponent(typeof(AudioSource));
        audioSource.time = 0.32f;
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            audioSource.clip = levelAudio;
            
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
