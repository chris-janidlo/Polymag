using UnityEngine;
using System.Collections.Generic;

// major credit to this article https://en.wikipedia.org/wiki/Centripetal_Catmull%E2%80%93Rom_spline for giving a full implementation in Unity
public class Curve {
	
	// set from 0-1
	public float Alpha;

	// control points
	public Vector3 p0, p1, p2, p3;

	float t0, t1, t2, t3;
	
	public Curve (Vector3 previous, Vector3 current, Vector3 next, Vector3 future, float alpha = 0.5f) {
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

	public List<Vector3> Samples (int n) {
		var samples = new List<Vector3>();

		for (float t = 0; t < 1; t += 1/n)
			samples.Add(Position(t));
		
		return samples;
	}

	// returns instantaneous derivative at time t on curve
	// from https://math.stackexchange.com/a/848290/574705
	public Vector3 Velocity (float t) {
		Vector3 Q1 = (p1 - p0) / (t1 - t0) - (p2 - p0) / (t2 - t0) + (p2 - p1) / (t2 - t1);
		Vector3 Q2 = (p2 - p1) / (t2 - t1) - (p3 - p1) / (t3 - t1) + (p3 - p2) / (t3 - t2);

		Vector3 R1 = p1 + (t2 - t1) / 3 * Q1;
		Vector3 R2 = p2 + (t2 - t1) / 3 * Q2;

		float u = (t - t1) / (t2 - t1);

		Vector3 dC_du = 3*(Mathf.Pow(1-u, 2)*(R1-p1) + 2*u*(1-u)*(R2-R1) + Mathf.Pow(u, 2)*(p2-R2));
		return dC_du / (t2 - t1);
	}

	float getT (float t, Vector3 p0, Vector3 p1) {
	    float a = Mathf.Pow((p1.x-p0.x), 2.0f) + Mathf.Pow((p1.y-p0.y), 2.0f) + Mathf.Pow((p1.z-p0.z), 2.0f);
	    float b = Mathf.Pow(a, 0.5f);
	    float c = Mathf.Pow(b, Alpha);
	   
	    return (c + t);
	}

}