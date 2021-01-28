using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class Shield : NetworkedBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            transform.parent = GameObject.FindWithTag("RightController").transform;
            transform.position += transform.forward * transform.parent.localScale.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
