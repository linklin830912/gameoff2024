using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip typewritterClip;
    [SerializeField]
    private AudioClip checkedClip;
    [SerializeField]
    private AudioClip applauseClip;
    internal static AudioManager AudioManagerInstance;
    internal static AudioSource typewritterSource;
    internal static AudioSource checkedSource;
    internal static AudioSource applauseSource;

    void Awake() {
        if (AudioManagerInstance == null) {
            AudioManagerInstance = this;
            typewritterSource =  AudioManagerInstance.AddComponent<AudioSource>();
            typewritterSource.clip = typewritterClip;
            checkedSource =  AudioManagerInstance.AddComponent<AudioSource>();
            checkedSource.clip = checkedClip;
            applauseSource =  AudioManagerInstance.AddComponent<AudioSource>();
            applauseSource.clip = applauseClip;
        }
    }

    internal static void PlayAudioSource(AudioSourceEnum audioType) {
        switch (audioType) { 
            case AudioSourceEnum.Typewriter:
                typewritterSource.Play();
                break;
            case AudioSourceEnum.Checked:
                checkedSource.Play();
                break;
            case AudioSourceEnum.Applause:
                applauseSource.Play();
                break;
            default:
                Debug.LogError("Invalid audio source");
                break;
        }
    }
}

internal enum AudioSourceEnum { 
    Typewriter,
    Checked,
    Applause
}
