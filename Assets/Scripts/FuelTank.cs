using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class FuelTank : Singleton<FuelTank> {

	public float Fuel, MaxFuel, LossPerSecond, GainPerCrystal;

	public Image AmountImage, BackgroundImage;
	public float AmountLerp;

	public float PickUpScale, PickUpTime;

	Vector3 origScale;

	void Start () {
		Instance = this;
		SingletonAllowReset();

		Fuel = MaxFuel;
		origScale = transform.localScale;
	}
	
	void Update () {
		if (PlayerManager.Instance.Started) {
			Fuel -= LossPerSecond * Time.deltaTime;
			Fuel = Mathf.Clamp(Fuel, 0, MaxFuel);
		}
		
		AmountImage.fillAmount = Mathf.Lerp(AmountImage.fillAmount, Fuel / MaxFuel, AmountLerp);
	}

	public void PickUpCrystal () {
		Fuel += GainPerCrystal;
		StartCoroutine(pickUpAnimation());
	}

	IEnumerator pickUpAnimation () {
		Vector3 velocity = Vector3.zero;

		transform.localScale = origScale * PickUpScale;

		float timer = PickUpTime;
		while (timer >= 0) {
			transform.localScale = Vector3.SmoothDamp(transform.localScale, origScale, ref velocity, PickUpTime);

			timer -= Time.deltaTime;
			yield return null;
		}
	}
}
