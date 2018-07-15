using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPlane : MonoBehaviour {

	public static EvilPlane Instance;

	public Transform Target;
	public float Distance { get; private set; }

	Plane plane;

	void Start () {
		Instance = this;
	}
	
	void Update () {
		plane = new Plane(transform.forward, transform.position);
		Distance = plane.GetDistanceToPoint(Target.position);
	}

}
