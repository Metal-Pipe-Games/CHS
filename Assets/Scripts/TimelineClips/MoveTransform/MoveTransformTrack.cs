using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.9f,0.3f,0f)]
[TrackBindingType(typeof(Transform))]
[TrackClipType(typeof(MoveTransformClip))]
public class MoveTransformTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MoveTransformMixer>.Create(graph, inputCount);
    }
}
