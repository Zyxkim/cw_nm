using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolation : MonoBehaviour {

    public static float dot(Quaternion a, Quaternion b)
    {
        return a.x * b.x + a.y * b.y + a.z *  b.z + a.w * b.w;
    }

    public static Quaternion LERP(Quaternion q1, Quaternion q2, float t)
    {
        float dotNumber = dot(q1, q2);
        if (dotNumber < 0.0f) {
            return LERP(Multi(q1, -1.0f), q2, t);
        }

        Quaternion q_new;
        float delta_t = 1 - t;

        q_new.x = delta_t * q1.x + t * q2.x;
        q_new.y = delta_t * q1.y + t * q2.y;
        q_new.z = delta_t * q1.z + t * q2.z;
        q_new.w = delta_t * q1.w + t * q2.w;

        q_new.Normalize();
        return q_new;
    }

    public static Quaternion LERP_MIN(Quaternion q1, Quaternion q2, float t)
    {
        float dotNumber = dot(q1, q2);
        /*if (dotNumber < 0.0f) {
            return LERP(Multi(q1, -1.0f), q2, t);
        }*/
        if (Mathf.Abs(dotNumber) >= 1f) {
            return q1;
        }

        Quaternion q_new;
        float delta_t = 1 - t;

        q_new.x = delta_t * q1.x + t * q2.x;
        q_new.y = delta_t * q1.y + t * q2.y;
        q_new.z = delta_t * q1.z + t * q2.z;
        q_new.w = delta_t * q1.w + t * q2.w;

        q_new.Normalize();
        return q_new;
    }

    public static Quaternion SLERP(Quaternion q1, Quaternion q2, float t)
    {
        float dotNumber = Quaternion.Dot(q1, q2);
        /*if (dotNumber < 0.0f) {
            return SLERP(Multi(q1, -1.0f), q2, t);
        }*/

        if (Mathf.Abs(dotNumber) >= 1f) {
            return q1;
        }

        Quaternion q_new;
        float a, b;

        float delta_t = 1 - t;
        
        float theta = Mathf.Acos(Quaternion.Dot(q1, q2));
        float sin = Mathf.Sin(theta);

        a = Mathf.Sin(theta * delta_t) / sin;
        b = Mathf.Sin(theta * t) / sin;

        //a = Mathf.Sin(theta * delta_t) * sin;
        //b = Mathf.Sin(theta * t) * sin;

        q_new.x = a * q1.x + b * q2.x;
        q_new.y = a * q1.y + b * q2.y;
        q_new.z = a * q1.z + b * q2.z;
        q_new.w = a * q1.w + b * q2.w;

        q_new.Normalize();
        return q_new;
    }

    
    public static Quaternion SLERP_MIN(Quaternion q1, Quaternion q2, float t)
    {
        float dotNumber = Quaternion.Dot(q1, q2);
        if (dotNumber < 0.0f) {
            return SLERP(Multi(q1, -1.0f), q2, t);
        }

        Quaternion q_new;
        float a, b;

        float delta_t = 1 - t;
        
        float theta = Mathf.Acos(Quaternion.Dot(q1, q2));
        float sin = Mathf.Sin(theta);

        a = Mathf.Sin(theta * delta_t) / sin;
        b = Mathf.Sin(theta * t) / sin;

        //a = Mathf.Sin(theta * delta_t) * sin;
        //b = Mathf.Sin(theta * t) * sin;

        q_new.x = a * q1.x + b * q2.x;
        q_new.y = a * q1.y + b * q2.y;
        q_new.z = a * q1.z + b * q2.z;
        q_new.w = a * q1.w + b * q2.w;

        q_new.Normalize();
        return q_new;
    }

    public static Quaternion SplineSegment(Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3, float t)
    {
        Quaternion qa = Intermediate(q0, q1, q2);
        Quaternion qb = Intermediate(q1, q2, q3);
        return SQUAD(q1, qa, qb, q2, t);
    }

    static Quaternion SQUAD(Quaternion q1, Quaternion t1, Quaternion t2, Quaternion q2, float t)
	{
		float slerpT = 2.0f * t * (1.0f - t);
		Quaternion slerp1 =  SLERP(q1, q2, t);
		Quaternion slerp2 = SLERP(t1, t2, t);
		return SLERP(slerp1, slerp2, slerpT);
	}

    public static Quaternion SplineSegmentSB(Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3, float t)
    {        
        Quaternion helpCalc = Double(q0, q1);
        Quaternion bonea1 = SLERP(helpCalc, q2, 0.5f);
        bonea1 = SLERP(q1, bonea1, 1f / 3f);

        helpCalc = Double(q1, q2);
        Quaternion bonea2 = SLERP(helpCalc, q3, 0.5f);
        Quaternion boneb2 = SLERP(q2, bonea2, -1f / 3f);

        return ShoBez(q1, bonea1, boneb2, q2, t);
    }

    static Quaternion Double(Quaternion p, Quaternion q)
    {
        float dot_ = (p.x * q.x + p.y * q.y + p.z * q.z + p.w * q.w);
        Quaternion res =  Multi(q, 2 * dot_);
        return Minus_(res, p);
    }

    static Quaternion ShoBez(Quaternion q1, Quaternion t1, Quaternion t2, Quaternion q2, float t)
	{
		float slerpT = 2.0f * t * (1.0f - t);
		return SLERP(SLERP(SLERP(q1, t1, t), SLERP(t1, t2, t), t), SLERP(SLERP(t1, t2, t), SLERP(t2, q2, t), t), t);
	    //return SLERP(SLERP(SLERP(q1, t1, t), SLERP(t1, t2, t), t), SLERP(SLERP(t1, t2, t), SLERP(t2, q2, t), t), t);
	}


    static Quaternion Intermediate(Quaternion q0, Quaternion q1, Quaternion q2 )
    {
        Quaternion q1inv  = Quaternion.Inverse(q1);
        Quaternion c1 = q1inv * q2;
        Quaternion c2 = q1inv * q0;
		Log(ref c1);
		Log(ref c2);
		Quaternion c3 = Add(c2, c1);
		Scale(ref c3, -0.25f);
        Exp(ref c3);
        Quaternion r  = q1 * c3;
        r.Normalize();
        return r;
    }


    static public void Scale(ref Quaternion a, float s){
		a.w *= s;
		a.x *= s;
		a.y *= s;
		a.z *= s;
	}


    static public Quaternion Multi(Quaternion a, float s){
		a.w *= s;
		a.x *= s;
		a.y *= s;
		a.z *= s;
        return a;
	}

    static public void Exp(ref Quaternion a){
		float angle = Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

		float sinAngle = Mathf.Sin(angle);
		a.w = Mathf.Cos(angle);

		if (Mathf.Abs(sinAngle) >= 1.0e-15){
			float coeff = sinAngle / angle;
			a.x *= coeff;
			a.y *= coeff;
			a.z *= coeff;
		}
	}

	static public void Log(ref Quaternion a){
		float a0 = a.w;
		a.w = 0.0f;
		if (Mathf.Abs(a0) < 1.0f){
			float angle = Mathf.Acos(a0);
			float sinAngle = Mathf.Sin(angle);
			if (Mathf.Abs(sinAngle) >= 1.0e-15){
				float coeff = angle / sinAngle;
				a.x *= coeff;
				a.y *= coeff;
				a.z *= coeff;
			}
		}
	}

    static public Quaternion Add(Quaternion a, Quaternion b){
		var r = new Quaternion();
		r.w = a.w + b.w;
		r.x = a.x + b.x;
		r.y = a.y + b.y;
		r.z = a.z + b.z;
		return r;
	}


    static public Quaternion Minus_(Quaternion a, Quaternion b){
		var r = new Quaternion();
		r.w = a.w - b.w;
		r.x = a.x - b.x;
		r.y = a.y - b.y;
		r.z = a.z - b.z;
		return r;
	}
}