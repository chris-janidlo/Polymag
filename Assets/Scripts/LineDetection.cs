using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDetection : MonoBehaviour {

	[Tooltip("Graph where x = shortest distance between the player and the track center, and y = the associated drag.")]
	public AnimationCurve DragByDistanceToCurve;

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		float dist = TrackController.Instance.DistanceFromCurve(transform.position);
		rb.drag = DragByDistanceToCurve.Evaluate(dist);
	}

}
