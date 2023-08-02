using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public float spawnTime;
    private PlayerHealthSystem _playerHealthSystem;
    public bool canSpawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _playerHealthSystem = player.GetComponent<PlayerHealthSystem>();
        SpawnZombie();
        StartCoroutine(SpawnZombieRoutine());
        _playerHealthSystem.playerDeathEvent.AddListener(StopSpawning);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void StopSpawning()
    {
        StopCoroutine(SpawnZombieRoutine());
        canSpawn = false;
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

        if (canSpawn)
        {
            StartCoroutine(SpawnZombieRoutine());
        }
    }
}
