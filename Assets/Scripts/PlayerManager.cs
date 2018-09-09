using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	public Transform CameraRotator;
	public Text CenterText;
	public Camera CameraT;
	public GameObject PlayerVisual;

	public float StartDelay;

	public bool Dead { get; private set; }

	bool quitting;

	void Start () {
		StartCoroutine(startRoutine());
	}

	void Update () {
		if (!quitting && Input.GetButton("Quit Game")) {
			quitting = true;
			StartCoroutine(quit());
		}
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

		CenterText.text = "Catch the Power Crystal";
		yield return new WaitForSeconds(StartDelay);

		CenterText.text = "3";
		yield return new WaitForSeconds(1);

		CenterText.text = "2";
		yield return new WaitForSeconds(1);

		CenterText.text = "1";
		yield return new WaitForSeconds(1);

		CenterText.text = "GO";
		SetControlsActive(true);
		CrystalManager.Instance.SpawnFirstCrystal();

		yield return new WaitForSeconds(1);
		CenterText.text = "";
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
		CenterText.text = "It got away...";

		yield return new WaitForSeconds(3.7f);
		StartCoroutine(quit()); // untested
	}

	IEnumerator quit () {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("main");
		yield return new WaitUntil(() => asyncLoad.isDone);
	}

}
