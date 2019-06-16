using UnityEngine;

public class PickUps : MonoBehaviour
{
    public int scoreToAdd = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            Score.ModifyScore(scoreToAdd);

            //Play sound

            LowEngine.Audio.AudioManager.instance.PlayPickupCoinSound(transform.position);
        }
    }
}