using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    Text scoretext;

    void Start()
    {
        scoretext = GetComponent<Text>();
    }


    void Update()
    {
        scoretext.text = "Score: " + Score.score;
    }
}
