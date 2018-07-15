using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance;

	public Transform CameraRotator;
	public Text CenterText;
	public Camera CameraT;
	public GameObject PlayerVisual;

	public float RotationTime = 1;
	public float RotateDelay;
	public float EvilDelay;

	public bool Dead { get; private set; }

	void Start () {
		Instance = this;
		StartCoroutine(startRoutine());
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Evil") {
			StartCoroutine(endRoutine());
		}
	}

	public void SetControlsActive (bool value) {
		GetComponent<ThrusterController>().enabled = value;
		GetComponent<RotationalThrust>().enabled = value;
		foreach (var thruster in GetComponents<TranslationalThrust>()) {
			thruster.enabled = value;
		}
	}

	IEnumerator startRoutine () {
		yield return null;
		SetControlsActive(false);
		TrackController.Instance.RideEnabled = false;

		yield return new WaitForSeconds(RotateDelay);

		float angle = 0;
		while (angle < 360) {
			CameraRotator.rotation = Quaternion.AngleAxis(angle, Vector3.up);
			angle += Time.deltaTime * (360 / RotationTime);
			yield return null;
		}
		CameraRotator.rotation = Quaternion.identity;

		yield return new WaitForSeconds(1);

		CenterText.text = "3";
		yield return new WaitForSeconds(1);

		CenterText.text = "2";
		yield return new WaitForSeconds(1);

		CenterText.text = "1";
		yield return new WaitForSeconds(1);

		CenterText.text = "go";
		SetControlsActive(true);
		yield return new WaitForSeconds(1);
		CenterText.text = "";

		yield return new WaitForSeconds(EvilDelay);
		TrackController.Instance.RideEnabled = true;
	}

	IEnumerator endRoutine () {
		Dead = true;
		SetControlsActive(false);

		// look at the stars
		transform.rotation = Quaternion.identity;
		CameraRotator.rotation = Quaternion.identity;

		// ui and player
		CameraT.cullingMask = (1 << 5) + (1 << 9);

		// play player destruction effect
		Destroy(PlayerVisual);

		yield return new WaitForSeconds(1);
		CenterText.text = "game over";

		yield return new WaitForSeconds(3.7f);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("main");
		yield return new WaitUntil(() => asyncLoad.isDone);
	}

}
