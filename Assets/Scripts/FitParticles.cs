using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem.ShapeModule particleSystemShape;
    private Sprite sprite;

    void Start()
    {
        particleSystemShape = GetComponent<ParticleSystem>().shape;
        sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
        MatchParticleShapeSize(sprite);
    }

    public void MatchParticleShapeSize(Sprite sprite)
    {
        transform.localPosition = new Vector3(0f, 0f, 0f);
        var size_x = sprite.rect.width / 8f - 1/4f; //find sprite width, divided by pixels per unit. -1/4f to shrink the size a bit more.
        var size_y = sprite.rect.height / 8f - 1/4f; //find sprite width, divided by pixels per unit. -1/4f to shrink the size a bit more.
        var offset_y = (sprite.rect.height/2f - sprite.pivot.y)/8f; //find actual center of sprite as opposed to pivot.
        particleSystemShape.scale = new Vector3(size_x, size_y, 0f);
        particleSystemShape.position = new Vector3(0f, offset_y, 0f);
    }
}
