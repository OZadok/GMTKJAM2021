using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSpreader : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private StateComponent stateComponent;
    
    [Header("Parameters")]
    [SerializeField] private LayerMask spreadGetter;
    [SerializeField] private State stateToSpread;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stateComponent.CurrentState != stateToSpread)
        {
            return;
        }
        if (((1 << other.gameObject.layer) & spreadGetter) != 0)
        {
            other.GetComponent<StateComponent>().CurrentState = stateToSpread;
        }
    }
}
