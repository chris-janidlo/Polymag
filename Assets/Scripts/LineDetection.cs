using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDetection : MonoBehaviour {

	public float MaxDistanceFromCenter;
	public float OffroadDrag = 1;
	
	bool offroad;

	int layerMask = 1 << 8;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (!Physics.CheckSphere(transform.position, MaxDistanceFromCenter, layerMask)) {
			if (!offroad) {
				rb.drag = OffroadDrag;
				offroad = true;
			}
		}
		else if (offroad) {
			rb.drag = 0;
			offroad = false;
		}
	}

}
