using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class AudioManager : MonoBehaviour
{
    [Foldout("AudioClip")]
    [SerializeField] AudioClip playerDamagedClip;
    [SerializeField] AudioClip normalEnemyDamagedClip;
    [SerializeField] AudioClip heavyEnemyDamagedClip;

    [Foldout("Volume")]
    [SerializeField][Range(0f, 1f)] float playerDamagedVolume = 1f;
    [SerializeField][Range(0f, 1f)] float normalEnemyDamagedVolume = 1f;
    [SerializeField][Range(0f, 1f)] float heavyEnemyDamagedVolume = 1f;
    [EndFoldout]

    static AudioManager instance;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayerDamagedClip()
    {
        PlayClip(playerDamagedClip, playerDamagedVolume);
    }

    public void NormalEnemyDamagedClip()
    {
        PlayClip(normalEnemyDamagedClip, normalEnemyDamagedVolume);
    }

    public void HeavyEnemyDamagedClip()
    {
        PlayClip(heavyEnemyDamagedClip, heavyEnemyDamagedVolume);
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}