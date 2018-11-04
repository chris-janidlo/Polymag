using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class Track : Singleton<Track>
{
	[Tooltip("The initial number of segments making up the overall line. Changing this value after the script is loaded has no effect")]
	public int InitialCurves;

	[Tooltip("Spawned at intervals along the line")]
	public GameObject CenterGameObject;

	public LineRenderer CenterLine;

	[Tooltip("Define multiple line renderers here. Lines are distributed equally in a circle around the center, definition line")]
	public LineRenderer[] Lines;
	[Tooltip("Distance between the outer lines and the center one as a function of the number of curve segments")]
	public AnimationCurve TrackRadius;
	[Tooltip("Resolution of each line")]
	public int DotsPerLine = 20;
	public int CenterObjectsPerLine = 5;

	List<CurveSegment> curves;
	List<GameObject> curveObjects;
	Dictionary<int, CurveSegment> curvesByClosestZPosition;

	CurveSegment previous { get { return curves[curves.Count - 1]; } }

	Transform curveParent;

	void Start ()
	{
		SingletonSetInstance(this, true);		

		curves = new List<CurveSegment>();
		curveObjects = new List<GameObject>();
		curvesByClosestZPosition = new Dictionary<int, CurveSegment>();

		curveParent = new GameObject("Curves").transform;
		curveParent.parent = transform;

		curves.Add(new CurveSegment(Vector3.back * 5, Vector3.back * 4, Vector3.back * 3, Vector3.back * 2, TrackRadius.Evaluate(0)));

		for (int i = 0; i < InitialCurves; i++)
			addNewCurve();
	}

	public Vector3 GetPositionAt (float s)
	{
		return getCurveAt(s).Position(s % 1);
	}

	public Vector3 GetVelocityAt (float s)
	{
		return getCurveAt(s).Velocity(s % 1);
	}

	public float DistanceFromCurve (Vector3 point, int samples)
	{
		int z = (int) point.z;
		
		if (z < curvesByClosestZPosition.Keys.Min())
		{
			return 0;
		}
		
		return curvesByClosestZPosition[z].ClosestDistanceToPoint(point, samples);
	}

	CurveSegment getCurveAt (float s)
	{
		int index = Mathf.FloorToInt(s);
		// FIXME: calls addNewCurve twice in same frame?
		if (index > curves.Count / 2) addNewCurve();

		return curves[index];
	}

	Vector3 newPointOffset ()
	{
		Vector2 xy = Random.insideUnitCircle * 60;
		float z = Random.Range(30, 50);

		return new Vector3(xy.x, xy.y, z);
	}

	void addNewCurve ()
	{
		CurveSegment prev = previous;

		float nextRadius = TrackRadius.Evaluate(curves.Count + 1);
		CurveSegment next = new CurveSegment(prev.p1, prev.p2, prev.p3, prev.p3 + newPointOffset(), nextRadius);

		curves.Add(next);

		for (int i = (int) next.p1.z; i < (int) next.p2.z; i++)
		{
			curvesByClosestZPosition[i] = next;
		}

		var parent = new GameObject("Curve " + curves.Count);
		parent.transform.parent = curveParent;
		curveObjects.Add(parent);

		var positions = previous.SamplePositions(DotsPerLine);
		var velocities = previous.SampleVelocities(DotsPerLine);

		int centerCycleCounter = 0;
		int centerCycle = DotsPerLine / CenterObjectsPerLine;

		Vector3 centerDirection = Vector3.zero;

		for (int i = 0; i < positions.Length; i++)
		{
			Vector3 point = positions[i];

			centerDirection += point - (i > 0 ? positions[i-1] : point);

			if (centerCycleCounter++ >= centerCycle)
			{
				Instantiate(CenterGameObject, point, Quaternion.Euler(positions[i-1] - point)).transform.parent = parent.transform;
				centerDirection = Vector3.zero;
				centerCycleCounter = 0;
			}

			CenterLine.SetPosition(CenterLine.positionCount++, point);

			float angle = 0;
			foreach (var line in Lines)
			{
				Vector3 dir = Vector3.Slerp(Vector3.forward, velocities[i].normalized, .25f);
				Vector3 dot = point + Quaternion.AngleAxis(angle, dir) * Vector3.right * next.Radius;
				line.SetPosition(line.positionCount++, dot);

				angle += 360f / Lines.Length;
			}
		}
	}
}
