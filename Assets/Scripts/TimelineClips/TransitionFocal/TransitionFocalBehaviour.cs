using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class TransitionFocalBehaviour : PlayableBehaviour
{
    public float fromDistance;
    public float toDistance;
    public float fromLength;
    public float toLength;
    public float fromAperture;
    public float toAperture;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        VolumeProfile postProcessing = playerData as VolumeProfile;

        var time = playable.GetTime();
        var dur = playable.GetDuration();
        var p = time / dur;

        var d = Mathf.Lerp(fromDistance, toDistance, (float)p);
        var l = Mathf.Lerp(fromLength, toLength, (float)p);
        var a = Mathf.Lerp(fromAperture, toAperture, (float)p);
        if (postProcessing.TryGet(out DepthOfField dof))
        {
            dof.focusDistance.value = d;
            dof.focalLength.value = l;
            dof.aperture.value = a;
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
    }
}
