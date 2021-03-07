using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpritesheetAnimationClip : PlayableAsset
{
    public Texture2D spritesheetTexture;
    public int frameCount;
    public float framesPerSecond;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpritesheetAnimationBehaviour>.Create(graph);

        SpritesheetAnimationBehaviour behaviour = playable.GetBehaviour();

        behaviour.spritesheetTexture = spritesheetTexture;
        behaviour.frameCount = frameCount;
        behaviour.framesPerSecond = framesPerSecond;
        behaviour.isPlaying = false;

        return playable;
    }
}
