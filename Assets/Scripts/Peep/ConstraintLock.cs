using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Animations;

public class ConstraintLock : MonoBehaviour
{
    [SerializeField] private RotationConstraint constraint;

    private void Start()
    {
        var cs = new ConstraintSource();

        var cl = GameObject.Find("ConstraintLock");
        if (cl == null)
        {
            cl = new GameObject("ConstraintLock");
        }
        
        cs.sourceTransform = cl.transform;
        cs.weight = 1;
        constraint.AddSource(cs);
    }
}
