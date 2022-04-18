using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionalMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    //чтобы dotween работал
    private void OnEnable()
    {
        Time.timeScale = 1;
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume",Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualitylevel)
    {
        QualitySettings.SetQualityLevel(qualitylevel);
    }

    public void Sound()
    {
        AudioListener.pause = !AudioListener.pause;
    }
}
