using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;
    
    private List<StateSpreader> stateSpreaders;
    private List<StateSpreader> fireStateSpreaders;
    private List<StateSpreader> crimeStateSpreaders;

    [Header("Parameters")] 
    // private float timeToStartFire = 3f;
    private float timeToStartEffect = 3f;

    private void Awake()
    {
        Instance = this;

        stateSpreaders = new List<StateSpreader>();
        fireStateSpreaders = new List<StateSpreader>();
        crimeStateSpreaders = new List<StateSpreader>();
    }

    private void Start()
    {
        StartCoroutine(EffectCoroutine(fireStateSpreaders));
        StartCoroutine(EffectCoroutine(crimeStateSpreaders));
    }

    public void Sign(StateSpreader stateSpreader)
    {
        stateSpreaders.Add(stateSpreader);
        if (stateSpreader.stateToSpread == State.OnFire)
        {
            fireStateSpreaders.Add(stateSpreader);
        }
        else if (stateSpreader.stateToSpread == State.Criminal)
        {
            crimeStateSpreaders.Add(stateSpreader);
        }
    }

    private IEnumerator EffectCoroutine(List<StateSpreader> stateSpreaders)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToStartEffect);
            stateSpreaders.Shuffle();
            foreach (var stateSpreader in stateSpreaders)
            {
                if (!stateSpreader.isSpreading)
                {
                    stateSpreader.stateComponent.CurrentState = stateSpreader.stateToSpread;
                    break;
                }
            }
        }
    }
}
