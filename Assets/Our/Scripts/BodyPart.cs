using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [HideInInspector]
    public bool go;
    public float speed;
    private float currTime;
   
    private void Update()
    {
        if (go)
        {
            GetComponent<SkinnedMeshRenderer>().material.SetFloat("Vector1_ef3b0207d48046f6933d61a61367308a", currTime* speed);
            currTime += Time.deltaTime;
        }
    }
}
