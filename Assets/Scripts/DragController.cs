using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
	[Tooltip("Graph where x = player distance from track center as percentage of track width, and y = the associated drag.")]
	public AnimationCurve DragByDistanceToCurve;

	[Tooltip("Using an iterative method, this is the number of points we test on the closest curve to find the smallest value.")]
	public int CurveDistanceSamples;

	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update ()
	{
		float percent = Track.Instance.DistanceFromCurve(transform.position, CurveDistanceSamples);
		rb.drag = DragByDistanceToCurve.Evaluate(percent);
	}
}
