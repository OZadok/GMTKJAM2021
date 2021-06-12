using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepAiController : MonoBehaviour
{
    public bool toMoveForward { get; private set; }
    public float horizontal { get; private set; }

    [Header("Parameters")] 
    [SerializeField] private float castDistance;

    [SerializeField] private LayerMask sidewalk;

    private Vector2 ForwardDirection => transform.TransformDirection(0, 1, 0);
    private Vector2 RightDirection => transform.TransformDirection(1, 1, 0);
    private Vector2 LeftDirection => transform.TransformDirection(-1, 1, 0);

    private void FixedUpdate()
    {
        var hitForward = GetHit(ForwardDirection);
        var hitRight = GetHit( RightDirection);
        var hitLeft = GetHit( LeftDirection);
        
        toMoveForward = !hitForward;
        horizontal = hitRight ? -1 : hitLeft ? 1 : toMoveForward ? 0 : -1;
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