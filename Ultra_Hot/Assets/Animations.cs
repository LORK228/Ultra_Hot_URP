using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    Animator mob;
    IEnumerator Start()
    {
        mob = GetComponent<Animator>();
        yield return new WaitForSeconds(3);
        mob.SetTrigger("Wok");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
