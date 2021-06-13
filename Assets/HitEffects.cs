using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem particleSystem;

    private void Start()
    {

    }

    public void GetHit()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
