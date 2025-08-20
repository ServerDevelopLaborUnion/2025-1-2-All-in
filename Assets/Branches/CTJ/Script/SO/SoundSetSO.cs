using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
public class SoundSetSO : ScriptableObject
{
    public float masterValue = 0.0f;
    public float bgmValue = 0.0f;
    public float sfxValue = 0.0f;
}
