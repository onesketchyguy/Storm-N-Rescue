using UnityEngine;
using UnityEngine.UI;

public class GameOverWinOrLose : MonoBehaviour
{
    public Text TextObject;

    [TextArea(2, 5)]
    public string Win;

    [TextArea(2, 5)]
    public string lose;

    private void Start()
    {
        if (Score.score > -1)
        {
            TextObject.text = Win;
        }
        else
        {
            TextObject.text = lose;
        }
    }
}