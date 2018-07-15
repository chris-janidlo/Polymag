using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerMeter : MonoBehaviour {

	public float MaxDangerDistance, SafeDistance;

	Image meter;

	void Start () {
		meter = GetComponent<Image>();
	}
	
	void Update () {
		float dist = EvilPlane.Instance.Distance;
		meter.fillAmount = (dist - SafeDistance) / (MaxDangerDistance - SafeDistance);
	}

}
