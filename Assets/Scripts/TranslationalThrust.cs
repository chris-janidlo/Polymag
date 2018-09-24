using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(ThrusterController))]
public class TranslationalThrust : MonoBehaviour
{
	public string InputAxisName;
	
	public Vector3 ForceAxis;
	public float Thrust;

	public bool Activated { get; private set; }

	Vector3 forceAxisNormalized;
	float input;

	Rigidbody rb;

	void Start ()
	{
		OnValidate();
		rb = GetComponent<Rigidbody>();
	}

	void Update ()
	{
		checkForceAxis();

		input = Input.GetAxis(InputAxisName);
	}

	void FixedUpdate ()
	{
		Activated = input != 0;

		rb.AddRelativeForce(forceAxisNormalized * Thrust * input);
	}

	void OnValidate ()
	{
		forceAxisNormalized = ForceAxis.normalized;
#if UNITY_EDITOR
		if (EditorApplication.isPlaying) checkForceAxis();
#endif
	}

	[Conditional("UNITY_EDITOR")]
	void checkForceAxis ()
	{
		if (forceAxisNormalized.Equals(Vector3.zero))
		{
			throw new Exception("'" + InputAxisName + "' thruster: ForceAxis " + ForceAxis.ToString() + " is too small to be used as a direction");
		}
	}

}
