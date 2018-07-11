using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrusterController : MonoBehaviour {

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();	
	}
	
	void FixedUpdate () {

		Vector3 desiredLinearVelocity = Vector3.zero, desiredAngularVelocity = Vector3.zero;
		Vector3 topLinearAcceleration = Vector3.zero, topAngularAcceleration = Vector3.zero;

		foreach (var thruster in GetComponents<Thruster>()) {
			if (thruster.IsRotation) {
				desiredAngularVelocity += thruster.DesiredVelocity;
				topAngularAcceleration += thruster.ForceAxisNormalized * thruster.TopAcceleration;
			}
			else {
				desiredLinearVelocity += thruster.DesiredVelocity;
				topLinearAcceleration += thruster.ForceAxisNormalized * thruster.TopAcceleration;
			}
		}

		Debug.Log("");
		Debug.Log(transform.TransformDirection(desiredLinearVelocity));
		Debug.Log(rb.velocity);
		Vector3 linearAccel = acceleration(transform.TransformDirection(desiredLinearVelocity) - rb.velocity, topLinearAcceleration);
		Vector3 angularAccel = acceleration(transform.TransformDirection(desiredAngularVelocity) - rb.angularVelocity, topAngularAcceleration);

		rb.AddRelativeTorque(angularAccel, ForceMode.Acceleration);
		rb.AddRelativeForce(linearAccel, ForceMode.Acceleration);
		// transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + angularAccel);

		// Debug.Log(rigidbody.velocity);
		// Debug.Log(rigidbody.angularVelocity);
	}

	Vector3 acceleration (Vector3 deltaV, Vector3 topAcceleration) {
		Vector3 accel = deltaV / Time.deltaTime;

		for (int i = 0; i < 3; i++) {
			if (Mathf.Abs(accel[i]) > topAcceleration[i]) {
				accel[i] = topAcceleration[i] * Mathf.Sign(accel[i]);
			}
		}

		return accel;
	}
}
