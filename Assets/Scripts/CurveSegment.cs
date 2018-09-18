﻿using UnityEngine;
using System.Collections.Generic;

// major credit to this article https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline for giving a full implementation in Unity
public class CurveSegment {
	
	// set from 0-1
	public float Alpha;

	// control points
	public Vector3 p0, p1, p2, p3;

	float t0, t1, t2, t3;
	
	public CurveSegment (Vector3 previous, Vector3 current, Vector3 next, Vector3 future, float alpha = 0.5f) {
		Alpha = alpha;

		p0 = previous;
		p1 = current;
		p2 = next;
		p3 = future;

		t0 = 0.0f;
		t1 = getT(t0, p0, p1);
		t2 = getT(t1, p1, p2);
		t3 = getT(t2, p2, p3);
	}
	
	// interpolates from 0 to 1 over the curve
	public Vector3 Position (float time) {
		float t = time * (t2 - t1) + t1;

		Vector3 A1 = (t1-t)/(t1-t0)*p0 + (t-t0)/(t1-t0)*p1;
		Vector3 A2 = (t2-t)/(t2-t1)*p1 + (t-t1)/(t2-t1)*p2;
		Vector3 A3 = (t3-t)/(t3-t2)*p2 + (t-t2)/(t3-t2)*p3;
		
		Vector3 B1 = (t2-t)/(t2-t0)*A1 + (t-t0)/(t2-t0)*A2;
		Vector3 B2 = (t3-t)/(t3-t1)*A2 + (t-t1)/(t3-t1)*A3;
		
		Vector3 C = (t2-t)/(t2-t1)*B1 + (t-t1)/(t2-t1)*B2;
		
		return C;
	}

	// returns pairs of Position, Velocity n times along the curve
	public System.Tuple<Vector3, Vector3>[] Samples (int n) {
		var samples = new System.Tuple<Vector3, Vector3>[n];

		for (int i = 0; i < n; i++) {
			float t = i * (1.0f / n);
			samples[i] = new System.Tuple<Vector3, Vector3>(Position(t), Velocity(t));
		}
		
		return samples;
	}

	// returns instantaneous derivative at time t on curve
	// from https://math.stackexchange.com/a/848290/574705
	public Vector3 Velocity (float t) {
		Vector3 Q1 = (p1 - p0) / (t1 - t0) - (p2 - p0) / (t2 - t0) + (p2 - p1) / (t2 - t1);
		Vector3 Q2 = (p2 - p1) / (t2 - t1) - (p3 - p1) / (t3 - t1) + (p3 - p2) / (t3 - t2);

		Vector3 R1 = p1 + Q1 * (t2 - t1) / 3f;
		Vector3 R2 = p2 - Q2 * (t2 - t1) / 3f;

		Vector3 dC_dt = 3*(Mathf.Pow(1-t, 2)*(R1-p1) + 2*t*(1-t)*(R2-R1) + Mathf.Pow(t, 2)*(p2-R2));
		return dC_dt / (t2 - t1);
	}

	public float ClosestDistanceToPoint (Vector3 point, int samples = 41) {
		float smallestDist = float.MaxValue;
		foreach (var sample in Samples(samples)) {
			float dist = Vector3.Distance(point, sample.Item1);
			smallestDist = Mathf.Min(smallestDist, dist);
		}
		return smallestDist;
	}

	float getT (float t, Vector3 p0, Vector3 p1) {
	    float a = Mathf.Pow((p1.x-p0.x), 2.0f) + Mathf.Pow((p1.y-p0.y), 2.0f) + Mathf.Pow((p1.z-p0.z), 2.0f);
	    float b = Mathf.Pow(a, 0.5f);
	    float c = Mathf.Pow(b, Alpha);
	   
	    return (c + t);
	}

}