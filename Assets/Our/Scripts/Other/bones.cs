using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bones : MonoBehaviour
{
    public GameObject Chast;
    public Transform[] Chast2;
    private Rigidbody rigid;
    private health health;
    private Collider collider;
    private void Start()
    {
        rigid=GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        health = GetComponentInParent<health>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (rigid.velocity.magnitude > 3 && name != "mixamorigs:Hips" && name != "mixamorigs:Spine")
            {
                print(collider.name);
                print(transform);
                print(rigid);
                print(0);
                print(health);
                health.death(transform, 0, collider, rigid);
            }
        }
    }
}
