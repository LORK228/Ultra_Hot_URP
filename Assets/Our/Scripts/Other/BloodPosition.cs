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
        particle = gameObject.GetComponent<ParticleSystem>();
        StartCoroutine("blood");
    }
    public IEnumerator blood()
    {
        yield return new WaitForSeconds(0.01f);
        transform.parent = parentPart;
    }
    private void Update()
    {
        if (triggerPart == null && particle.isPlaying == false)
        {
            print("fasfas");
            particle.Play();

        }
    }
}
