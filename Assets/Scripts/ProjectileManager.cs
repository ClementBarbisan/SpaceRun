using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // private Rigidbody _rb;
    // public bool getProj = true;
    // private Transform _currentParent;
    [SerializeField] private GameObject prefabProj;
    private float _force;
    private Transform _tr;
    [SerializeField]
    private int maxPrj = 3;
    public List<GameObject> projList;
    // Start is called before the first frame update
    void Awake()
    {
        // tag = "weapon";
        // _rb = GetComponent<Rigidbody>();
        _tr = transform;
        projList = new List<GameObject>();
        // _currentParent = _tr.parent;
    }

    public void GetProjectile(GameObject proj)
    {
        Player.Instance.RemoveCrosshair(projList.IndexOf(proj));
        projList.Remove(proj);
        Destroy(proj);
        // transform.parent = _currentParent;
        // getProj = true;
        // tag = "weapon";
        // _rb.velocity = Vector3.zero;
        // _rb.angularVelocity = Vector3.zero;
        // _tr.position = _tr.parent.position;
        // _tr.rotation = _tr.parent.rotation;
    }

    void LaunchProjectile()
    {
        GameObject go = Instantiate(prefabProj, _tr.parent.position, _tr.parent.rotation);
        projList.Add(go);
        Player.Instance.AddCrosshair();
        go.GetComponent<Projectile>().parent = this;
        go.GetComponent<Rigidbody>().AddForce(transform.parent.forward * _force, ForceMode.VelocityChange);
        _force = 1f;
        // getProj = false;
        // tag = "projectile";
        // _rb.AddForce(transform.parent.forward * _force, ForceMode.VelocityChange);
        // _tr.parent = null;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("joystick button 14") && projList.Count < maxPrj)
            _force += Time.deltaTime * 75f;
        if (Input.GetKeyUp("joystick button 14") && projList.Count < maxPrj)
        {
            LaunchProjectile();
        }

        if (Input.GetKey("joystick button 4") && Player.Instance.indexProj < projList.Count)
        {
            projList[Player.Instance.indexProj].GetComponent<Rigidbody>().isKinematic = true;
            projList[Player.Instance.indexProj].transform.position +=
                (Player.Instance.transform.position - projList[Player.Instance.indexProj].transform.position).normalized *
                Time.deltaTime * 75f;
            // GetProjectile(projList[projList.Count - 1]);
        }
        else if (projList.Count > 0 && Player.Instance.indexProj < projList.Count)      
            projList[Player.Instance.indexProj].GetComponent<Rigidbody>().isKinematic = false;
    }

    public void ChangeIndex()
    {
        for (int i = 0; i < projList.Count; i++)
            projList[i].GetComponent<Rigidbody>().isKinematic = false;
    }
}
