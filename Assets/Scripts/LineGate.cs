using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LineGate : MonoBehaviour {

	event System.Action<CurveSegment> collisionEvent;
	CurveSegment associatedCurve;

	public void Initialize (CurveSegment associatedCurve, System.Action<CurveSegment> onCollision) {
		this.associatedCurve = associatedCurve;
		collisionEvent += onCollision;
	}

	void OnTriggerEnter (Collider collision) {
		if (collisionEvent != null && collision.gameObject.tag == "Player")
			collisionEvent(associatedCurve);
	}

}
