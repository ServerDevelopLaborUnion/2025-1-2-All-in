using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    Slider slider;
    string[] soundsNums = { "Master", "BGM", "SFX" };

    bool toggle = false;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void AudioControl(int chooseSounds)
    {
        float sound = slider.value;

        if (sound == -40.0f) masterMixer.SetFloat(soundsNums[chooseSounds], -80.0f);
        else masterMixer.SetFloat (soundsNums[chooseSounds], sound);
    }

    public void ToggleNoVolme()
    {
        toggle = !toggle;
        AudioListener.volume = toggle ? 0.0f : 1.0f;
    }
}
