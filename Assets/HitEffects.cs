using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    // Update is called once per frame
    public void GetHit()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
