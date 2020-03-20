using UnityEngine;

public class AddScoreOnAwake : MonoBehaviour
{
    public int scoreToAdd = 10;

    private void Awake()
    {
        Score.ModifyScore(scoreToAdd);
    }
}