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
}
