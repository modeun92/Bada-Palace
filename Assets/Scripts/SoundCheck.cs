using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
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
    public void Apply()
    {
        //GameMainSettingManager.SetVolumeStatic(soundValue);
        GameMainSettingManager.SetValue(Assets.Scripts.ConfigParameter.VOLUME, soundValue);
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameMainSettingManager.GetVolumeStatic(out soundValue);
        GameMainSettingManager.GetValue(Assets.Scripts.ConfigParameter.VOLUME, out soundValue);
        slider.value = soundValue;
        mixer.SetFloat("MasterVolume", soundValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
