using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleForAI : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 0)
            if(gameObject.GetComponentInParent<AI>().Chase == true)  
                if(other.gameObject.GetComponent<ThrowAwayWeaponAndroid>().enabled == true)  
                    if (gameObject.GetComponentInParent<AI>().enabled == true)
                      if (other.isTrigger == false)
                        other.gameObject.GetComponent<ThrowAwayWeaponAndroid>().enabled = false;
                    
                    
    }
}
