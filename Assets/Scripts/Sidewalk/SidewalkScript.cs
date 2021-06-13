using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewalkScript : MonoBehaviour
{
    public static List<SidewalkScript> sidewalks = new List<SidewalkScript>();

    private void Awake()
    {
        sidewalks.Add(this);
    }

    private void OnDestroy()
    {
        sidewalks.Remove(this);
    }
}
