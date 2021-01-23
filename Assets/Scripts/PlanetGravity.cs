using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    private SphereCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("projectile"))
        {
            Vector3 direction = new Vector3(transform.position.x - other.gameObject.transform.position.x,
                transform.position.y - other.gameObject.transform.position.y,
                transform.position.z - other.gameObject.transform.position.z).normalized;
            float value = 1f / (Mathf.Pow(Vector3.Distance(transform.position, other.gameObject.transform.position)
                                          / (_collider.radius * 20f), 2)) * Time.deltaTime * 25f;
            other.attachedRigidbody.AddForce(new Vector3(direction.x * value, direction.y * value, direction.z * value), ForceMode.Acceleration);
        }
    }
}
