using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    
    public static int score; //The player's score

   

    void Start()
    {
        score = 0;
    }

   
    void Update()
    {
        
    }

    public static void Reset()
    {
        score = 0;
    }

    public static void ModifyScore(int mod)
    {
        score = score + mod;
    }




}
