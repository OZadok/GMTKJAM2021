using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    private SpriteRenderer spriteRenderer;
    private int randIndex;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite(sprites);
    }

    public void SetRandomSprite(List<Sprite> sprites)
    {
        randIndex = Random.RandomRange(0, sprites.Count);
        spriteRenderer.sprite = sprites[randIndex];
    }
}
