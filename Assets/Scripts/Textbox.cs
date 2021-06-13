using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Textbox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textfield;

    public void Init(string text)
    {
        textfield.text = text;
    }
}
