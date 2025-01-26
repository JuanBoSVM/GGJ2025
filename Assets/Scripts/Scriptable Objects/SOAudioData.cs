using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/Audios")]
public class SOAudioData : ScriptableObject
{
    [Header("Audio Clips")]
    [Space(10)]

    public AudioClip _bubbleShot;
    public AudioClip _bubblePop;
    public AudioClip _dash;
    public AudioClip _damage;
    public AudioClip _death;
    public AudioClip _music;
    public AudioClip _victory;
    public AudioClip _click;
    public AudioClip _parry;

    [Space(10)]
    [Header("Audio Volumes")]
    [Space(15)]

    [Range(0.0f, 1.0f)]
    public float _MusicVolume;

    [Range(0.0f, 1.0f)]
    public float _SFXVolume;
}
