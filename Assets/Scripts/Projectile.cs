using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    public bool getProj = true;
    private Transform currentParent;

    private float force;

    // Start is called before the first frame update
    void Awake()
    {
        tag = "weapon";
        rb = GetComponent<Rigidbody>();
        currentParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("joystick button 15") && getProj)
            force += Time.deltaTime * 10f;
        if (Input.GetKeyUp("joystick button 15") && getProj)
        {
            getProj = false;
            tag = "projectile";
            rb.AddForce(transform.parent.forward * force, ForceMode.VelocityChange);
            transform.parent = null;
            force = 1f;
        }

        if (Input.GetKeyDown("joystick button 5") && !getProj)
        {
            transform.parent = currentParent;
            getProj = true;
            tag = "weapon";
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
        }
    }
}
