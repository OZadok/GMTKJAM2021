using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntroLine
{
    [TextArea]
    public string dialogueLine = "";
    [Range(0,4)]
    public int slideIndex = 0;
}
