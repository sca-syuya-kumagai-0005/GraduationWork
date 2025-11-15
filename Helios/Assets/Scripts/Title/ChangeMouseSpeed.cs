using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class ChangeMouseSpeed : MonoBehaviour
{

    public const uint SPI_SETMOUSESPEED = 0x0071;
    public const uint SPI_GETMOUSESPEED = 0x0070;

    [DllImport("User32.dll")]
    static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

    [DllImport("User32.dll")]
    static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

    [SerializeField] uint currSpeed = 0;
    [SerializeField] Slider mouseSpeedSlider;
    [SerializeField] Text speedText;

    private void Awake()
    {
        SystemParametersInfo(SPI_GETMOUSESPEED, 0, ref currSpeed, 0);
        mouseSpeedSlider.value = currSpeed;
        speedText.text = currSpeed.ToString();
    }

    public void AddMouseSpeed(float _add)
    {
        currSpeed += (uint)_add;
        if (currSpeed > mouseSpeedSlider.maxValue) currSpeed = (uint)mouseSpeedSlider.maxValue;
        else if(currSpeed < mouseSpeedSlider.minValue) currSpeed = (uint)mouseSpeedSlider.minValue;
        if(mouseSpeedSlider.value != currSpeed)
        {
            mouseSpeedSlider.value = currSpeed;
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, currSpeed, 0);
            speedText.text = currSpeed.ToString();
        }
    }

    public void SetMouseSpeed()
    {
        currSpeed = (uint)mouseSpeedSlider.value;
        SystemParametersInfo(SPI_SETMOUSESPEED, 0, currSpeed, 0);
        speedText.text = currSpeed.ToString();
    }
}
