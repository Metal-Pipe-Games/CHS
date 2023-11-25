using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TransitionFocalMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        VolumeProfile postProcessing = playerData as VolumeProfile;

        int inputCount = playable.GetInputCount();

        float totalWeight = 0;
        float d = 0;
        float l = 0;
        float a = 0;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TransitionFocalBehaviour> inputPlayable = (ScriptPlayable<TransitionFocalBehaviour>)playable.GetInput(i);
            TransitionFocalBehaviour behaviour = inputPlayable.GetBehaviour();

            var dur = inputPlayable.GetDuration();
            var time = inputPlayable.GetTime();

            var p = time / dur;

            var td = Mathf.Lerp(behaviour.fromDistance, behaviour.toDistance, (float)p);
            var tl = Mathf.Lerp(behaviour.fromLength, behaviour.toLength, (float)p);
            var ta = Mathf.Lerp(behaviour.fromAperture, behaviour.toAperture, (float)p);

            d += td * inputWeight;
            l += tl * inputWeight;
            a += ta * inputWeight;

            totalWeight += inputWeight;
        }
        if (totalWeight == 0)
        {
            return;
        }
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