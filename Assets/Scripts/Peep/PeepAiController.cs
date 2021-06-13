using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepAiController : MonoBehaviour
{
    public bool toMoveForward { get; private set; }
    public float horizontal { get; private set; }

    [Header("References")]
    [SerializeField] private TrailRenderer[] tireMarks;

    [SerializeField] private StateComponent stateComponent;
    [SerializeField] private PeepMovement movement;

    [Header("Parameters")] 
    [SerializeField] private float castDistance;

    [SerializeField] private LayerMask sidewalk;

    private Vector2 ForwardDirection => transform.TransformDirection(0, 1, 0);
    private Vector2 RightDirection => transform.TransformDirection(1, 1, 0);
    private Vector2 LeftDirection => transform.TransformDirection(-1, 1, 0);

    private bool lastSideHitIsRight;
    
    private void Start()
    {
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
    }

    private void OnStateChangeEnter(State state)
    {
        switch (state)
        {
            case State.Damaged:
                movement.Speed = movement.InitialState / 2f;
                break;
            case State.Destroyed:
                movement.Speed = 0;
                break;
            default:
                movement.Speed = movement.InitialState;
                break;
        }
    }

    private void FixedUpdate()
    {
        var hitForward = GetHit(ForwardDirection);
        var hitRight = GetHit( RightDirection);
        var hitLeft = GetHit( LeftDirection);
        
        toMoveForward = !hitForward;
        horizontal = hitRight ? -1 : hitLeft ? 1 : toMoveForward ? 0 : lastSideHitIsRight ? -1 : 1;

        if (hitRight || hitLeft)
        {
            lastSideHitIsRight = hitRight;
        }
    }

    void OnDrawGizmosSelected()
    {
        DrawRay(ForwardDirection);
        DrawRay(RightDirection);
        DrawRay(LeftDirection);

        void DrawRay(Vector2 direction)
        {
            var hit = GetHit(direction);
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, direction.normalized * castDistance);
        }
    }

    private RaycastHit2D GetHit(Vector2 direction)
    {
        var notSideWalk = ~sidewalk;
        return Physics2D.Raycast(transform.position,direction, castDistance, notSideWalk);
    }
    
}
