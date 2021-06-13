using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerVisual : MonoBehaviour
{
    [SerializeField] Timer timer;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = $"{timer.Minutes.ToString("00")}:{timer.Seconds.ToString("00")}:{timer.Centiseconds.ToString("00")}";
    }
}
