using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(ThrusterController))]
public class Thruster : MonoBehaviour {

	public string InputAxisName;
	
	public Vector3 ForceAxis;
	public bool IsRotation;

	public float TopAcceleration, TopSpeed;//, DecelTime = .1f;

	public Vector3 ForceAxisNormalized { get; private set; }
	public Vector3 DesiredVelocity { get; private set; }

	void Start () {
		OnValidate();
	}

	void Update () {
		checkForceAxis();

		float input = Input.GetAxis(InputAxisName);
		DesiredVelocity = ForceAxisNormalized * TopSpeed * input;

	}

	void OnValidate () {
		ForceAxisNormalized = ForceAxis.normalized;
#if UNITY_EDITOR
		if (EditorApplication.isPlaying) checkForceAxis();
#endif
	}

	[Conditional("UNITY_EDITOR")]
	void checkForceAxis () {
		if (ForceAxisNormalized.Equals(Vector3.zero)) {
			throw new Exception("'" + InputAxisName + "' thruster: ForceAxis " + ForceAxis.ToString() + " is too small to be used as a direction");
		}
	}

}
