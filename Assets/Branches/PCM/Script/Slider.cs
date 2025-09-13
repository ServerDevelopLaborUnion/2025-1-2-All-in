using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Slidr : MonoBehaviour
{
    [SerializeField] private string soundName;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;

    private AudioSource _aud;

    private void Awake()
    {
        _aud = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetLevel(slider.value); // 슬라이더 값으로 초기 설정
    }

    public void SetLevel(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat(soundName, dB);
    }
}


