using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyWeapon : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
    }
}
