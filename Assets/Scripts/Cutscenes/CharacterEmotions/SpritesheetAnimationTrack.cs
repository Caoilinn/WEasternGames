using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackBindingType(typeof(SpritesheetAnimation))]
[TrackClipType(typeof(SpritesheetAnimationClip))]
public class SpritesheetAnimationTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SpritesheetAnimationTrackMixer>.Create(graph,inputCount);
    }
}
