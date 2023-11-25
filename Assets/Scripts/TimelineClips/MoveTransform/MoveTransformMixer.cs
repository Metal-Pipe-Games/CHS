using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MoveTransformMixer : PlayableBehaviour
{
    [Range(-1, 1)]
    [InspectorName("Strength")]
    public float s = 1;
    public bool inv = false;

    private bool firstFramePassed;

    bool useTemp = false;
    Transform temp;

    Transform fromTransform;
    Transform toTransform;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Debug.Log("Got here");
        var targetTransform = playerData as Transform;

        int inputCount = playable.GetInputCount();
        Debug.Log(inputCount);

        Vector3 newPos = Vector3.zero;
        Quaternion newRot = Quaternion.identity;
        float totalWeight = 0;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<MoveTransformBehaviour> inputPlayable = (ScriptPlayable<MoveTransformBehaviour>)playable.GetInput(i);
            MoveTransformBehaviour behaviour = inputPlayable.GetBehaviour();

            if (behaviour.fromTransform == null || behaviour.toTransform == null) continue;
            var dur = inputPlayable.GetDuration();
            var time = inputPlayable.GetTime();

            var p = time / dur;

            fromTransform = behaviour.fromTransform;
            toTransform = behaviour.toTransform;
            behaviour.CalculateResult(p, out Vector3 np, out Quaternion nr);
            newPos += np * inputWeight;
            newRot *= Quaternion.Lerp(Quaternion.identity,nr,inputWeight);
            totalWeight += inputWeight;
            Debug.Log(inputWeight + "   " + i);
        }
        if (totalWeight == 0)
        {
            return;
        }

        targetTransform.SetPositionAndRotation(newPos, newRot);
    }

    public void CalculateResult(double p, out Vector3 newPos, out Quaternion newRot)
    {
        var t = CalculateMult(p);
        Debug.Log(t);
        var cFromTransform = useTemp ? temp : fromTransform;

        newPos = Vector3.Lerp(cFromTransform.position, toTransform.position, t);
        newRot = Quaternion.Lerp(cFromTransform.rotation, toTransform.rotation, t);
    }

    private float CalculateMult(double val)
    {
        float x = (float)(inv ? 1 - val : val);
        if (s * s == 1) return x;
        float v = (Mathf.Pow(s * s, x) - 1) / (s * s - 1);
        if (s > 0) return v;
        else return 1 - v;
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