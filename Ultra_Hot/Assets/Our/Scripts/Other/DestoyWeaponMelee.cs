using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyWeaponMelee : MonoBehaviour
{
    public bool isNeedFixed;
    private void Start()
    {
        if (isNeedFixed)
        {
            transform.Rotate(transform.rotation.x - 90, transform.rotation.y, transform.rotation.z);
        }
    }
}
