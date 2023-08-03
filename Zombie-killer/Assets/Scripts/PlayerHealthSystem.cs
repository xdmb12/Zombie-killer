using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    public float health;
    
    [Header("Events")]
    public UnityEvent playerHitEvent;
    public UnityEvent playerDeathEvent;
    
    [Header("Audio")]
    [SerializeField] private AudioSource takeDamage;
    [SerializeField] private AudioClip deathSound;
    [Range(0, 1)] [SerializeField] private float deathAudioVolume = 0.5f;
    
    [Header("Ragdoll")]
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    [SerializeField] private float invokeDelay = 1f;
    
    // Components
    private ThirdPersonController _thirdPersonController;
    private ThirdPersonShooterController _thirdPersonShooterController;
    private Animator _animator;
    private CharacterController _characterController;

    private void Start()
    {
        // Get the components
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        takeDamage = GetComponent<AudioSource>();
        
        health = maxHealth;
        
        RagdollOnStart();
    }
    
    void PlayerDeath()
    {
        if (health < 20)
        {
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
    
    private IEnumerator DelayedInvokePlayerDeathEvent()
    {
        yield return new WaitForSeconds(invokeDelay);
        playerDeathEvent.Invoke();
    }
}
