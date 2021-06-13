using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkScript : MonoBehaviour
{
    public static List<CrosswalkScript> crosswalks = new List<CrosswalkScript>();

    public LayerMask SidewalkMask;
    
    public SidewalkScript UpSidewalk;
    public SidewalkScript DownSidewalk;

    private void Awake()
    {
        crosswalks.Add(this);
        SetSidewalks();
    }

    private void OnDestroy()
    {
        crosswalks.Remove(this);
    }

    private void SetSidewalks()
    {
        var hitUp = Physics2D.Raycast(transform.position ,transform.up, 1000 , SidewalkMask);
        var hitDown = Physics2D.Raycast(transform.position ,-transform.up, 1000 , SidewalkMask);

        UpSidewalk = hitUp.collider.GetComponent<SidewalkScript>();
        DownSidewalk = hitDown.collider.GetComponent<SidewalkScript>();
    }

    public SidewalkScript GetFartherSidewalk(Vector3 position)
    {
        var upSqrDist = (position - UpSidewalk.transform.position).sqrMagnitude;
        var downSqrDist = (position - DownSidewalk.transform.position).sqrMagnitude;
        return (upSqrDist > downSqrDist) ? UpSidewalk : DownSidewalk;
    }
}
