using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableImageClip : PlayableAsset
{
    public Color imageColor;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PlayableImageBehaviour>.Create(graph);

        PlayableImageBehaviour playableImageBehaviour = playable.GetBehaviour();
        playableImageBehaviour.imageColor = imageColor;

        return playable;
    }
}
