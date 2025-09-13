using UnityEngine;

public class Btns : MonoBehaviour
{
    [SerializeField] private GameObject _sginupPanel;

    private void Start()
    {
        _sginupPanel.SetActive(false);
    }

    public void Qiut()
    {
        Application.Quit();
    }

    public void SginupOnOff()
    {
        if (!_sginupPanel.activeSelf)
        {
            _sginupPanel.SetActive(true);
        }
        else
        {
            _sginupPanel.SetActive(false);
        }
    }
}
