using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthSystem : MonoBehaviour
{
    [Header("Health")]
    private float health;
    [SerializeField] private float maxHealth;
    
    [Header("Death")]
    [SerializeField] private float timeToDestroy;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    public UnityEvent onDeath;
    
    // Components
    private EnemyController _enemyController;
    private Animator _animator;
    private GameManager gm;
    private AudioSource hitSound;

    private void Awake()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        gm = gameController.GetComponent<GameManager>();
        
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        hitSound = GetComponent<AudioSource>();
        
        RagdollOnStart();
        health = maxHealth;
    }

    private void RagdollOnStart()
    {
        for(int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = true;
        }
    }
    
    private void MakePhysical()
    {
        for(int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].isKinematic = false;
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !_enemyController.isDead)
        {
            gm.score++;
            hitSound.Play();
            
            _enemyController.isDead = true;
            _animator.enabled = false;
            
            MakePhysical();
            
            Destroy(gameObject, timeToDestroy);
        }
    }
}
