using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource theme;
    [SerializeField] private AudioClip[] swingSound;
    [SerializeField] private AudioClip whistle;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PLayTheme();
    }

    public void StopTheme()
    {
        theme.Stop();
    }

    public void PLayTheme()
    {
        theme.Play();
    }

    public void PlayRandomSwingSound()
    {
        audioSource.PlayOneShot(swingSound[Random.Range(0, swingSound.Length)]);
    }

    public void PlayWhistleSound()
    {
        audioSource.PlayOneShot(whistle);
    }
}
