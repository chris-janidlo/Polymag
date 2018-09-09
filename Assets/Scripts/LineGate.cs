using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LineGate : MonoBehaviour {

	event System.Action<Curve> collisionEvent;
	Curve associatedCurve;

	public void Initialize (Curve associatedCurve, System.Action<Curve> onCollision) {
		this.associatedCurve = associatedCurve;
		collisionEvent += onCollision;
	}

	void OnTriggerEnter (Collider collision) {
		if (collisionEvent != null && collision.gameObject.tag == "Player")
			collisionEvent(associatedCurve);
	}

}
