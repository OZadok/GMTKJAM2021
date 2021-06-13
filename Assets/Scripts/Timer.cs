using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float startTime;
    private float stopTime;
    private bool isRunning;

    /// <summary>
    /// Total time in seconds
    /// </summary>
    private float TimeElapse => endTime - startTime;

    private float endTime => isRunning ? Time.time : stopTime;

    public int Minutes => (int)TimeElapse / 60;
    public int Seconds => (int)TimeElapse % 60;
    public int Centiseconds => (int)((TimeElapse - (int)TimeElapse) * 100);

    private void Start()
    {
        ResetTimer();
        GameManager.Instance.OnGameLose.AddListener(StopTimer);
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        stopTime = Time.time;
        isRunning = false;
    }
}
