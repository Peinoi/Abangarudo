using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioAttack;
    [SerializeField] private AudioClip audioMove;
    [SerializeField] private AudioClip[] audioButton;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void ButtionClick()
    {
        audioSource.clip = audioButton[0];
        audioSource.Play();
    }

   public void Open()
    {
        audioSource.clip = audioButton[1];
        audioSource.Play();
    }
    public void Close()
    {
        audioSource.clip = audioButton[2];
        audioSource.Play();
    }

}
