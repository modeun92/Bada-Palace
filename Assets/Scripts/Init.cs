using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Init : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        float soundValue;
        //GameMainSettingManager.GetVolumeStatic(out soundValue);
        ConfigManager.GetValue(Assets.Scripts.ConfigParameter.VOLUME, out soundValue);
        mixer.SetFloat("MasterVolume", soundValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
