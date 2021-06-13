using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepScript : MonoBehaviour
{
    [SerializeField] public StateComponent StateComponent;
    private void Start()
    {
        GameManager.Instance.Register(this);
    }
}
