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
    [SerializeField] private float startingDelayFire = 5f;
    [SerializeField] private float startingDelayCrime = 5f;
    [SerializeField, Tooltip("Adds random offset to all timers, ranging from +0 to this value.")]
    private float randomOffset = 2f;

    [Header("Fire Parameters")]
    [SerializeField] private float startFireTime = 17.5f;
    [SerializeField] private float minFireTime = 10f;
    [SerializeField, Tooltip("By how many seconds to lower the next instance.")]
    private float fireSnowballEffect = 0.5f;

    [Header("Crime Parameters")]
    [SerializeField] private float startCriminalTime = 15f;
    [SerializeField] private float minCrimeTime = 7.5f;
    [SerializeField, Tooltip("By how many seconds to lower the next instance.")]
    private float crimelSnowballEffect = 0.5f;

    private void Awake()
    {
        Instance = this;

        stateSpreaders = new List<StateSpreader>();
        fireStateSpreaders = new List<StateSpreader>();
        crimeStateSpreaders = new List<StateSpreader>();
    }

    private void Start()
    {
        StartCoroutine(DoStartFire());
        StartCoroutine(DoStartCriminals());

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

    private IEnumerator DoStartFire()
    {
        yield return new WaitForSeconds(startingDelayFire);
        StartCoroutine(EffectCoroutine(fireStateSpreaders, startFireTime, minFireTime, fireSnowballEffect));
    }
    private IEnumerator DoStartCriminals()
    {
        yield return new WaitForSeconds(startingDelayCrime);
        StartCoroutine(EffectCoroutine(crimeStateSpreaders, startCriminalTime, minCrimeTime, crimelSnowballEffect));
    }


    private IEnumerator EffectCoroutine(List<StateSpreader> stateSpreaders, float systemTimer, float minimumTimer, float snowballEffect)
    {
        while (true)
        {
            stateSpreaders.Shuffle();
            foreach (var stateSpreader in stateSpreaders)
            {
                if (!stateSpreader.isSpreading)
                {
                    stateSpreader.stateComponent.CurrentState = stateSpreader.stateToSpread;
                    if (systemTimer > minimumTimer)
                    {
                        systemTimer -= snowballEffect;
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(systemTimer + UnityEngine.Random.Range(0, randomOffset));
        }
    }
}
