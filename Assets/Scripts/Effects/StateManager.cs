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

    [Header("Parameters"), SerializeField]
    private float startingDelay = 10f;
    [SerializeField]
    private float startFireTime = 3f;
    [SerializeField]
    private float startCriminalTime = 3f;
    [SerializeField, Tooltip("Adds random offset to all timers, ranging from +0 to this value.")]
    private float randomOffset = 2f;

    private void Awake()
    {
        Instance = this;

        stateSpreaders = new List<StateSpreader>();
        fireStateSpreaders = new List<StateSpreader>();
        crimeStateSpreaders = new List<StateSpreader>();
    }

    private void Start()
    {
        StartCoroutine(DoStart());
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

    private IEnumerator DoStart()
    {
        yield return new WaitForSeconds(startingDelay);
        StartCoroutine(EffectCoroutine(fireStateSpreaders, startFireTime));
        StartCoroutine(EffectCoroutine(crimeStateSpreaders, startCriminalTime));
    }

    private IEnumerator EffectCoroutine(List<StateSpreader> stateSpreaders, float systemTimer)
    {
        while (true)
        {
            yield return new WaitForSeconds(systemTimer + UnityEngine.Random.Range(0, randomOffset));
            print("TRIGGERED");
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
