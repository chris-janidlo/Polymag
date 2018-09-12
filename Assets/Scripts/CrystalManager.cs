using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class CrystalManager : Singleton<CrystalManager> {

	public Crystal CrystalPrefab;

	public float FirstSpawnDistance;

	public float NewSpawnDelay;
	public Vector2 NewSpawnDistanceRange;

	[Tooltip("On being caught, crystals will travel to this target.")]
	public RectTransform DeathTarget;

	Crystal currentCrystal;

	void Start () {
		Instance = this;
		SingletonAllowReset();
	}

	public void SpawnFirstCrystal () {
		if (currentCrystal != null) throw new System.Exception("Should not be calling this twice");

		spawnCrystal(FirstSpawnDistance);
	}

	IEnumerator spawnNewCrystal (float oldDistance) {
		yield return new WaitForSeconds(NewSpawnDelay);

		float newDistance = oldDistance + Random.Range(NewSpawnDistanceRange.x, NewSpawnDistanceRange.y);
		spawnCrystal(newDistance);
	}

	void spawnCrystal (float distance) {
		var position = TrackController.Instance.GetPositionAt(distance);

		currentCrystal = Instantiate(CrystalPrefab, position, Quaternion.identity);
		currentCrystal.Initialize(distance);

		currentCrystal.Caught += OnCurrentCrystalCaught;
	}

	void OnCurrentCrystalCaught (float distance) {
		currentCrystal.Caught -= OnCurrentCrystalCaught;
		StartCoroutine(spawnNewCrystal(distance));
	}
}
