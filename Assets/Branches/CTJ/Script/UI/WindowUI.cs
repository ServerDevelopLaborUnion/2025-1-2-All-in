using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
    [SerializeField] private Toggle togglebnt;
    [SerializeField] private int resolutionnum;
    [SerializeField] private WindowScreenSO SavaScreen;
    private int optionnum;
    int _width;
    int _height;
    void Start()
    {
        IntiUI();

        Screen.SetResolution(_width, _height, fullScreenMode);
    }

    void IntiUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value >= 60 &&
                (Screen.resolutions[i].width * 9 == Screen.resolutions[i].height * 16) && Screen.resolutions[i].width >= 1280 ||
                Screen.resolutions[i].refreshRateRatio.value >= 60 &&
                (Screen.resolutions[i].width * 10 == Screen.resolutions[i].height * 16) && Screen.resolutions[i].width >= 1280)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        resolutionDropdown.options.Clear();

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{item.width} x {item.height} {Convert.ToInt32(item.refreshRateRatio.value)}";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionnum;
            }
            optionnum++;
        }

        resolutionDropdown.RefreshShownValue();

        togglebnt.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionnum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void OkbtnCilck()
    {
        Screen.SetResolution(resolutions[resolutionnum].width, resolutions[resolutionnum]
            .height, fullScreenMode, resolutions[resolutionnum].refreshRateRatio);
        SaveDate();
    }

    public void SaveDate()
    {
        SavaScreen.Height = resolutions[resolutionnum].height;
        SavaScreen.Widht = resolutions[resolutionnum].width;
        SavaScreen.ScreenMode = fullScreenMode;
    }
}