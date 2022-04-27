using UnityEngine;
using System.Collections;

public class GlassBreaker : MonoBehaviour {
	Vector3 vel;
	BreakGlass g;
	void FixedUpdate () {
		vel = GetComponent<Rigidbody>().velocity;
	}
	
	
}
