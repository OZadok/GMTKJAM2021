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
    [SerializeField] private float brakeForce;
    [SerializeField] private float reverseMaxSpeed;

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
    }

    private void FixedUpdate()
    {
        ApplyEngineForce(carController.vertical);
        if (rigidbody2D.velocity.magnitude != 0)
        {
            ApplySteering(carController.horizontal);
        }

        if (carController.vertical == 0 || carController.horizontal == 0)
        {
            Align();
        }
    }

    private void CheckDrift(bool isBrake)
    {
        if (isBrake)
        {
            StartTireEmitter();
        }
        else
        {
            StopTireEmitter();
        }
    }

    private void StartTireEmitter()
    {
        if (tierMarksOn) return;
        foreach (TrailRenderer trailRenderer in tireMarks)
        {
            trailRenderer.emitting = true;
        }

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
        float targetSpeed;
        var velocity = rigidbody2D.velocity;
        var velocityMagnitude = velocity.magnitude;
        var inputDirection = Mathf.Sign(verticalInput);
        bool isMoving = velocityMagnitude > 0.1f;
        bool isMovingForward = Vector2.Angle(velocity, transform.up) < 90;
        bool isRegularAcceleration = (isMovingForward || !isMoving) && verticalInput > 0;
        bool isReverse = (!isMovingForward || !isMoving) && verticalInput < 0;
        bool isBrake = !isRegularAcceleration && !isReverse;
        float forceAmountToAdd;
        
        if (isRegularAcceleration || isReverse)
        {
            targetSpeed = isMovingForward ? maxSpeed : reverseMaxSpeed;
            forceAmountToAdd = -velocityMagnitude + targetSpeed;
            forceAmountToAdd = Mathf.Min(forceAmountToAdd, accelerationForce);
            var force = transform.up * (forceAmountToAdd * inputDirection);
            var perpDecForce = Vector3.Project(velocity, transform.right);
            force += -perpDecForce;
            rigidbody2D.AddForce(force, ForceMode2D.Force);
        }
        else
        {
            if (velocityMagnitude <= 0.1f)
            {
                rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                if (verticalInput != 0)
                {
                    forceAmountToAdd = Mathf.Max(velocityMagnitude, brakeForce);
                }
                else
                {
                    forceAmountToAdd = Mathf.Min(velocityMagnitude, brakeForce);
                }

                var force = -velocity.normalized * (forceAmountToAdd);
                rigidbody2D.AddForce(force, ForceMode2D.Force);
            }
        }
        CheckDrift(isReverse);
    }

    private void ApplySteering(float horizontalInput)
    {
        var rotationAngle = -horizontalInput * turnFactor * Time.fixedDeltaTime;
        var rot = Quaternion.Euler(0, 0, rotationAngle);
        var rotation = transform.rotation * rot;
        rigidbody2D.MoveRotation(rotation);
        rigidbody2D.velocity = rot * rigidbody2D.velocity;
        isAligned = false;
        directionArrow.rotation = rotation;
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
}
