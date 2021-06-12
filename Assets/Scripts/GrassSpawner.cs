using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foliage;
    [SerializeField] private int numberOfObjects;

    void Start()
    {
        SpawnFoliage();
    }

    private void SpawnFoliage()
    {

        for (int i = 0; i < numberOfObjects; i++)
        {
            int rand_x = Random.Range(-30, 30);
            int rand_y = Random.Range(-17, 17);
            Instantiate(foliage, new Vector2(rand_x+1/16f,rand_y), Quaternion.identity, transform);
        }
    }
}