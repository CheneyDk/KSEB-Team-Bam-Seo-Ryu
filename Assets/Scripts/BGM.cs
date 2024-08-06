using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class BGM : MonoBehaviour
{
    public AudioClip[] BGMClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;

        var randomMusicNumber = Random.Range(0, BGMClip.Length);
        PlayClip(BGMClip[randomMusicNumber]);
    }


    public void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
