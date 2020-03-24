using UnityEngine.UI;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoretext;
    public Text reasonText;

    private int score;

    private const string highScore = "HighScore";
    private const string lowScore = "LowScore";

    private float messageTime;

    private void Update()
    {
        if (reasonText != null)
            reasonText.color = scoretext.color;

        if (score != Score.score)
        {
            if (reasonText != null)
            {
                reasonText.text = Score.reason;
                messageTime = Time.time + 1;
            }

            if (Score.score > score) // Adding score
            {
                scoretext.color = Color.green;

                scoretext.text = $" {score} + {Score.score - score}";
                score++;
            }

            if (Score.score < score) // Subtracting score
            {
                scoretext.color = Color.red;

                scoretext.text = $" {score} - {score - Score.score}";
                score--;
            }
        }
        else
        {
            string HighScoreCheck = score > PlayerPrefs.GetInt(highScore, 100) ? " New Highscore!" : "";
            string LowScoreCheck = score < PlayerPrefs.GetInt(lowScore, -50) ? " New Lowscore!" : "";

            scoretext.text = $" {score}{(HighScoreCheck != "" ? HighScoreCheck : LowScoreCheck)}";
            scoretext.color = Color.white;

            if (reasonText != null && messageTime < Time.time)
                reasonText.text = "";
        }
    }

    public void CallReset()
    {
        if (Score.score > PlayerPrefs.GetInt(highScore)) PlayerPrefs.SetInt(highScore, Score.score);
        if (Score.score < PlayerPrefs.GetInt(lowScore)) PlayerPrefs.SetInt(lowScore, Score.score);

        Score.Reset();
    }
}