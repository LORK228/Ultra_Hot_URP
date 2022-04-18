using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            var skiMesh = GetComponent<SkinnedMeshRenderer>();
            skiMesh.BakeMesh(GetComponent<MeshFilter>().mesh);
            Destroy(skiMesh);
            GetComponent<Destroyer>().DestroyMesh();
        }
    }
}
