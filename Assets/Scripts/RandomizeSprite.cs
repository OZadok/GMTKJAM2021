using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizeSprite : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private List<Sprite> destroyed_sprites;
    [SerializeField] private bool randomizeRotation;
    [SerializeField] private bool randomizeFlip;
    private SpriteRenderer spriteRenderer;
    private SpriteMask spriteMask;
    private int randIndex;
    
    [SerializeField] private StateComponent stateComponent;

    void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite(sprites);
        if (spriteMask != null)
        {
            spriteMask.sprite = spriteRenderer.sprite;
        }
        
        stateComponent?.OnStateChange.AddListener(OnStateChangeEnter);
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
                transform.rotation = Quaternion.Euler(0f,180f,transform.rotation.z);
            }
        }
    }

    public void DestroySprite()
    {
        spriteRenderer.sprite = destroyed_sprites[randIndex];
    }

    public void FixSprite()
    {
        spriteRenderer.sprite = sprites[randIndex];
    }
    
    private void OnStateChangeEnter(State state)
    {
        switch (state)
        {
            case State.Destroyed:
                DestroySprite();
                break;
            default:
                FixSprite();
                break;
        }
    }
}
