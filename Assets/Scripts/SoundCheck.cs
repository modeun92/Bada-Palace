using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
using Assets.Scripts.Global;
public class SoundCheck : MonoBehaviour, IPointerUpHandler
{
    public AudioSource sound;
    public Slider slider;
    public AudioMixer mixer;
    private float soundValue;
    public void OnPointerUp(PointerEventData eventData)
    {
        soundValue = slider.value;
        
        if (soundValue == -40f)
        {
            soundValue = -80f;
        }
        mixer.SetFloat("MasterVolume", soundValue);
        sound.Play();
    }
    // UNITY
    public void Apply()
    {
        ConfigManager.Setting.Volume = soundValue;
    }
    void Start()
    {
        soundValue = ConfigManager.Setting.Volume;
        slider.value = soundValue;
        mixer.SetFloat("MasterVolume", soundValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
