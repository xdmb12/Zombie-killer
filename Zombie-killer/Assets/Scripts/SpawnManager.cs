using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public float spawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnZombie();
        StartCoroutine(SpawnZombieRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SpawnZombie()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity, gameObject.transform);
    }
    
    IEnumerator SpawnZombieRoutine()
    {
        yield return new WaitForSeconds(spawnTime);
        SpawnZombie();
        StartCoroutine(SpawnZombieRoutine());
    }
}
