using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayableImageTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Image imageObject = playerData as Image;

        if (!imageObject) { return; }

        Color currentColor = new Color(0,0,0,0);
        float currentAlpha = 0;

        int numberOfClips = playable.GetInputCount();
        for (int i = 0; i < numberOfClips; i++) {
            float clipWeight = playable.GetInputWeight(i);

            if (clipWeight > 0f) {
                ScriptPlayable<PlayableImageBehaviour> clipPlayable = (ScriptPlayable<PlayableImageBehaviour>)playable.GetInput(i);

                PlayableImageBehaviour clipBehaviour = clipPlayable.GetBehaviour();
                currentColor = clipBehaviour.imageColor;
                currentAlpha = clipWeight;
            }
        }

        imageObject.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
    }
}
