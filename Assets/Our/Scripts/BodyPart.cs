using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [HideInInspector]
    public bool go;
    public float speed;
    private float currTime;
    private Transform chast;
    private SkinnedMeshRenderer mesh;
    private Rigidbody rigid;
    private health health;
    private Collider collider;
    private void Start()
    {
//      mesh = GetComponent<SkinnedMeshRenderer>();
        chast = GetComponent<Destroyer>().pos;
        // rigid = GetComponent<Destroyer>().pos.gameObject.GetComponent<Rigidbody>();
        // collider = GetComponent<Destroyer>().pos.gameObject.GetComponent<Collider>();
        // health = GetComponentInParent<health>();
    }
    private void Update()
    {
        if (go)
        {
            GetComponent<SkinnedMeshRenderer>().material.SetFloat("Vector1_ef3b0207d48046f6933d61a61367308a", currTime * speed);
            currTime += Time.deltaTime;
        }
        transform.position = chast.position;
        transform.rotation = chast.rotation;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 9)
    //    {
    //        if (rigid.velocity.magnitude > 3 && name != "mixamorigs:Hips" && name != "mixamorigs:Spine")
    //        {
    //            print(collider.name);
    //            print(transform);
    //            print(rigid);
    //            print(0);
    //            print(health);
    //            health.death(transform, 0, rigid);
    //        }
    //    }
    //}
}
