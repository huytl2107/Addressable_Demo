using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawmer : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoint;
    [SerializeField] private GameObject enemy;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 3)
        {
            int rand = Random.Range(0,spawnPoint.Count);
            Instantiate(enemy, spawnPoint[rand]);
            time = 0;
        }
    }
}
