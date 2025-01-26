using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load saved volume settings
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);

        // Apply saved values to sliders and audio sources
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;

        musicSource.volume = savedMusicVolume;
        sfxSource.volume = savedSFXVolume;

        // Add listeners to sliders
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    private void UpdateMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    private void UpdateSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
