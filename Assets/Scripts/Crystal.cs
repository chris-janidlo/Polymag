using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Collider))]
public class Crystal : MonoBehaviour {

	public float Speed;

	public float ValueAlongCurve;

	public Vector3 RotationAxis;
	public float RotationRate;

	public float SpawnBubbleTime, SpawnBubbleScale;
	public float DeathFlyTime;

	public event System.Action<float> Caught; // the float passed along is the value at the curve

	bool dead, doneSpawning;

	void Update () {
		if (dead) return;

		if (doneSpawning) {
			transform.Rotate(RotationAxis, RotationRate * Time.deltaTime);
		}

		var position = TrackController.Instance.GetPositionAt(ValueAlongCurve);
		var velocity = TrackController.Instance.GetVelocityAt(ValueAlongCurve);

		// FIXME: speed is smooth, but not constant across segments of different length
		ValueAlongCurve += Speed / velocity.magnitude;

		transform.position = position;
	}

	public void Initialize (float s) {
		StartCoroutine(birthSequence());
		ValueAlongCurve = s;
	}

	void OnTriggerEnter (Collider other) {
		StartCoroutine(deathSequence());
		Caught?.Invoke(ValueAlongCurve);
	}

	IEnumerator birthSequence () {
		Vector3 target = transform.localScale * SpawnBubbleScale, velocity = Vector3.zero, oldScale = transform.localScale;
		float timer = SpawnBubbleTime;

		transform.localScale /= 2;

		while (timer >= 0) {
			transform.localScale = Vector3.Lerp(transform.localScale, target, .25f);

			timer -= Time.deltaTime;
			yield return null;
		}
		doneSpawning = true;

		timer = SpawnBubbleTime;
		while (timer >= 0) {
			transform.localScale = Vector3.SmoothDamp(transform.localScale, oldScale, ref velocity, SpawnBubbleTime);

			timer -= Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator deathSequence () {
		GetComponent<Collider>().enabled = false;
		dead = true;

		transform.parent = GameObject.Find("Canvas").transform;
		
		Transform target = CrystalManager.Instance.DeathTarget;
		Vector3 velocity = Vector3.zero;

		while (Vector3.Distance(transform.position, target.position) > 1) {
			transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, DeathFlyTime);
			yield return null;
		}

		// TODO: increase fuel

		Destroy(gameObject);
	}
}
