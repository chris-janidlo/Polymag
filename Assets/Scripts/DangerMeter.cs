using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerMeter : MonoBehaviour {

	public float MaxDangerDistance, SafeDistance;

	public Image Meter;
	
	void Update () {
		float dist = EvilPlane.Instance.Distance;
		Meter.fillAmount = (dist - SafeDistance) / (MaxDangerDistance - SafeDistance);
		
		if (PlayerManager.Instance.Dead) {
			gameObject.SetActive(false);
		}
	}

}
