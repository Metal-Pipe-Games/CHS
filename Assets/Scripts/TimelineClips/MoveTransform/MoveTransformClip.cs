using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MoveTransformClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<Transform> fromTransform;

    public ExposedReference<Transform> toTransform;

    [Range(-1,1)]
    public float strength = 1;
    public bool inverted = false;

    public ClipCaps clipCaps { get { return ClipCaps.Blending; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        MoveTransformBehaviour temp = new();
        temp.fromTransform = fromTransform.Resolve(graph.GetResolver());
        temp.toTransform = toTransform.Resolve(graph.GetResolver());
        temp.s = strength;
        temp.inv = inverted;
        return ScriptPlayable<MoveTransformBehaviour>.Create(graph, temp);
    }
}
