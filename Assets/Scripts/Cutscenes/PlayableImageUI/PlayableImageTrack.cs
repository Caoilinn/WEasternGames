using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Playables;

[TrackBindingType(typeof(Image))]
[TrackClipType(typeof(PlayableImageClip))]
public class PlayableImageTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<PlayableImageTrackMixer>.Create(graph,inputCount);
    }
}
