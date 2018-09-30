using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

[RequireComponent(typeof(Collider))]
public class Crystal : MonoBehaviour
{
	public AnimationCurve SpeedByPlayerDistance;

	[Tooltip("Distance from crystal within which player is safe from dying")]
	public float SafeDistance;

	public float ValueAlongCurve;

	public Vector3 RotationAxis;
	public float RotationRate;

	public float SpawnBubbleTime, SpawnBubbleScale;
	public float DeathFlyTime;

	public event System.Action<float> Caught; // the float passed along is the value at the curve

	bool dead, doneSpawning;

	void Update ()
	{
		if (dead) return;

		if (doneSpawning)
		{
			transform.Rotate(RotationAxis, RotationRate * Time.deltaTime);
		}

		var position = Track.Instance.GetPositionAt(ValueAlongCurve);
		var velocity = Track.Instance.GetVelocityAt(ValueAlongCurve);

		float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
		float speed = SpeedByPlayerDistance.Evaluate(distance);

		transform.position = position;

		// FIXME: speed is smooth, but not constant across segments of different length
		ValueAlongCurve += speed / velocity.magnitude;

		FuelTank.Instance.DisableDying = distance <= SafeDistance;
	}

	public void Initialize (float s)
	{
		StartCoroutine(birthSequence());
		ValueAlongCurve = s;
	}

	void OnTriggerEnter (Collider other)
	{
		StartCoroutine(deathSequence());
		Caught?.Invoke(ValueAlongCurve);
	}

	IEnumerator birthSequence ()
	{
		Vector3 target = transform.localScale * SpawnBubbleScale, velocity = Vector3.zero, oldScale = transform.localScale;
		float timer = SpawnBubbleTime;

		transform.localScale /= 2;

		while (timer >= 0)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, target, .25f);

			timer -= Time.deltaTime;
			yield return null;
		}
		doneSpawning = true;

		timer = SpawnBubbleTime;
		while (timer >= 0)
		{
			transform.localScale = Vector3.SmoothDamp(transform.localScale, oldScale, ref velocity, SpawnBubbleTime);

			timer -= Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator deathSequence ()
	{
		FuelTank.Instance.DisableDying = true;

		GetComponent<Collider>().enabled = false;
		dead = true;

		transform.parent = GameObject.Find("Canvas").transform;
		gameObject.layer = 5; // UI layer. TODO: can we get the crystal on top of the UI when it's dead?
		
		RectTransform target = CrystalManager.Instance.DeathTarget;
		Vector3 velocity = Vector3.zero;
		
		var mc = CameraCache.Main;

		while (!RectTransformUtility.RectangleContainsScreenPoint(target, mc.WorldToScreenPoint(transform.position), mc))
		{
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, target.localPosition, ref velocity, DeathFlyTime);
			yield return null;
		}

		Destroy(gameObject);

		FuelTank.Instance.PickUpCrystal();
		FuelTank.Instance.DisableDying = false;
	}
}
