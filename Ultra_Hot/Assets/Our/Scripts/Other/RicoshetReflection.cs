using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicoshetReflection : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("b_dea");
    }
    IEnumerator b_dea()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
