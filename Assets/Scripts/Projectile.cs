﻿using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : NetworkedBehaviour
{
    [FormerlySerializedAs("parent")] public ProjectileManager parentProjectileManager;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        parentProjectileManager = FindObjectOfType<ProjectileManager>();
        _rb = GetComponent<Rigidbody>();
        parentProjectileManager.projList.Add(gameObject);
        if (!IsOwner)
            _rb.isKinematic = true;
        else
        {
            Player.Instance.AddCrosshair();
            _rb.AddForce(transform.forward * parentProjectileManager.force, ForceMode.VelocityChange);
            parentProjectileManager.force = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
            _rb.AddForce(transform.forward * transform.localScale.magnitude, ForceMode.Acceleration);
    }
    [ClientRPC]
    public void RestartPlayer()
    {
        Player.Instance.dead++;
        Player.Instance.StartPosition();
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
        if (other.CompareTag("Planet") || other.CompareTag("Shield"))
        {
            parentProjectileManager.GetProjectile(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<NetworkedObject>().IsLocalPlayer)
                parentProjectileManager.GetProjectile(gameObject);
            else
            {
                Player.Instance.kills++;
                InvokeClientRpcOnClient("RestartPlayer", other.GetComponent<PlayerPrefab>().OwnerClientId);
                Debug.Log("DIE");
            }
        }
        if (other.CompareTag("Target"))
        {
            parentProjectileManager.GetProjectile(gameObject);
            StartCoroutine(ExploseTarget(other.gameObject));
        }
    }
}
