using UnityEngine;

public class AddScoreOnAwake : MonoBehaviour
{
    public int scoreToAdd = 10;

    public string reason = "";

    private void Awake()
    {
        Score.ModifyScore(scoreToAdd, reason);
    }
}