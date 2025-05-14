using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private string channelName;

    private void Start()
    {
        if (PlayerPrefs.HasKey(channelName)) 
        {
            AudioManager.SetVolume(channelName, PlayerPrefs.GetFloat(channelName));

            //Set the slider to match the current value
            GetComponent<Slider>().value = PlayerPrefs.GetFloat(channelName);
        }
    }

    public void SetVolume(float volume) 
    { 
        AudioManager.SetVolume(channelName, volume);

        PlayerPrefs.SetFloat(channelName, volume);

        PlayerPrefs.Save();
    }
}
