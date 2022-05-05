using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPosition : MonoBehaviour
{
    public Transform parentPart;
    public GameObject triggerPart;
    private ParticleSystem particle;

    private void Start()
    {
        particle = gameObject.GetComponent<ParticleSystem>();;
    }
    
    private void Update()
    {
        if (triggerPart == null && particle.isPlaying == false)
        {
            particle.Play();

        }
    }
}
