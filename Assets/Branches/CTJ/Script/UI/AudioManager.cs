using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] SoundSetSO ssso;
    [SerializeField] AudioMixer am;

    public void SaveAudioSetingInSO()
    {
        am.GetFloat("Master", out float masterValue);
        ssso.masterValue = masterValue;

        am.GetFloat("BGM", out float bgmValue);
        ssso.bgmValue = bgmValue;

        am.GetFloat("SFX", out float sfxValue);
        ssso.sfxValue = sfxValue;
    }
}
