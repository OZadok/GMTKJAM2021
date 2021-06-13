using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeepAiController : MonoBehaviour
{
    public enum PeepState
    {
        Wander, GoToClosestSidewalk, RoadCross
    }

    [SerializeField] private PeepState peepState;

    private SidewalkScript closestSidewalkToGoTo;
    
    public bool toMoveForward { get; private set; }
    public float horizontal { get; private set; }

    [Header("References")]
    [SerializeField] private TrailRenderer[] tireMarks;

    [SerializeField] private StateComponent stateComponent;
    [SerializeField] private PeepMovement movement;

    [Header("Parameters")] 
    [SerializeField] private float castDistance;

    [SerializeField] private LayerMask sidewalk;
    [SerializeField] private LayerMask notSidewalk;
    [SerializeField] private LayerMask crosswalk;

    [SerializeField] private float timeBetweenCrosses = 10f;
    [SerializeField] private float timeToBeStuckInWander = 5f;

    private Vector2 ForwardDirection => transform.TransformDirection(0, 1, 0);
    private Vector2 RightDirection => transform.TransformDirection(1, 1, 0);
    private Vector2 LeftDirection => transform.TransformDirection(-1, 1, 0);

    private bool lastSideHitIsRight;

    private float lastTimeCrossed;

    private bool isDestroyed = false;
    
    private void Start()
    {
        stateComponent.OnStateChange.AddListener(OnStateChangeEnter);
        peepState = PeepState.Wander;
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
                toMoveForward = false;
                horizontal = 0;
                isDestroyed = true;
                break;
            default:
                movement.Speed = movement.InitialState;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDestroyed)
        {
            return;
        }
        StateHandle();
        SetMovementInput();
    }

    private void StateHandle()
    {
        switch (peepState)
        {
            case PeepState.Wander:
                WanderToCross();
                if (peepState != PeepState.Wander) break;
                OnStuck();
                
                // var hitForward = GetHitSidewalk(ForwardDirection);
                // var hitRight = GetHitSidewalk(RightDirection);
                // var hitLeft = GetHitSidewalk(LeftDirection);
                //
                // if (!(hitForward || hitRight || hitLeft))
                // {
                //     closestSidewalkToGoTo = SidewalkScript.sidewalks.Aggregate(
                //         (curMin, x) => 
                //             (curMin == null || SqrDist(x) < SqrDist(curMin) ? x : curMin));
                //     // (curMin == null || (x != null ? SqrDist(x) : float.MaxValue) < SqrDist(curMin) ? x : curMin));
                //     
                //     if (SqrDist(closestSidewalkToGoTo) > 0.25f)
                //     {
                //         peepState = PeepState.GoToClosestSidewalk;
                //     }
                // }
                break;
            case PeepState.GoToClosestSidewalk:
                if (SqrDist(closestSidewalkToGoTo) < 0.25f)
                {
                    peepState = PeepState.Wander;
                }
                break;
            case PeepState.RoadCross:
                if (SqrDist(closestSidewalkToGoTo) < 0.25f)
                {
                    peepState = PeepState.Wander;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        float SqrDist(SidewalkScript sidewalkScript)
        {
            return (sidewalkScript.transform.position - transform.position).sqrMagnitude;
        }

        void WanderToCross()
        {
            if (Time.time < lastTimeCrossed + timeBetweenCrosses)
            {
                return;
            }
            var hit = Physics2D.CircleCast(transform.position, castDistance, transform.up, 0.1f, crosswalk);
            if (hit)
            {
                closestSidewalkToGoTo =
                    hit.collider.GetComponent<CrosswalkScript>().GetFartherSidewalk(transform.position);
                peepState = PeepState.RoadCross;
                lastTimeCrossed = Time.time;
            }
        }

        void OnStuck()
        {
            var hit = Physics2D.CircleCast(transform.position, castDistance, transform.up, 0.1f, sidewalk);
            bool isNoSidewalkNear = !hit;
            bool isStuckLongTime = !stuckFlag && Time.time > stuckStartTime + timeToBeStuckInWander;
            if (isNoSidewalkNear || isStuckLongTime)
            {
                closestSidewalkToGoTo = SidewalkScript.sidewalks.Aggregate(
                    (curMin, x) => 
                        (curMin == null || (SqrDist(x) < SqrDist(curMin) && SqrDist(x) > castDistance * (castDistance + 1f)) ? x : curMin));
                // (curMin == null || (x != null ? SqrDist(x) : float.MaxValue) < SqrDist(curMin) ? x : curMin));
                
                // if (SqrDist(closestSidewalkToGoTo) > 0.25f)
                if (SqrDist(closestSidewalkToGoTo) > 0.25f)
                {
                    peepState = PeepState.GoToClosestSidewalk;
                }
            }
        }
    }

    private void SetMovementInput()
    {
        switch (peepState)
        {
            case PeepState.Wander:
                Wander();
                break;
            case PeepState.GoToClosestSidewalk:
                GoToTarget(closestSidewalkToGoTo.transform.position);
                break;
            case PeepState.RoadCross:
                GoToTarget(closestSidewalkToGoTo.transform.position);
                break;
            default:
                Wander();
                break;
        }
    }

    private void GoToTarget(Vector3 target)
    {
        var directionToTarget = -transform.position + target;
        var angle = Vector2.SignedAngle(directionToTarget, ForwardDirection);

        horizontal = Mathf.Sign(angle);
        toMoveForward = angle < 90 && angle > -90;
    }

    private bool stuckFlag = false;
    private float stuckRandDir = 1f;
    private float stuckStartTime;

    private void Wander()
    {
        // var hitForward = GetHitNotSidewalk(ForwardDirection);
        // var hitRight = GetHitNotSidewalk( RightDirection);
        // var hitLeft = GetHitNotSidewalk( LeftDirection);
        
        var hitForward = GetHitNotSidewalk(ForwardDirection);
        var hitRight = GetHitNotSidewalk( RightDirection);
        var hitLeft = GetHitNotSidewalk( LeftDirection);
        
        toMoveForward = !hitForward;

        if (toMoveForward)
        {
            horizontal = hitRight ? -1 : hitLeft ? 1 : 0;
            stuckFlag = true;
        }
        else
        {
            // can't move forward need to turn
            if (stuckFlag)
            {
                stuckFlag = false;
                stuckRandDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                stuckStartTime = Time.time;
            }
            horizontal = stuckRandDir;
        }

        // var stuckWithAll = hitForward && hitRight && hitLeft;
        // if (stuckWithAll)
        // {
        //     if (stuckWithAllFlag)
        //     {
        //         stuckWithAllFlag = false;
        //         stuckWithAllRandDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        //     }
        //     horizontal = stuckWithAllRandDir;
        // }
        // else
        // {
        //     horizontal = hitRight ? -1 : hitLeft ? 1 : toMoveForward ? 0 : lastSideHitIsRight ? -1 : 1;
        //     stuckWithAllFlag = true;
        // }
        

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
            var hit = GetHitNotSidewalk(direction);
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, direction.normalized * castDistance);
        }
    }

    private RaycastHit2D GetHitNotSidewalk(Vector2 direction)
    {
        return GetHit(direction, notSidewalk); ;
    }
    
    private RaycastHit2D GetHitSidewalk(Vector2 direction)
    {
        return GetHit(direction, sidewalk);
    }
    
    private RaycastHit2D GetHit(Vector2 direction, int mask)
    {
        return Physics2D.Raycast(transform.position,direction, castDistance, mask);
    }
    
}
