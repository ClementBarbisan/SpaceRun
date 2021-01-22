using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlanet : MonoBehaviour
{
    public float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(transform.position.z), Mathf.Sin(Time.time), Mathf.Sin(transform.position.x) * Mathf.Cos(transform.position.y));
    }
}
