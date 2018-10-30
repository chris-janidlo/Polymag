using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using crass;

public class PlayerManager : Singleton<PlayerManager> 
{
	public static bool SkipIntro = false;

	public Transform CameraRotator;
	public Text CenterText;
	public Camera CameraT;
	public GameObject PlayerVisual;

	public float StartDelay;

	public ParticleSystem DeathEffect;

	public PausedWhenActive PauseMenu;

	public bool Started { get; private set; }
	public bool Dead { get; private set; }

	void Start () 
	{
		SingletonSetInstance(this, true);		
		
		StartCoroutine(startRoutine());
	}

	void Update () 
	{
		if (Input.GetButtonDown("Cancel")) 
		{
			PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeSelf);
		}
	}
	
	void OnTriggerEnter (Collider other) 
	{
		if (other.tag == "Evil") 
		{
			StartCoroutine(endRoutine());
		}
	}

	public void Quit ()
	{
		SceneManager.LoadScene("Main");
	}

	public void SetControlsActive (bool value) 
	{
		GetComponent<ThrusterController>().enabled = value;
		GetComponent<RotationalThrust>().enabled = value;
		foreach (var thruster in GetComponents<TranslationalThrust>()) 
		{
			thruster.enabled = value;
		}
	}

	public void GameOver () 
	{
		if (Dead) return;
		Dead = true;
		StartCoroutine(endRoutine());
	}

	IEnumerator startRoutine () 
	{
		yield return null;

		if (!SkipIntro) 
		{
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
		}

		Started = true;

		CrystalManager.Instance.SpawnFirstCrystal();

		yield return new WaitForSeconds(1);
		CenterText.text = "";
	}

	IEnumerator endRoutine () 
	{
		SetControlsActive(false);

		CameraCache.Main.transform.parent = null;
		
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		Destroy(PlayerVisual);
		Instantiate(DeathEffect, transform.position, Quaternion.identity);

		yield return new WaitForSeconds(1);
		CenterText.text = "You're Dead!";

		yield return new WaitForSeconds(3.7f);
		SceneManager.LoadScene("Endless");
	}
}
