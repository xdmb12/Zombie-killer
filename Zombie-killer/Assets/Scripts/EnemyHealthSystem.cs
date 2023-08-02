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
    private AudioSource hitSound;

    private void Awake()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        gm = gameController.GetComponent<GameManager>();
        
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
        hitSound = GetComponent<AudioSource>();
        
        health = maxHealth;
        
        RagdollOnStart();
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
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !_enemyController.isDead)
        {
            Debug.Log("Zombie Dead");
            gm.score++;
            hitSound.Play();
            _enemyController.isDead = true;
            _animator.enabled = false;
            
            MakePhysical();
            Destroy(gameObject, timeToDestroy);
        }
    }
}
