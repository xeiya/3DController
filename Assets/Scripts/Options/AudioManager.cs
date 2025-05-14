using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    //This will hold a static (global) reference to our mixer
    private static AudioMixer staticMixer;

    private void Awake()
    {
        //Set up our static variable
        staticMixer = audioMixer;
    }
    
    
    public static void SetVolume(string channelName, float volume) 
    { 
        staticMixer.SetFloat(channelName, volume);
    }
    
    /*
    public void SetMasterVolume(float volume) 
    {
        audioMixer.SetFloat("VolumeMaster", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("VolumeMusic", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("VolumeSFX", volume);
    }
    */
}
