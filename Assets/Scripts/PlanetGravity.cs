﻿using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    private SphereCollider _collider;

    private Transform _tr;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _tr = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Vector3 otherPos = other.gameObject.transform.position;
            Vector3 direction = new Vector3(transform.position.x - otherPos.x,
                _tr.position.y - otherPos.y,
                _tr.position.z - otherPos.z).normalized;
            float value = 1f / (Mathf.Pow(Vector3.Distance(_tr.position, otherPos)
                                          / (_collider.radius * _tr.parent.localScale.x * 2.5f), 2)) * Time.deltaTime * 25f;
            other.attachedRigidbody.AddForce(new Vector3(direction.x * value, direction.y * value, direction.z * value), ForceMode.Acceleration);
        }
    }
}
