using UnityEngine.UI;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private Text scoretext;

    private int score;

    private void Start()
    {
        scoretext = GetComponent<Text>();
    }

    private void Update()
    {
        if (score != Score.score)
        {
            if (Score.score > score) // Adding score
            {
                scoretext.color = Color.green;

                scoretext.text = $"Score: {score} + {Score.score - score}";
                score++;
            }

            if (Score.score < score) // Subtracting score
            {
                scoretext.color = Color.red;

                scoretext.text = $"Score: {score} - {score - Score.score}";
                score--;
            }
        }
        else
        {
            scoretext.text = $"Score: {score}{(score > PlayerPrefs.GetInt("Score", 100) ? " Highscore!" : "")}";
            scoretext.color = Color.white;
        }
    }

    public void CallReset()
    {
        if (Score.score > PlayerPrefs.GetInt("Score", 100)) PlayerPrefs.SetInt("Score", Score.score);

        Score.Reset();
    }
}