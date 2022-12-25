using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestController : MonoBehaviour
{
    [SerializeField] [Range(0f,5f)] float lerpTime;
    /*Quaternion[] myAngles = {
        Quaternion.Euler(12, 323, 0),
        Quaternion.Euler(23, 0, 41),
        Quaternion.Euler(12, 187, 302),
        Quaternion.Euler(0, 145, 276)
    };*/

    Quaternion[] myAngles = {
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(0, 0, 90),
        Quaternion.Euler(90, 0, 90),
        Quaternion.Euler(0, 90, 0)
    };

    //[SerializeField] Vector3[] myAngles;

    [SerializeField] bool regular;

    [SerializeField] bool lerp;
    [SerializeField] bool my_lerp;
    [SerializeField] bool my_lerp_min;


    [SerializeField] bool slerp;
    [SerializeField] bool my_slerp;
    [SerializeField] bool my_slerp_min;

    [SerializeField] bool circular_blending;
    [SerializeField] bool circular_blending_;

    [SerializeField] bool bezier;

    [SerializeField] bool squad;

    int angleIndex = 0;
    int angleNumber = 0;
    int len;
    [SerializeField] [Range(0f,1f)] float t;
    //float t = 0;
    float w_ = 0;

    float reg_t = 0f;
    Quaternion q0;
    Quaternion q1;
    Quaternion q2;
    Quaternion q3;
    Quaternion q4;

    int prev_index = 0;
    int next_index = 0;
    int next_next_index = 0;
    int prev_prev_index = 0;

    [Range(2, 64)] public int arcPoints = 16;
    string _lerp = "";
    string _slerp = "";
    string _squad = "";
    string _bezier = "";

    string _t = "";

    string ListToText(List<float> list)
     {
        string text = "";
        foreach (int item in list) {
            text += item.ToString("F3") + ", ";
        }
        return text;
     }

    void Start()
    {
        len = myAngles.Length;

        prev_prev_index = len - 2;
        prev_index = len - 1;
        next_index = angleIndex + 1;
        next_next_index = next_index + 1;

        //q0 = myAngles[angleIndex];
        q0  = Quaternion.Euler(0, 0, 0);
        q1 = transform.rotation;
        q2 = myAngles[angleIndex];
        q3 = myAngles[next_index];
        q4 = myAngles[next_next_index];
    }

    // Update is called once per frame
    void Update()
    {   
        //q0 = myAngles[prev_index];
        //q0  = transform.rotation;
        q1 = transform.rotation;
        q2 = myAngles[angleIndex];
        q3 = myAngles[next_index];
        q4 = myAngles[next_next_index];

        if (t < 2f){
        if (regular) {
            var angles = myAngles[angleIndex].eulerAngles;
            angles.x += lerpTime * Time.deltaTime;
            angles.y += lerpTime * Time.deltaTime;
            angles.z += lerpTime * Time.deltaTime;
            transform.rotation = myAngles[angleIndex];
        } else if (lerp){
            Quaternion q1 = transform.rotation;
            transform.rotation = Quaternion.Lerp(myAngles[prev_index], myAngles[angleIndex], t);
            Quaternion q2 = Quaternion.Lerp(myAngles[prev_index], myAngles[angleIndex], t);
            Vector3 v = new Vector3(
                q1.x * q2.y - q1.y * q2.x - q1.z * q2.w + q1.w * q2.z,
                q1.x * q2.z + q1.y * q2.w - q1.z * q2.x - q1.w * q2.y,
                q1.x * q2.w - q1.y * q2.z + q1.z * q2.y - q1.w * q2.x
            );
            float vel = (2 / 0.2f) * v.magnitude;
            Debug.Log("AAAAAAAAAAAAAAA");
            Debug.Log(vel);
            Debug.Log("BBBBBBBBBBBBBBB");
        } else if (my_lerp){
            transform.rotation = Interpolation.LERP(myAngles[prev_index], myAngles[angleIndex], t);
            _lerp += Interpolation.LERP(myAngles[prev_index], myAngles[angleIndex], t).w + "| ";
        } else if (my_lerp_min){
            transform.rotation = Interpolation.LERP_MIN(myAngles[prev_index], myAngles[angleIndex], t);
        } else if (slerp){
            Quaternion q1 = myAngles[prev_index];
            transform.rotation = Quaternion.Slerp(myAngles[prev_index], myAngles[angleIndex], t);
            Quaternion q2 =  Quaternion.Slerp(myAngles[prev_index], myAngles[angleIndex], t);
            Vector3 v = new Vector3(
                q1.x * q2.y - q1.y * q2.x - q1.z * q2.w + q1.w * q2.z,
                q1.x * q2.z + q1.y * q2.w - q1.z * q2.x - q1.w * q2.y,
                q1.x * q2.w - q1.y * q2.z + q1.z * q2.y - q1.w * q2.x
            );
            Debug.Log(q2);
            float vel = (2 / 0.2f) * v.magnitude;
            Debug.Log("AAAAAAAAAAAAAAA");
            Debug.Log(vel);
            Debug.Log("BBBBBBBBBBBBBBB");
        } else if (my_slerp){
            transform.rotation = Interpolation.SLERP(myAngles[prev_index], myAngles[angleIndex], t);
        }  else if (my_slerp_min){
            transform.rotation = Interpolation.SLERP_MIN(myAngles[prev_index], myAngles[angleIndex], t);
            _slerp += Interpolation.SLERP_MIN(myAngles[prev_index], myAngles[angleIndex], t).w + "| ";
        }  else if (squad){
            transform.rotation = Interpolation.SplineSegment(myAngles[prev_prev_index], myAngles[prev_index], myAngles[angleIndex], myAngles[next_index], t);
            _squad += Interpolation.SplineSegment(myAngles[prev_prev_index], myAngles[prev_index], myAngles[angleIndex], myAngles[next_index], t).w + "| ";
        }  else if (bezier){
            transform.rotation = Interpolation.SplineSegmentSB(myAngles[prev_prev_index], myAngles[prev_index], myAngles[angleIndex], myAngles[next_index], t);
            _bezier += Interpolation.SplineSegmentSB(myAngles[prev_prev_index], myAngles[prev_index], myAngles[angleIndex], myAngles[next_index], t).w + "| ";
            //w_ = transform.rotation.w;
            /*Debug.Log('+');
            Debug.Log(t);
            Debug.Log(transform.rotation.w);
            Debug.Log('+');*/
        }
        }

        //_t += t + "| ";

        //t = t + lerpTime * Time.deltaTime;
        //t = t + 0.01f;
        //t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        reg_t = lerpTime * Time.deltaTime;
        if (t > 1f) {
            t = t - 1f;
        }
        if (t == 1f) {
            //q0 = Interpolation.CircularBlending(q2, q3, q4, t);
            if (t < 2) {
                Debug.Log("_lerp");
                Debug.Log(_lerp);
                Debug.Log("_slerp");
                Debug.Log(_slerp);
                Debug.Log("_squad");
                Debug.Log(_squad);
                Debug.Log("_bezier");
                Debug.Log(_bezier);
                //Debug.Log(ListToText(_squad));
                //Debug.Log(ListToText(_bezier));
                Debug.Log(_t);
            }

            t = t - 1f;
            reg_t = 0f;

            angleIndex = angleIndex + 1;
            
            if (angleIndex == len) {
                prev_prev_index = len - 2;
                prev_index = len - 1;
                angleIndex = 0;
                next_index = angleIndex + 1;
                next_next_index = next_index + 1;
            } else if (angleIndex == len - 1) {
                prev_prev_index = len - 3;
                prev_index = len - 2;
                angleIndex = len - 1;
                next_index = 0;
                next_next_index = 1;
            } else if (angleIndex == len - 2) {
                prev_prev_index = 0;
                prev_index = len - 3;
                angleIndex = len - 2;
                next_index = len - 1;
                next_next_index = 0;
            } else {
                prev_prev_index = len - 1;
                prev_index = angleIndex - 1;
                next_index = angleIndex + 1;
                next_next_index = next_index + 1;
            }
            /*
            prev_index = angleIndex - 1;
            next_index = angleIndex + 1;
            next_next_index = next_index + 1;
            if (next_index == len) {
                next_index = 0;
                next_next_index = 1;
            }
            if (next_next_index == len) {
                next_next_index = 0;
            }
            if (prev_index < 0) {
                prev_index = len - 1;
            }*/
            q0 = myAngles[prev_index];
            //q0  = transform.rotation;
            q1 = transform.rotation;
            q2 = myAngles[angleIndex];
            q3 = myAngles[next_index];
            q4 = myAngles[next_next_index];
        }
    }
}
