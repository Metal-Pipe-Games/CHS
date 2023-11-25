using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class MoveTransformBehaviour : PlayableBehaviour
{
    public Transform fromTransform;
    public Transform toTransform;

    [Range(-1, 1)]
    [InspectorName("Strength")]
    public float s = 1;
    public bool inv = false;

    private bool firstFramePassed;

    bool useTemp = false;
    Transform temp;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var targetTransform = playerData as Transform;
        if (fromTransform == null || toTransform == null || targetTransform == null) return;
        if (!firstFramePassed)
        {
            firstFramePassed = true;
            if (targetTransform.Equals(fromTransform))
            {
                temp = new GameObject().transform;
                temp.SetPositionAndRotation(fromTransform.position, fromTransform.rotation);
                useTemp = true;
            }
        }

        var time = playable.GetTime();
        var dur = playable.GetDuration();
        var p = time / dur;

        CalculateResult(p, out Vector3 np, out Quaternion nr);

        targetTransform.SetPositionAndRotation(np, nr);
    }

    public void CalculateResult(double p, out Vector3 newPos, out Quaternion newRot)
    {
        var t = CalculateMult(p);
        //Debug.Log(t);
        var cFromTransform = useTemp ? temp : fromTransform;

        newPos = Vector3.Lerp(cFromTransform.position, toTransform.position, t);
        newRot = Quaternion.Lerp(cFromTransform.rotation, toTransform.rotation, t);
    }

    private float CalculateMult(double val)
    {
        float x = (float)(inv ? 1 - val : val);
        if (s * s == 1) return x;
        float v = (Mathf.Pow(s*s, x) - 1) / (s*s - 1);
        if (s > 0) return v;
        else return 1 - v;
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        firstFramePassed = false;
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        if (temp)
        {
            UnityEngine.Object.DestroyImmediate(temp.gameObject);
            temp = null;
        }
        base.OnPlayableDestroy(playable);
    }
}
