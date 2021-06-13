using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PeepMovement : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private PeepAiController peepController;
    [FormerlySerializedAs("speed")]
    [Header("Parameters")]
    [SerializeField] private float initialSpeed;
    [Tooltip("the rotation in degrees per second")]
    [SerializeField] private float turnFactor;

    public float InitialState => initialSpeed;
    public float Speed { get; set; }

    private void Awake()
    {
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        Speed = initialSpeed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        MoveForward(peepController.toMoveForward);
        Rotate(peepController.horizontal);
    }

    private void MoveForward(bool toMove)
    {
        var velocity = toMove ? transform.up * Speed : Vector3.zero;
        rigidbody2D.velocity = velocity;
    }

    private void Rotate(float horizontalInput)
    {
        var rotationAngle = -horizontalInput * turnFactor * Time.fixedDeltaTime;
        var rot = Quaternion.Euler(0, 0, rotationAngle);
        var rotation = transform.rotation * rot;
        rigidbody2D.MoveRotation(rotation);
        rigidbody2D.velocity = rot * rigidbody2D.velocity;
    }
}
