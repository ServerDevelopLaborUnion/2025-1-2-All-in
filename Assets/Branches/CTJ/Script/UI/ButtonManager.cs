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
    bool _onLogin = false;

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

    public void www()
    {
        Application.OpenURL("https://www.notion.so/ALL-IN-2706f28388af80e98ec6f94c2e6bc3c0");
    }
    public void Start()
    {
        if(loginPanel == null) { return; }

        loginPanel.SetActive(false);
        
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
        if (!_onLogin)
        {
            loginPanel.SetActive(true);
            _onLogin = true;
        }
        else if(_onLogin)
        {
            loginPanel.SetActive(false);
            _onLogin = false;
        }
    }
}
