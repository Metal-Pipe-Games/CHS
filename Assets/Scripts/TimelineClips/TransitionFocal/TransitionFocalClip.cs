using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

[Serializable]
public class TransitionFocalClip : PlayableAsset, ITimelineClipAsset
{
    public float fromDistance;
    public float toDisance;
    public float fromLength;
    public float toLength;
    public float fromAperture;
    public float toAperture;

    public ClipCaps clipCaps { get { return ClipCaps.Blending; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        TransitionFocalBehaviour temp = new();

        temp.fromDistance = fromDistance;
        temp.toDistance = toDisance;
        temp.fromLength = fromLength;
        temp.toLength = toLength;
        temp.fromAperture = fromAperture;
        temp.toAperture = toAperture;

        return ScriptPlayable<TransitionFocalBehaviour>.Create(graph, temp);
    }
}
