﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class TrackController : Singleton<TrackController> {

	[Tooltip("The total number of segments making up the overall line. Changing this value after the script is loaded has no effect")]
	public int NumberOfCurves;

	[Tooltip("Spawned at intervals along the line. Used by the player object to determine distance")]
	public Collider CenterGameobject;

	public LineRenderer CenterLine;

	[Tooltip("Define multiple line renderers here. Lines are distributed equally in a circle around the center, definition line")]
	public LineRenderer[] Lines;
	[Tooltip("Distance from each line to the center")]
	public float LineDistance = 5;
	[Tooltip("Resolution of each line")]
	public int DotsPerLine = 20;
	public int CenterObjectsPerLine = 5;

	List<Curve> curves;
	List<GameObject> curveObjects;

	Curve previous { get { return curves[curves.Count - 1]; } }

	Transform curveParent;

	float s;

	void Start () {
		Instance = this;

		curves = new List<Curve>();
		curveObjects = new List<GameObject>();

		curveParent = new GameObject("Curves").transform;
		curveParent.parent = transform;

		// Vector3 next = Vector3.forward * 10;
		curves.Add(new Curve(Vector3.back * 5, Vector3.back * 4, Vector3.back * 3, Vector3.back * 2));

		for (int i = 0; i < NumberOfCurves; i++)
			addNewCurve();
	}

	public Vector3 GetPositionAt (float s) {
		return getCurveAt(s).Position(s % 1);
	}

	public Vector3 GetVelocityAt (float s) {
		return getCurveAt(s).Velocity(s % 1);
	}

	Curve getCurveAt (float s) {
		int index = Mathf.FloorToInt(s);
		if (index > curves.Count / 2) addNewCurve();

		return curves[index];
	}

	Vector3 newPointOffset () {
		Vector2 xy = Random.insideUnitCircle * (60 + curves.Count / 2.0f);
		float z = Random.Range(30, 50);

		return new Vector3(xy.x, xy.y, z);
	}

	void addNewCurve () {
		Curve prev = previous;
		Curve next = new Curve(prev.p1, prev.p2, prev.p3, prev.p3 + newPointOffset());

		curves.Add(next);

		var parent = new GameObject("Curve " + curves.Count);
		parent.transform.parent = curveParent;
		curveObjects.Add(parent);

		var samples = previous.Samples(DotsPerLine);

		int centerCycleCounter = 0;
		int centerCycle = DotsPerLine / CenterObjectsPerLine;

		Vector3 centerDirection = Vector3.zero;

		for (int i = 0; i < samples.Length; i++) {

			Vector3 point = samples[i].Item1;

			centerDirection += point - (i > 0 ? samples[i-1].Item1 : point);

			if (centerCycleCounter++ >= centerCycle) {
				Instantiate(CenterGameobject, point, Quaternion.Euler(samples[i-1].Item1 - point)).transform.parent = parent.transform;
				centerDirection = Vector3.zero;
				centerCycleCounter = 0;
			}

			CenterLine.SetPosition(CenterLine.positionCount++, point);

			float angle = 0;
			foreach (var line in Lines) {
				Vector3 dir = Vector3.Slerp(Vector3.forward, samples[i].Item2.normalized, .25f);
				Vector3 dot = point + Quaternion.AngleAxis(angle, dir) * Vector3.right * LineDistance;
				line.SetPosition(line.positionCount++, dot);

				angle += 360f / Lines.Length;
			}

		}

	}

}
