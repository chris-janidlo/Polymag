using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class Thruster : MonoBehaviour {

	public string InputAxisName;
	
	public Vector3 ForceAxis;
	public bool IsRotation;

	public float Acceleration, TopSpeed, DecelTime = .1f;

	float speed;
	float decelTimer, speedAtDecelTime;
	bool accel;

	Vector3 forceAxisNormalized;

	new Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {
		checkForceAxis();

		float input = Input.GetAxis(InputAxisName);

		if (input > 0) {
			accel = true;

			speed += Acceleration * input * Time.deltaTime;
			speed = Mathf.Min(speed, TopSpeed);
		}
		else if (input < 0) {
			accel = true;

			speed -= Acceleration * input * Time.deltaTime;
			speed = Mathf.Max(speed, -TopSpeed);
		}
		else {
			if (accel) {
				accel = false;
				decelTimer = 0;
				speedAtDecelTime = speed;
			}

			decelTimer += Time.deltaTime;
			speed = Mathf.Lerp(speedAtDecelTime, 0, decelTimer / DecelTime);
		}

		if (IsRotation) {
			rigidbody.velocity = replaceNonZero(rigidbody.velocity, forceAxisNormalized * speed);
		}
		else {
			rigidbody.angularVelocity = replaceNonZero(rigidbody.angularVelocity, forceAxisNormalized * speed);
		}
	}

	void OnValidate () {
		forceAxisNormalized = ForceAxis.normalized;
		if (EditorApplication.isPlaying) checkForceAxis();
	}

	Vector3 replaceNonZero (Vector3 original, Vector3 replacement) {
		return new Vector3(
			replacement.x == 0 ? original.x : replacement.x,
			replacement.y == 0 ? original.y : replacement.y,
			replacement.z == 0 ? original.z : replacement.z
		);
	}

	[Conditional("UNITY_EDITOR")]
	void checkForceAxis () {
		if (forceAxisNormalized.Equals(Vector3.zero)) {
			throw new Exception("ForceAxis " + ForceAxis.ToString() + " is too small to be used as a direction");
		}
	}

}
