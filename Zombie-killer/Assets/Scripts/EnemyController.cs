using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float damage;
    public float attackCooldown;
    public float attackDistance;
    public LayerMask whatIsPlayer;
    
    // Variables
    private bool alreadyAttacked;
    private bool playerInAttackRange;
    [HideInInspector] public bool isDead;
    
    // References
    private Transform _player;
    private Vector3 target;
    
    // Components
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private PlayerHealthSystem _playerHealth;
    private EnemyHealthSystem _enemyHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get references
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;
        _playerHealth = player.GetComponent<PlayerHealthSystem>();
        _enemyHealth = GetComponent<EnemyHealthSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        // Add listeners
        _enemyHealth.onDeath.AddListener(ZombieDeath);
        _playerHealth.playerDeathEvent.AddListener(PlayerDeath);
        
        // Start coroutines
        StartCoroutine(UpdateForTarget());
    }
    
    
    void FixedUpdate()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position + Vector3.up, attackDistance, whatIsPlayer);
        
        if(!isDead)
        {
            if (playerInAttackRange)
            {
                target = transform.position;
                transform.LookAt(_player.position);

                if (!alreadyAttacked)
                {
                    _animator.SetBool("isAttacking", playerInAttackRange);
                }
            }
            else
            {
                ChasingPlayer();
            }
        }
        else
        {
            target = transform.position;
        }
    }
    
    void AttackPlayer()
    {
        _playerHealth.TakeDamage(damage);
        alreadyAttacked = true;
        
        Invoke(nameof(ResetAttack), attackCooldown);
    }
    
    private void ChasingPlayer()
    {
        target = _player.position;
        _animator.SetBool("isAttacking", playerInAttackRange);
    }

    void ZombieDeath()
    {
        isDead = true;
    }

    void PlayerDeath()
    {
        isDead = true;
        _animator.SetTrigger("PlayerDead");
    }
    
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    private IEnumerator UpdateForTarget()
    {
        yield return new WaitForSeconds(0.15f);
        _navMeshAgent.SetDestination(target);
        StartCoroutine(UpdateForTarget());
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, attackDistance);
    }
}
