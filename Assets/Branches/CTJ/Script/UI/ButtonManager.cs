using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject settingPanel;
    [SerializeField] private GameObject loginPanel;

    bool _onMenu = false;
    bool _onSettingPanel = false;

    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnOffMenu();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnOffMenu()
    {
        if(!_onMenu)
        {
            menuPanel.SetActive(true);
            _onMenu = true;
        }
        else if(_onMenu)
        {
            settingPanel.SetActive(false);
            menuPanel.SetActive(false);
            _onMenu = false;
            _onSettingPanel = false;
        }
    }
    public void Starts()
    {

    }
    public void OnOffSettingPanel()
    {
        if(!_onSettingPanel)
        {
            settingPanel.SetActive(true);
            _onSettingPanel= true;
        }
        else if(_onSettingPanel)
        {
            settingPanel.SetActive(false);
            _onSettingPanel = false;
        }
    }

    public void OnLogin()
    {
        if (!loginPanel)
        {
            loginPanel.SetActive(true);
        }
        else
        {
            loginPanel.SetActive(false);
        }
    }
}
