using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class SubtitleTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI subtitleTextObject = playerData as TextMeshProUGUI;

        if (!subtitleTextObject) { return; }

        string currentText = "";
        float currentAlpha = 0.0f;

        int numberOfClips = playable.GetInputCount();
        for (int i = 0; i < numberOfClips; i++) {
            float clipWeight = playable.GetInputWeight(i);

            if (clipWeight > 0f) {
                ScriptPlayable<SubtitleBehaviour> clipPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);

                SubtitleBehaviour clipBehaviour = clipPlayable.GetBehaviour();
                currentText = clipBehaviour.subtitleText;
                currentAlpha = clipWeight;
            }
        }

        subtitleTextObject.text = currentText;
        subtitleTextObject.color = new Color(1,1,1, currentAlpha);

    }
}
