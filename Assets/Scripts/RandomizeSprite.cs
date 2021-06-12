using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private bool randomizeRotation;
    [SerializeField] private bool randomizeFlip;
    private SpriteRenderer spriteRenderer;
    private int randIndex;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite(sprites);
    }

    public void SetRandomSprite(List<Sprite> sprites)
    {
        randIndex = Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[randIndex];
        if (randomizeRotation)
        {
            int randomRotation = Random.Range(0, 3);
            switch (randomRotation)
            {
                case 0:
                    transform.rotation = Quaternion.Euler(0f, 0f, 0);
                        break;
                case 1:
                    transform.rotation = Quaternion.Euler(0f, 0f, 90);
                    break;
                case 2:
                    transform.rotation = Quaternion.Euler(0f, 0f, 180);
                    break;
                case 3:
                    transform.rotation = Quaternion.Euler(0f, 0f, 270);
                    break;
            }
        }
        if (randomizeFlip)
        {
            int randomFlip = Random.Range(0, 2);
            if (randomFlip == 1)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}
