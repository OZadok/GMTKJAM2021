using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTimeMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StateComponent stateComponent;

    [Header("Parameters")]
    [SerializeField] private State fromState;
    [SerializeField] private State toState;
    [SerializeField] private float timeToMove;
    [SerializeField] private Sound soundToPlayOnMove;

    private void Start()
    {
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
    }

    private void OnStateChangeEnter(State state)
    {
        if (state != fromState)
        {
            return;
        }
        stateComponent.OnStateChange.RemoveListener(OnStateChangeEnter);
        StartCoroutine(MoveToNextState());
        stateComponent.OnStateChange.AddListener(OnStateChangeExit);
    }
    
    private void OnStateChangeExit(State state)
    {
        stateComponent.OnStateChange.RemoveListener(OnStateChangeExit);
        StopAllCoroutines();
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
    }

    private IEnumerator MoveToNextState()
    {
        yield return new WaitForSeconds(timeToMove);
        if (soundToPlayOnMove != null)
        {
            AudioManager.instance.Play(soundToPlayOnMove, false);
            print("Played " + soundToPlayOnMove.name);
        }

        stateComponent.CurrentState = toState;
    }
}
