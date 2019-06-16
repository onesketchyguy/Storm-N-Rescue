using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public float timeBeforeLoad = 0;
    private string levelToLoad = "GameScene";

    private void Awake()
    {
        if (timeBeforeLoad > 0)
        {
            Invoke("LoadScene", timeBeforeLoad);
        }
    }

    public void LoadScene(string level = "GameScene")
    {
        SceneManager.LoadScene(level);
    }
}