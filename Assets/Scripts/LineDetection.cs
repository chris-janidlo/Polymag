using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDetection : MonoBehaviour {

	public float OffroadDrag = 1;
	
	float tolerance = 0.01f;
	bool offroad;

	int layerMask = 1 << 8;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (!Physics.CheckSphere(transform.position, tolerance, layerMask, QueryTriggerInteraction.Collide)) {
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
