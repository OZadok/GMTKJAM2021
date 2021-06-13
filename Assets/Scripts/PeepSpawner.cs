using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeepSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject peep;
    [SerializeField] Transform peepHolder;
    [SerializeField] Transform pavementHolder;

    [Header("Parameters")]
    [SerializeField] private int numberOfPeeps;

    private List<Transform> pavements;
    private int index = 0;

    void Start()
    {
        List<Transform> pavements = new List<Transform>();

        foreach (Transform pavement in pavementHolder)
        {
            pavements.Add(pavement);
        }
        for (int i = 0; i < pavements.Count; i++)
        {
            Transform temp = pavements[i];
            int randomIndex = Random.Range(i, pavements.Count);
            pavements[i] = pavements[randomIndex];
            pavements[randomIndex] = temp;
        }
        foreach (Transform pavement in pavements)
        {
            Instantiate(peep, pavement.position, Quaternion.identity, peepHolder);
            index++;
            if (index >= numberOfPeeps)
            {
                break;
            }
        }    
    }
}
