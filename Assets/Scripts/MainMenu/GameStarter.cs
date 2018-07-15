using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {

	public float FadeInTime, FadeOutTime;

	Image image;

	void Start () {
		image = GetComponent<Image>();
		StartCoroutine(start());
	}

	void Update () {
		if (Input.anyKeyDown) {
			StartCoroutine(end());
		}
	}

	IEnumerator start () {
		yield return new WaitForEndOfFrame();
		image.CrossFadeAlpha(0, FadeInTime, true);
	}

	IEnumerator end () {
		image.CrossFadeAlpha(1, FadeOutTime, true);
		yield return new WaitForSecondsRealtime(FadeOutTime);
		SceneManager.LoadSceneAsync("Endless");
	}

}
