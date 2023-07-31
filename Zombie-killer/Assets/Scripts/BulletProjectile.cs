using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float speed;

    public Transform hitVFX;
    public Transform hitVFX2;
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

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Enemy"))
        // {
        //     Instantiate(hitVFX, transform.position, Quaternion.identity);
        // }
        // else
        // {
        //     Instantiate(hitVFX2, transform.position, Quaternion.identity);
        // }
        Instantiate(hitVFX, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
