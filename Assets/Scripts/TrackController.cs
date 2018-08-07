using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackController : MonoBehaviour {

	[Tooltip("The total number of segments making up the overall line. Changing this value after the script is loaded has no effect")]
	public int NumberOfCurves;

	public GameObject Rider;
	public bool RideEnabled;

	public float Speed;
	public float SpeedIncrease;
	public float MaxSpeed;

	[Tooltip("Spawned at intervals along the line. Used by the player object to determine distance")]
	public Collider CenterGameobject;

	public LineRenderer CenterLine;

	[Tooltip("Define multiple line renderers here. Lines are distributed equally in a circle around the center, definition line")]
	public LineRenderer[] Lines;
	[Tooltip("Distance from each line to the center")]
	public float LineDistance = 5;
	[Tooltip("Resolution of each line")]
	public int DotsPerLine = 20;
	public int CenterObjectsPerLine = 5;
	
	public static TrackController Instance;

	public Vector3 Position { get; private set; }
	public Vector3 Velocity { get; private set; }

	List<Curve> curves;
	List<GameObject> curveObjects;
	int curveCount = 0;

	Curve previous { get { return curves[curves.Count - 1]; } }

	Transform curveParent;

	float s = 0;

	void Start () {
		Instance = this;

		curves = new List<Curve>();
		curveObjects = new List<GameObject>();

		curveParent = new GameObject("Curves").transform;
		curveParent.parent = transform;

		// Vector3 next = Vector3.forward * 10;
		curves.Add(new Curve(Vector3.back * 5, Vector3.back * 4, Vector3.back * 3, Vector3.back * 2, 0));

		for (int i = 0; i < NumberOfCurves; i++)
			addNewCurve();
	}

	void Update () {
		if (!RideEnabled) return;

		Curve ridingCurve = curves[0];

		Position = ridingCurve.Position(s);

		s += Time.deltaTime;

		if (s >= 1) {
			s -= 1;

			addNewCurve();
			
			curves.RemoveAt(0);
			Destroy(curveObjects[0], 10);
			curveObjects.RemoveAt(0);

			// FIXME: remove line renderer parts for old curve

			Curve tmp = curves[0];
		}

		Speed = Mathf.Min(Speed + SpeedIncrease * Time.deltaTime, MaxSpeed);

		Rider.transform.position = Position;
		var look = (curves[0].Position(s) - Position).normalized;
		if (look.z < 0) {
			;
		}
		Rider.transform.rotation = Quaternion.LookRotation(look);
	}

	Vector3 newPointOffset () {
		Vector2 xy = Random.insideUnitCircle * (60 + curveCount / 2.0f);
		float z = Random.Range(30, 50);

		return new Vector3(xy.x, xy.y, z);
	}

	void addNewCurve () {
		Curve prev = previous;
		Curve next = new Curve(prev.p1, prev.p2, prev.p3, prev.p3 + newPointOffset(), 0);

		curves.Add(next);

		var parent = new GameObject("Curve " + curveCount++);
		parent.transform.parent = curveParent;
		curveObjects.Add(parent);

		var samples = previous.Samples(DotsPerLine);

		int centerCycleCounter = 0;
		int centerCycle = DotsPerLine / CenterObjectsPerLine;

		Vector3 centerDirection = Vector3.zero;

		for (int i = 0; i < samples.Length; i++) {

			Vector3 point = samples[i];

			centerDirection += point - (i > 0 ? samples[i-1] : point);

			if (centerCycleCounter++ >= centerCycle) {
				Instantiate(CenterGameobject, point, Quaternion.Euler(samples[i-1] - point)).transform.parent = parent.transform;
				centerDirection = Vector3.zero;
				centerCycleCounter = 0;
			}

			CenterLine.SetPosition(CenterLine.positionCount++, point);

			float angle = 0;
			foreach (var line in Lines) {
				// note that circle doesn't rotate with the center line; it's always along the xy plane
				Vector3 dot = point + Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * LineDistance;
				line.SetPosition(line.positionCount++, dot);

				angle += 360f / Lines.Length;
			}

		}

	}

}
