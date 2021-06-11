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
    [Header("Parameters")]
    [SerializeField] private float accelerationForce;
    [SerializeField] private float maxSpeed;
    [Tooltip("the rotation in degrees per second")]
    [SerializeField] private float turnFactor;

    private void Awake()
    {
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        rigidbody2D.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce(carController.moveForward);
        ApplySteering(carController.horizontal);
    }

    private void ApplyEngineForce(bool input)
    {
        if (!input)
        {
            return;
        }
        var forceAmountToAdd = -rigidbody2D.velocity.magnitude + maxSpeed;
        forceAmountToAdd = Mathf.Min(forceAmountToAdd, accelerationForce);
        var force = transform.up * forceAmountToAdd;
        rigidbody2D.AddForce(force, ForceMode2D.Force);
    }

    private void ApplySteering(float horizontalInput)
    {
        var rotationAngle = -horizontalInput * turnFactor * Time.fixedDeltaTime;
        var rot = Quaternion.Euler(0, 0, rotationAngle);
        var rotation = transform.rotation * rot;
        rigidbody2D.MoveRotation(rotation);
        rigidbody2D.velocity = rot * rigidbody2D.velocity;
    }
}
