using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Aim : MonoBehaviour
{
    public Transform sight;
    public float _speed;
   // private float t;
   // private float dist;
    private void Start()
    {

    }
    void Update()
    {
        transform.position= Vector3.MoveTowards(transform.position, sight.position,_speed);
       // dist = Vector3.Distance(transform.position, sight.position);
       // print(dist);

        
           // transform.position = new Vector3(Mathf.Lerp(transform.position.x, sight.position.x, (t * (1 / _speed))*dist/100), Mathf.Lerp(transform.position.y, sight.position.y, (t * (1 / _speed)) * dist / 100), Mathf.Lerp(transform.position.z, sight.position.z, (t * (1 / _speed)) * dist / 100));

        
    }
}
