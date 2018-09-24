using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour {

	[Tooltip("Graph where x = player distance from track center as percentage of track width, and y = the associated drag.")]
	public AnimationCurve DragByDistanceToCurve;

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		float percent = Track.Instance.DistanceFromCurve(transform.position) / Track.Instance.LineDistance;
		Debug.Log(percent);
		rb.drag = DragByDistanceToCurve.Evaluate(percent);
	}

}
