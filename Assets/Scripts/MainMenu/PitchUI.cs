using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PitchUI : MonoBehaviour
{
	void OnEnable ()
	{
		GetComponent<Toggle>().isOn = RotationalThrust.PitchInverted;
	}

	public void SetPitchInversion (bool value)
	{
		RotationalThrust.PitchInverted = value;
	}
}
