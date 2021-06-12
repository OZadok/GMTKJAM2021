using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStartingFrame : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        float randomTime = Random.Range(0f, 1f);
        anim.Play(0, 0, randomTime);
    }
}
