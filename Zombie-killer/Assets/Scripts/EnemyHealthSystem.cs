using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float timeToDestroy;
    private EnemyController _enemyController;
    private Animator _animator;
    
    public Rigidbody[] ragdollRigidbodies;
    public UnityEvent onDeath;
    private GameManager gm;

    private void Awake()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        gm = gameController.GetComponent<GameManager>();
        
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        
        health = maxHealth;
        
        RagdollOnStart();
    }

    private void Update()
    {
        if (health <= 0 && !_enemyController.isDead)
        {
            Debug.Log("Zombie Dead");
            gm.score++;
            _enemyController.isDead = true;
            _animator.enabled = false;
            
            MakePhysical();
            Destroy(gameObject, timeToDestroy);
        }
    }

    void RagdollOnStart()
    {
        for(int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = true;
        }
    }
    
    public void MakePhysical()
    {
        for(int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = false;
        }
    }
}
