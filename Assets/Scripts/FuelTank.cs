using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class FuelTank : Singleton<FuelTank>
{
	public float Fuel, MaxFuel, LossPerSecond, GainPerCrystal;

	public int TankDivisions;

	public int DangerFlashes;
	public float DangerFlashRate;

	public Image AmountImage, BackgroundImage;
	public float AmountLerp;

	public Color BackgroundNeutral, BackgroundDanger;

	public float PickUpScale, PickUpTime;

	public float PercentRemaining { get { return Fuel / MaxFuel; } }

	[SerializeField]
	private bool _disableDying;
	public bool DisableDying
	{
		get { return _disableDying; }
		set
		{
			if (value)
				resetDangerTimer();
			_disableDying = value;
		}
	}

	Vector3 origScale;

	bool inDanger;
	float dangerTimer, dangerFlashTimer;

	void Start ()
	{
		SingletonSetInstance(this, true);

		Fuel = MaxFuel;
		origScale = transform.localScale;
		BackgroundNeutral = BackgroundImage.color;
	}
	
	void Update ()
	{
		if (PlayerManager.Instance.Started)
		{
			Fuel -= LossPerSecond * Time.deltaTime;
			Fuel = Mathf.Clamp(Fuel, 0, MaxFuel);
		}
		
		AmountImage.fillAmount = imageFillAmount();

		if (Fuel == 0 && !inDanger)
		{
			inDanger = true;
			resetDangerTimer();
		}
		if (Fuel != 0 && inDanger)
		{
			inDanger = false;
		}

		if (inDanger)
		{
			if (BackgroundImage.color == BackgroundNeutral)
			{
				dangerFlashTimer = DangerFlashRate;
				BackgroundImage.color = BackgroundDanger;
			}
			else
			{
				BackgroundImage.color = Color.Lerp(BackgroundNeutral, BackgroundDanger, dangerFlashTimer / DangerFlashRate);
				dangerFlashTimer -= Time.deltaTime;
			}

			dangerTimer -= Time.deltaTime;
			if (!DisableDying && dangerTimer <= 0)
			{
				PlayerManager.Instance.GameOver();
			}
		}
		else
		{
			BackgroundImage.color = BackgroundNeutral;
		}
	}

	public void PickUpCrystal ()
	{
		inDanger = false;
		Fuel += GainPerCrystal;
		StartCoroutine(pickUpAnimation());
	}

	float imageFillAmount ()
	{
		float amountPerTank = MaxFuel / TankDivisions;
		float roundedFuel = Mathf.Ceil(Fuel / amountPerTank) * amountPerTank;
		return Mathf.Lerp(AmountImage.fillAmount, roundedFuel / MaxFuel, AmountLerp);
	}

	void resetDangerTimer ()
	{
		dangerTimer = DangerFlashes * DangerFlashRate;
	}

	IEnumerator pickUpAnimation ()
	{
		Vector3 velocity = Vector3.zero;

		transform.localScale = origScale * PickUpScale;

		float timer = PickUpTime;
		while (timer >= 0)
		{
			transform.localScale = Vector3.SmoothDamp(transform.localScale, origScale, ref velocity, PickUpTime);

			timer -= Time.deltaTime;
			yield return null;
		}
	}
}
