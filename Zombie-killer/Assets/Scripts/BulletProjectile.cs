using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed;

    [Header("Bullet VFX")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject hitBlood;
    [HideInInspector] public Vector3 target;
    
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
