using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public UnityEvent playerHitEvent;
    public UnityEvent playerDeathEvent;
    private AudioSource takeDamage;
    public AudioClip deathSound;
    [Range(0, 1)] public float deathAudioVolume = 0.5f;
    
    public Rigidbody[] ragdollRigidbodies;
    public float invokeDelay = 1f;
    
    private ThirdPersonController _thirdPersonController;
    private ThirdPersonShooterController _thirdPersonShooterController;
    private Animator _animator;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        takeDamage = GetComponent<AudioSource>();
        RagdollOnStart();
        health = maxHealth;
    }
    
    void PlayerDeath()
    {
        if (health < 20)
        {
            Debug.Log("Dead");
            StartCoroutine(DelayedInvokePlayerDeathEvent());
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathAudioVolume);
            
            _thirdPersonController.enabled = false;
            _thirdPersonShooterController.enabled = false;
            _animator.enabled = false;
            _characterController.enabled = false;
            
            MakePhysical();
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        playerHitEvent.Invoke();
        takeDamage.Play();
        PlayerDeath();
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
    
    private IEnumerator DelayedInvokePlayerDeathEvent()
    {
        yield return new WaitForSeconds(invokeDelay);
        playerDeathEvent.Invoke();
    }
}
