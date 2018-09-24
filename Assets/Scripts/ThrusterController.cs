using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrusterController : MonoBehaviour
{
	public float MaxSpeed;
	public float DecelTime;

	bool decelerating;
	float decelTime;
	Vector3 speedAtDecel;

	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();	
	}
	
	void FixedUpdate ()
	{
		bool shouldDecelerate = true;
		foreach (TranslationalThrust thruster in GetComponents<TranslationalThrust>())
		{
			if (thruster.Activated)
			{
				shouldDecelerate = false;
				break;
			}
		}

		if (shouldDecelerate)
		{
			if (!decelerating)
			{
				decelerating = true;

				decelTime = 0;
				speedAtDecel = rb.velocity;
			}
			rb.velocity = Vector3.Lerp(speedAtDecel, Vector3.zero, decelTime);
			decelTime += Time.deltaTime / DecelTime;
		}
		else
		{
			decelerating = false;
		}

		if (rb.velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			rb.velocity = rb.velocity.normalized * MaxSpeed;
		}
	}
}
