using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    void Start()
    {
        Resolution nativeRes = Screen.resolutions.Last();
        Screen.SetResolution(nativeRes.width, nativeRes.height, true);
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            Resolution nativeRes = Screen.resolutions.Last();
            Screen.SetResolution(nativeRes.width, nativeRes.height, true);
        }
        else if (val == 1)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (val == 2)
        {
            Screen.SetResolution(1366, 768, true);
        }
        else if (val == 3)
        {
            Screen.SetResolution(1280, 720, true);
        }
        else if (val == 4)
        {
            Screen.SetResolution(1024, 768, true);
        }
    }

    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
