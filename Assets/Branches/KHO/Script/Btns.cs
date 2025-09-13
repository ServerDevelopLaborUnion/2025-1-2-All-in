using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Btns : MonoBehaviour
{
    [SerializeField] private GameObject _sginupPanel;
    [SerializeField] private GameObject Dead;
    [SerializeField] private SloltMachine SloltMachine;

    private void Start()
    {
        if (_sginupPanel != null)
        {
            _sginupPanel.SetActive(false);
        }
        if (Dead != null)
        {
            Dead.SetActive(false);
        }
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

    private void Update()
    {
        if (MoneyManager.Instance.Money <= 0 && SloltMachine.maxCheck == false)
        {
            Dead.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("CTJMachine");
    }
}
