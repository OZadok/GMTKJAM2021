using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskClock : MonoBehaviour
{
    public Image clockImage;
    public StateComponent StateComponent;

    private void Start()
    {
        StateComponent.OnStartFix += OnStartFix;
        StateComponent.OnStopFix += OnStopFix;
        StateComponent.OnStateEnter.AddListener(OnStateEnter);
        StateComponent.OnStateExit.AddListener(OnStateExit);
        clockImage.enabled = false;
    }

    private void OnStateEnter(State state)
    {
        if (state == State.Criminal || state == State.Damaged || state == State.OnFire)
        {
            clockImage.enabled = true;
        }
    }
    
    private void OnStateExit(State state)
    {
        if (state == State.Criminal || state == State.Damaged || state == State.OnFire)
        {
            clockImage.enabled = false;
        }
    }

    private void OnStopFix()
    {
        clockImage.fillAmount = 1;
        StopAllCoroutines();
    }

    private void OnStartFix(float timeToFix)
    {
        StartCoroutine(FixEnumerator(timeToFix));
    }

    private IEnumerator FixEnumerator(float timeToFix)
    {
        // clockImage.enabled = true;
        var startTime = Time.time;
        float fillAmount = 1 - (Time.time - startTime) / timeToFix;
        while (fillAmount > 0)
        {
            clockImage.fillAmount = fillAmount;
            yield return null;
            fillAmount = 1 - (Time.time - startTime) / timeToFix;
        }

        fillAmount = 0;
        clockImage.fillAmount = fillAmount;
        
    }
}
