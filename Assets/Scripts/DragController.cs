using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour {

	[Tooltip("Graph where x = shortest distance between the player and the track center, and y = the associated drag.")]
	public AnimationCurve DragByDistanceToCurve;

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		float dist = Track.Instance.DistanceFromCurve(transform.position);
		rb.drag = DragByDistanceToCurve.Evaluate(dist);
	}

}
