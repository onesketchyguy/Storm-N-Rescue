using UnityEngine.UI;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
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