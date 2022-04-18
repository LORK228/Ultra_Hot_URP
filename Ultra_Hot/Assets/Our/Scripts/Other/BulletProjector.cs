using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjector : MonoBehaviour {

	[SerializeField] private float distanceTolerance = 0.05f;
	private float origNearClipPlane;
	private float origFarClipPlane;
	private Projector projector;

	void Start() 
	{
		if(gameObject.name == "ProjectorWall(Clone)")
        {
			GameObject a = GameObject.Find("Projectors");
			a.GetComponent<Projectors>().projectors.Add(gameObject);
			transform.parent = a.transform;
		}
		projector = GetComponent<Projector>();
		origNearClipPlane = projector.nearClipPlane;
		origFarClipPlane = projector.farClipPlane;
		Late();
	}

	void Late()
	{
		Ray ray = new Ray(projector.transform.position + projector.transform.forward.normalized * origNearClipPlane, projector.transform.forward);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast (ray, out hit, origFarClipPlane - origNearClipPlane, ~projector.ignoreLayers)) 
		{
			float dist = hit.distance + origNearClipPlane;
			projector.nearClipPlane = Mathf.Max(dist - distanceTolerance, 0);
			projector.farClipPlane = dist + distanceTolerance;
		}
	}

}
