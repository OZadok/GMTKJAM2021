using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    private static CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineFramingTransposer cinemachineComposer;
    private float shakeTime;
    private static float shakeTimer;
    public Transform targetGroup;

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineComposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }


    public static void Shake(float amplitude, float frequency, float length)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        shakeTimer = length;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;
            }
        }
    }

    public void SetTarget(Transform target)
    {
        cinemachineVirtualCamera.Follow = target;
        cinemachineComposer.m_DeadZoneHeight = 0f;
        cinemachineComposer.m_DeadZoneWidth = 0f;
    }

    public void ResetTarget()
    {
        SetTarget(targetGroup);
        cinemachineComposer.m_DeadZoneHeight = 0.05f;
        cinemachineComposer.m_DeadZoneWidth = 0.05f;
    }
}
