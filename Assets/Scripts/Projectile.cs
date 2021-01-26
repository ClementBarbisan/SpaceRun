using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : NetworkedBehaviour
{
    [FormerlySerializedAs("parent")] public ProjectileManager parentProjectileManager;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (!IsOwner)
            _rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
            _rb.AddForce(transform.forward * transform.localScale.magnitude, ForceMode.Acceleration);
    }

    IEnumerator ExploseTarget(GameObject target)
    {
        Material mat = target.GetComponent<Renderer>().material;
        Color currentColor = mat.color;
        float value = 0;
        while (value < 1f)
        {
            currentColor.a = 1f - value;
            mat.color = currentColor;
            value += Time.deltaTime;
            yield return null;
        }
        Destroy(target);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet") || other.CompareTag("Player"))
        {
            parentProjectileManager.GetProjectile(gameObject);
        }

        if (other.CompareTag("Target"))
        {
            StartCoroutine(ExploseTarget(other.gameObject));
        }
    }
}
