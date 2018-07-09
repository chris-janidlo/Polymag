using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackController : MonoBehaviour {

	public GameObject dot;
	
	public static TrackController Instance;

	public float Speed;
	public Vector3 Position { get; private set; }
	public Vector3 Velocity { get; private set; }

	List<Curve> curves;

	float s = 0;

	void Start () {
		Instance = this;
		curves = new List<Curve>();

		Vector3 next = Vector3.forward * 10;
		curves.Add(new Curve(Vector3.back, Vector3.zero, next, next + newPointOffset()));

		for (int i = 0; i < 100; i++)
			addNewCurve();
	}

	void Update () {
		Curve ridingCurve = curves[0];

		Position = ridingCurve.Position(s);
		Velocity = ridingCurve.Velocity(s);

		// FIXME: position should change at a constant velocity, but it doesn't
		s += Speed / Velocity.magnitude * Time.deltaTime;

		if (s >= 1) {
			s -= 1;
			addNewCurve();
			curves.RemoveAt(0);

			Curve tmp = curves[0];
		}

		Instantiate(dot, Position, Quaternion.identity);
	}

	Vector3 newPointOffset () {
		Vector2 xy = Random.insideUnitCircle * 30;
		float z = Random.Range(10, 20);

		return new Vector3(xy.x, xy.y, z);
	}

	void addNewCurve () {
		Curve prev = curves[curves.Count - 1];
		Curve next = new Curve(prev.p1, prev.p2, prev.p3, prev.p3 + newPointOffset());

		curves.Add(next);
	}

}
