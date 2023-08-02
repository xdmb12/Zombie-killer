using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    public float damage;
    private PlayerHealthSystem _playerHealth;
    private Vector3 target;
    private bool playerInAttackRange;
    public float attackDistance;
    public LayerMask whatIsPlayer;
    private bool alreadyAttacked;
    public float attackCooldown;
    private Animator _animator;
    private EnemyHealthSystem _enemyHealth;
    public bool isDead;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _player = player.transform;
        _playerHealth = player.GetComponent<PlayerHealthSystem>();
        _enemyHealth = GetComponent<EnemyHealthSystem>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyHealth.onDeath.AddListener(ZombieDeath);
        _playerHealth.playerDeathEvent.AddListener(PlayerDeath);
        
        StartCoroutine(UpdateForTarget());
    }

    // Update is called once per frame
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

    void ZombieDeath()
    {
        isDead = true;
    }

    void PlayerDeath()
    {
        isDead = true;
        _animator.SetTrigger("PlayerDead");
    }
    
    private void ChasingPlayer()
    {
        target = _player.position;
        _animator.SetBool("isAttacking", playerInAttackRange);
    }
    
    void AttackPlayer()
    {
        Debug.Log("Attack");
        _playerHealth.TakeDamage(damage);
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), attackCooldown);
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
