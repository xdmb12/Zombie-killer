using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Zombie Prefab")]
    [SerializeField] private GameObject zombiePrefab;
    
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnTime;
    
    // Private Variables
    private PlayerHealthSystem _playerHealthSystem;
    private bool canSpawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _playerHealthSystem = player.GetComponent<PlayerHealthSystem>();
        
        SpawnZombie();
        StartCoroutine(SpawnZombieRoutine());
        
        _playerHealthSystem.playerDeathEvent.AddListener(StopSpawning);
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
