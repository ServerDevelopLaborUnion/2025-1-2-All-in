using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] SoundSetSO ssso;
    [SerializeField] AudioMixer am;

    private void OnEnable()
    {
        am.SetFloat("Master", ssso.masterValue);
        am.SetFloat("BGM", ssso.bgmValue);
        am.SetFloat("SFX", ssso.sfxValue);
    }

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
