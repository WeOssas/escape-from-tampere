using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.STARTINGSCENE);
        SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync((int)SceneIndexes.MAP, LoadSceneMode.Additive);
        
    }


}
