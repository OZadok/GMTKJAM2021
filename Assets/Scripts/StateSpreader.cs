using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateSpreader : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] public StateComponent stateComponent;
    [SerializeField] private Collider2D spreadCollider;
    
    [Header("Parameters")]
    // [SerializeField] private LayerMask spreadGetter;
    [SerializeField] public State stateToSpread;
    [SerializeField] private ContactFilter2D contactFilter2D;
    
    [SerializeField] private float minTimeToSpread = 5f;
    [SerializeField] private float maxTimeToSpread = 10f;

    [SerializeField] private bool canStartEffect;
    public bool isSpreading => stateComponent.CurrentState == stateToSpread;
    public bool canSpread => stateComponent.CanMoveToState(stateToSpread);

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (stateToSpread == State.OnFire && other.gameObject.layer == LayerMask.NameToLayer("Building"))
    //     {
    //         Debug.Log("OnTriggerEnter2D Building");
    //     }
    //     if (stateComponent.CurrentState != stateToSpread)
    //     {
    //         return;
    //     }
    //     if (((1 << other.gameObject.layer) & spreadGetter) != 0)
    //     {
    //         other.GetComponent<StateComponent>().CurrentState = stateToSpread;
    //     }
    // }

    private void Start()
    {
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
        if (canStartEffect)
        {
            StateManager.Instance.Sign(this);
        }
    }

    private void OnStateChangeEnter(State state)
    {
        if (state != stateToSpread)
        {
            return;
        }
        stateComponent.OnStateChange.RemoveListener(OnStateChangeEnter);
        StartCoroutine(SpreadCoroutine());
        stateComponent.OnStateChange.AddListener(OnStateChangeExit);
    }
    
    private void OnStateChangeExit(State state)
    {
        stateComponent.OnStateChange.RemoveListener(OnStateChangeExit);
        StopAllCoroutines();
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
    }

    private IEnumerator SpreadCoroutine()
    {
        while (true)
        {
            var timeToSpread = Random.Range(minTimeToSpread, maxTimeToSpread);
            yield return new WaitForSeconds(timeToSpread);
            var isSpread = Spread();
        }
    }

    [ContextMenu("Spread")]
    private bool Spread()
    {
        var value = false;
        var queriesHitTriggers = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        
        var results = new List<Collider2D>();
        var amount = spreadCollider.OverlapCollider(contactFilter2D, results);
        if (amount > 0)
        {
            results.Shuffle();
            foreach (var result in results)
            {
                var sc = result.GetComponent<StateComponent>();
                if (sc && sc.CanMoveToState(stateToSpread))
                {
                    sc.CurrentState = stateToSpread;
                    
                    value = true;
                    break;
                }
            }
        }

        Physics2D.queriesHitTriggers = queriesHitTriggers;
        return value;
    }
}
