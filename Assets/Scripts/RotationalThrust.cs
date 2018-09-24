using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalThrust : MonoBehaviour
{
	public string PitchInputAxis;
	public string YawInputAxis;
	public float TurnSpeed;

	Vector2 input;
	
	void Update ()
	{
		input = new Vector2(
			Input.GetAxis(PitchInputAxis),
			Input.GetAxis(YawInputAxis)
		);
	}

	void FixedUpdate ()
	{
		transform.localRotation *= Quaternion.AngleAxis(input.y * TurnSpeed, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(input.x * TurnSpeed, Vector3.right);
	}
}
