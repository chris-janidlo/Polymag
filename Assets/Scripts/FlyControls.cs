using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyControls : MonoBehaviour {

	public float  PitchForce, // positive = pitch up (aka not typical flight controls)
				  YawForce,
				  VerticalThrustForce, // up/down
				  LateralThrustForce, // right/left
				  HorizontalThrustForce; // forward/backward

	// inputs
	float i_pitch, i_yaw, i_verticalThrust, i_lateralThrust, i_horizontalThrust;
	new Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {
		// TODO: gamepad assignment
		float pitch = Input.GetAxis("Pitch"),
			  yaw = Input.GetAxis("Yaw"),
			  vert = Input.GetAxis("Vertical Thrust"),
			  lat = Input.GetAxis("Lateral Thrust"),
			  hor = Input.GetAxis("Horizontal Thrust");

		if (pitch != 0) {
			i_pitch = pitch;
		}

		if  (yaw != 0) {
			i_yaw = yaw;
		}

		if (vert != 0) {
			i_verticalThrust = vert;
		}

		if (lat != 0) {
			i_lateralThrust = lat;
		}

		if (hor != 0) {
			i_horizontalThrust = hor;
		}
	}

	void FixedUpdate () {
		var translationalThrusts = new Vector3(
			i_lateralThrust * LateralThrustForce,
			i_verticalThrust * VerticalThrustForce,
			i_horizontalThrust * HorizontalThrustForce
		);
		rigidbody.AddRelativeForce(translationalThrusts);

		var rotationalThrusts = new Vector3(
			i_pitch * PitchForce,
			i_yaw * YawForce
		);
		rigidbody.AddRelativeTorque(rotationalThrusts);
	}

}
