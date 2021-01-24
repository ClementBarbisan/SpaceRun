using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileManager parent;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.AddForce(transform.forward * transform.localScale.magnitude, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet") || other.CompareTag("Player"))
        {
            parent.GetProjectile(gameObject);
        }
    }
}
