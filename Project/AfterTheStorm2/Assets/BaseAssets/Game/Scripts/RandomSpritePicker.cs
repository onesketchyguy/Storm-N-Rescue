using UnityEngine;

public class RandomSpritePicker : MonoBehaviour
{
    public Sprite[] sprites;
    public new SpriteRenderer renderer;

    private void Start()
    {
        if (renderer == null)
            renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}