using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        if (val == 1)
        {
            Screen.SetResolution(1366, 768, true);
        }
        if (val == 2)
        {
            Screen.SetResolution(1280, 720, true);
        }
        if (val == 3)
        {
            Screen.SetResolution(1024, 768, true);
        }
    }

    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
