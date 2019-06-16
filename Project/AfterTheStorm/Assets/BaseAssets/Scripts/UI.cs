using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private Text scoretext;

    private void Start()
    {
        scoretext = GetComponent<Text>();
    }

    private void Update()
    {
        scoretext.text = "Score: " + Score.score;
    }

    public void CallReset()
    {
        Score.Reset();
    }
}