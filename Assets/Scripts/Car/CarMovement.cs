using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rigidbody2D;
    [Tooltip("the center of mass of the car, the car will rotate around this location")]
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private CarController carController;
    [SerializeField] private CarAnimation carAnimation;
    [SerializeField] private TrailRenderer[] tireMarks;
    [SerializeField] private Transform directionArrow;
    [Header("Parameters")]
    [SerializeField] private float accelerationForce;
    [SerializeField] private float maxSpeed;
    [Tooltip("the rotation in degrees per second")]
    [SerializeField] private float turnFactor;
    [Tooltip("this force should be greater than accelerationForce")]
    [SerializeField] private float reverseMaxSpeed;

    [SerializeField] private float driftFactor = 0.6f;

    [Header("Brake")] 
    [SerializeField] private float brakeMaxDrag = 3f;
    [SerializeField] private float brakeDragLerpT = 3f;
    private bool firstBrake = true;

    [Header("Feel")]
    [SerializeField] private Sound screechSound;
    [SerializeField] private Sound speedingSound, rammingSound, rammingPeepSound;

    [Header("Collisions")]
    [SerializeField] private float speedToCollideBuilding = 10f;
    [SerializeField] private float speedToCollidePeep = 5f;

    private bool isAligned;
    private bool tierMarksOn;

    private void Awake()
    {
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        rigidbody2D.centerOfMass = centerOfMass.localPosition;
    }

    private void Start()
    {
        isAligned = false;
        RandomizeDirection();
    }

    private void RandomizeDirection()
    {
        var euler = transform.eulerAngles;
        euler.z = UnityEngine.Random.Range(0.0f, 360.0f);
        transform.eulerAngles = euler;
    }

    private void FixedUpdate()
    {
        if (carController.vertical != 0)
        {
            ApplyEngineForce(carController.vertical);
            CheckDrift(false);
        }
        else
        {
            Brake();
            CheckDrift(true);
        }

        if(carController.vertical == 0 && carController.horizontal == 0)
            AudioManager.instance.Stop(speedingSound);

        KillOrthogonalVelocity();
        ApplySteering(carController.horizontal);

        // if (carController.vertical == 0 || carController.horizontal == 0)
        // {
        //     Align();
        // }
    }

    private void CheckDrift(bool isBrake)
    {
        if (isBrake)
        {
            if (firstBrake)
            {
                firstBrake = false;
            }
            else
            {
                StartTireEmitter();
            }
        }
        else
        {
            StopTireEmitter();
            //AudioManager.instance.Stop(screechSound);
        }
    }

    private void StartTireEmitter()
    {
        if (tierMarksOn) return;

        foreach (TrailRenderer trailRenderer in tireMarks)
        {
            trailRenderer.emitting = true;
        }
        AudioManager.instance.Play(screechSound, true);
        //print("Screechin'");

        tierMarksOn = true;
    }

    private void StopTireEmitter()
    {
        if (!tierMarksOn) return;
        foreach (TrailRenderer trailRenderer in tireMarks)
        {
            trailRenderer.emitting = false;
        }

        tierMarksOn = false;
    }
    
    

    private void ApplyEngineForce(float verticalInput)
    {
        rigidbody2D.drag = 0;

        var velocityVsUp = Vector2.Dot(transform.up, rigidbody2D.velocity);

        if (velocityVsUp > maxSpeed && verticalInput > 0)
        {
            return;
        }
        if (velocityVsUp < -reverseMaxSpeed && verticalInput < 0)
        {
            return;
        }

        if (rigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && verticalInput > 0)
        {
            return;
        }
        
        var engineForceVector = transform.up * (verticalInput * accelerationForce);
        
        rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);

        AudioManager.instance.Play(speedingSound, true);

        // rigidbody2D.
        //
        // float targetSpeed;
        // var velocity = rigidbody2D.velocity;
        // var velocityMagnitude = velocity.magnitude;
        // var inputDirection = Mathf.Sign(verticalInput);
        // bool isMoving = velocityMagnitude > 0.1f;
        // bool isMovingForward = Vector2.Angle(velocity, transform.up) < 90;
        // bool isRegularAcceleration = (isMovingForward || !isMoving) && verticalInput > 0;
        // bool isReverse = (!isMovingForward || !isMoving) && verticalInput < 0;
        // bool isBrake = !isRegularAcceleration && !isReverse;
        // float forceAmountToAdd;
        //
        // if (isRegularAcceleration || isReverse)
        // {
        //     targetSpeed = isMovingForward ? maxSpeed : reverseMaxSpeed;
        //     forceAmountToAdd = -velocityMagnitude + targetSpeed;
        //     forceAmountToAdd = Mathf.Min(forceAmountToAdd, accelerationForce);
        //     var force = transform.up * (forceAmountToAdd * inputDirection);
        //     var perpDecForce = Vector3.Project(velocity, transform.right);
        //     force += -perpDecForce;
        //     rigidbody2D.AddForce(force, ForceMode2D.Force);
        // }
        // else
        // {
        //     if (velocityMagnitude <= 0.1f)
        //     {
        //         rigidbody2D.velocity = Vector2.zero;
        //     }
        //     else
        //     {
        //         if (verticalInput != 0)
        //         {
        //             forceAmountToAdd = Mathf.Max(velocityMagnitude, brakeForce);
        //         }
        //         else
        //         {
        //             forceAmountToAdd = Mathf.Min(velocityMagnitude, brakeForce);
        //         }
        //
        //         var force = -velocity.normalized * (forceAmountToAdd);
        //         rigidbody2D.AddForce(force, ForceMode2D.Force);
        //     }
        // }
        // CheckDrift(isReverse);
    }

    private void Brake()
    {
        rigidbody2D.drag = Mathf.Lerp(rigidbody2D.drag, brakeMaxDrag, Time.fixedDeltaTime * brakeDragLerpT);
    }

    private void ApplySteering(float horizontalInput)
    {
        var direction = Mathf.Sign(Vector2.Dot(rigidbody2D.velocity, transform.up));
        var minSpeedBeforeAllowTurningFactor = rigidbody2D.velocity.magnitude / (maxSpeed / 2f);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
        
        var rotationAngle = -horizontalInput * turnFactor * Time.fixedDeltaTime * minSpeedBeforeAllowTurningFactor * direction;
        var rot = Quaternion.Euler(0, 0, rotationAngle);
        var rotation = transform.rotation * rot;
        rigidbody2D.MoveRotation(rotation);
        
        // var rotationAngle = -horizontalInput * turnFactor * Time.fixedDeltaTime;
        // var rot = Quaternion.Euler(0, 0, rotationAngle);
        // var rotation = transform.rotation * rot;
        // rigidbody2D.MoveRotation(rotation);
        // rigidbody2D.velocity = rot * rigidbody2D.velocity;
        // isAligned = false;
        // directionArrow.rotation = rotation;
    }

    private void KillOrthogonalVelocity()
    {
        var up = transform.up;
        var right = transform.right;
        var forwardVelocity = up * Vector2.Dot(rigidbody2D.velocity, up);
        var rightVelocity = right * Vector2.Dot(rigidbody2D.velocity, right);

        rigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void Align()
    {
        var rotation = rigidbody2D.rotation;
        if (rotation < 360f) rotation += 360f;
        var frameAmount = carAnimation.FramesAmount;
        var segmentSize = 360f / frameAmount;

        var newAngle = Mathf.Round(rotation / (float) segmentSize) * segmentSize;
        
        rigidbody2D.MoveRotation(newAngle);
        isAligned = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidbody2D.velocity.magnitude > 0.1)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Building") && collision.gameObject.tag == "Building")
            {
                CollisionWithBuilding(collision);
            }

            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Peep"))
            {
                
                CollisionWithPeep(collision);
            }
        }
    }
    
    private void CollisionWithBuilding(Collision2D collision)
    {
        if (collision.relativeVelocity.sqrMagnitude <
            speedToCollideBuilding * speedToCollideBuilding)
        {
            return;
        }
        
        var stateComponent = collision.collider.GetComponent<StateComponent>();
        if (stateComponent)
        {
            stateComponent.CurrentState = State.Destroyed;
        }
        
        AudioManager.instance.Play(rammingSound, true);
        //CameraManager.Shake(2,5,0.1f);
        collision.gameObject.GetComponent<HitEffects>().GetHit();
    }

    private void CollisionWithPeep(Collision2D collision)
    {
        if (collision.relativeVelocity.sqrMagnitude <
            speedToCollidePeep * speedToCollidePeep)
        {
            return;
        }
        
        //change state
        var peepStateComponent = collision.collider.GetComponent<StateComponent>();
        peepStateComponent.CurrentState = State.Damaged;
        
        AudioManager.instance.Play(rammingPeepSound, false);
        CameraManager.Shake(1, 5, 0.1f);
        
        // // get impact
        // var impulse = ComputeTotalImpulse(collision);
        // collision.collider.GetComponent<Rigidbody2D>().AddForce(-impulse * 10000, ForceMode2D.Impulse);
    }
    
    static Vector2 ComputeTotalImpulse(Collision2D collision) {
        Vector2 impulse = Vector2.zero;

        int contactCount = collision.contactCount;
        for(int i = 0; i < contactCount; i++) {
            var contact = collision.GetContact(i);
            impulse += contact.normal * contact.normalImpulse;
            // impulse += contact.tangentImpulse * (Vector2)(Quaternion.Euler(0, 0, 90) *contact.normal);
            // impulse.x += contact.tangentImpulse * contact.normal.y;
            // impulse.y -= contact.tangentImpulse * contact.normal.x;
        }

        return impulse;
    }
}
