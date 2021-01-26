using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class PlayerPrefab : NetworkedBehaviour
{
    // Start is called before the first frame update
    public override void NetworkStart()
    {
        base.NetworkStart();
        if (!IsLocalPlayer)
            return;
        Player.Instance.id = OwnerClientId;
        transform.parent = GameObject.Find("PlayerStart").transform;
        transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
