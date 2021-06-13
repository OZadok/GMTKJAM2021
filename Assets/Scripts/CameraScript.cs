using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float DESIGN_WIDTH = 480;
    private float DESIGN_HEIGHT = 270;

    private float PPU = 8; 
    
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;


    private void Start()
    {
        InitiateBaseSize(Screen.width, Screen.height);
    }

    private void InitiateBaseSize(float width, float height)
    {
        float orthographicSize;
            
        // Set resolution
        float windowAspect = width / height;
        float targetAspect = DESIGN_WIDTH / DESIGN_HEIGHT;
        float scaleHeight = windowAspect / targetAspect;

        // Debug.Log("windowAspect: " + windowAspect + ", targetAspect: " + targetAspect);

        if (windowAspect < targetAspect)
        {
            orthographicSize = (DESIGN_HEIGHT / (2f * PPU)) / scaleHeight;
        }
        else
        {
            orthographicSize = DESIGN_HEIGHT / (2f * PPU);
        }

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
