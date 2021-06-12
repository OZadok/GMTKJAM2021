using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepAnimationScript : MonoBehaviour
{
    [SerializeField] private StateComponent stateComponent;
    [SerializeField] private Animator animator;

    private void Start()
    {
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
    }

    private void OnStateChangeEnter(State state)
    {
        animator.Play(GetAnimationState(state));
    }

    private string GetAnimationState(State state)
    {
        return state switch
        {
            State.Damaged => "peep_wouded",
            State.Destroyed => "peep_dead",
            _ => "peep_walk"
        };
    }
}
