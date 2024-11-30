using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip typewritterClip;
    internal static AudioManager AudioManagerInstance;
    internal static AudioSource typewritterSource;

    void Awake() {
        if (AudioManagerInstance == null) {
            AudioManagerInstance = this;
           typewritterSource =  AudioManagerInstance.AddComponent<AudioSource>();
            typewritterSource.clip = typewritterClip;
        }
    }

    internal static void PlayAudioSource(AudioSourceEnum audioType) {
        switch (audioType) { 
            case AudioSourceEnum.Typewriter:
                Debug.Log("!!!");
                typewritterSource.Play();
                break;
            default:
                Debug.LogError("Invalid audio source");
                break;
        }
    }
}

internal enum AudioSourceEnum { 
    Typewriter
}
