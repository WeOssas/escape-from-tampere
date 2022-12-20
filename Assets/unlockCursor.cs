using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using escapefromtampere.PlayerControl;

public class unlockCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.SetCursorLock(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
