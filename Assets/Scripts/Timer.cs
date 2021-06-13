using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float startTime;

    /// <summary>
    /// Total time in seconds
    /// </summary>
    private float TimeElapse => Time.time - startTime;

    private int Minutes => (int)TimeElapse / 60;
    private int Seconds => (int)TimeElapse % 60;
    private int Centiseconds => (int)((TimeElapse - (int)TimeElapse) * 100);

    private void Start()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        startTime = Time.time;
    }
}
