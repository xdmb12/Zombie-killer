using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float speed;
    public float damage;

    public GameObject hitVFX;
    public GameObject hitBlood;
    public Vector3 target;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed);
        
        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent<EnemyHealthSystem>(out EnemyHealthSystem enemyHealth))
            {
                enemyHealth.health -= damage;
            }
            
            GameObject particle = Instantiate(hitBlood, transform.position, Quaternion.identity);
            Destroy(particle, 1f);
        }
        else
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
