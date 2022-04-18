using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class moving : MonoBehaviour
{
    [SerializeField] private int x;
    public void moveright()
    {
        transform.DOMoveX(transform.position.x-x, 1);
    }
    public void moveleft()
    {
        transform.DOMoveX(transform.position.x+x, 1);
    }
}
