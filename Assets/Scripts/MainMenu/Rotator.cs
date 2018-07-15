using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	void Update () {
		Vector3 rotation = new Vector3(37, 37, 37) * Time.deltaTime;
		transform.rotation *= Quaternion.Euler(rotation);
	}

}
