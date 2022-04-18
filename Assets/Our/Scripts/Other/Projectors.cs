using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectors : MonoBehaviour
{
    [SerializeField] private int maxProjectors;
    [HideInInspector] public List<GameObject> projectors;

    private void Awake()
    {
        projectors = new List<GameObject>();
    }
    void Update()
    {
        if (projectors.Count > maxProjectors)
        {
            Destroy(projectors[0].gameObject);
            projectors.RemoveAt(0);
        }
    }
}
