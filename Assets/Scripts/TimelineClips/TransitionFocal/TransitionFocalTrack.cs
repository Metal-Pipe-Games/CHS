using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

[TrackColor(0.2f,0.9f,0f)]
[TrackBindingType(typeof(VolumeProfile))]
[TrackClipType(typeof(TransitionFocalClip))]
public class TransitionFocalTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TransitionFocalMixer>.Create(graph, inputCount);
    }
}
