using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crystal : MonoBehaviour {

	public float Speed;

	public float ValueAlongCurve;

	public event System.Action<float> Caught; // the float passed along is the value at the curve

	void Update () {
		var position = TrackController.Instance.GetPositionAt(ValueAlongCurve);
		var velocity = TrackController.Instance.GetVelocityAt(ValueAlongCurve);

		// FIXME: speed is smooth, but not constant across segments of different length
		ValueAlongCurve += Speed / velocity.magnitude;

		transform.position = position;
		transform.rotation = Quaternion.LookRotation(velocity.normalized);
	}

	public void Initialize (float s) {
		ValueAlongCurve = s;
	}

	void OnTriggerEnter (Collider other) {
		// play fun animation
		// increment score
		Caught?.Invoke(ValueAlongCurve);
		Destroy(gameObject);
	}
}
