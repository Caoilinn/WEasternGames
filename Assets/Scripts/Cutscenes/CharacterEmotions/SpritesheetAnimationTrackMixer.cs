using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpritesheetAnimationTrackMixer : PlayableBehaviour
{
    private bool isStopped = true;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        SpritesheetAnimation spritesheetAnimation = playerData as SpritesheetAnimation;

        if (!spritesheetAnimation) {return;}

        int numberOfClips = playable.GetInputCount();
        bool playedClip = false;

        for (int i = 0 ; i < numberOfClips; i++) {

            float currentClipWeight = playable.GetInputWeight(i);
            ScriptPlayable<SpritesheetAnimationBehaviour> scriptPlayable = (ScriptPlayable<SpritesheetAnimationBehaviour>)playable.GetInput(i);
            SpritesheetAnimationBehaviour spritesheetBehaviour = scriptPlayable.GetBehaviour();

            if (currentClipWeight > 0) {
                if (!spritesheetBehaviour.isPlaying && spritesheetBehaviour.spritesheetTexture) {
                    spritesheetBehaviour.isPlaying = true;
                    spritesheetAnimation.SetSpritesheetTexture(spritesheetBehaviour.spritesheetTexture, spritesheetBehaviour.frameCount, spritesheetBehaviour.framesPerSecond);
                    spritesheetAnimation.StartAnimating();
                    isStopped = false;
                }

                playedClip = true;
            } else {
                spritesheetBehaviour.isPlaying = false;
            }
        } 

        if (!playedClip && !isStopped) {
            spritesheetAnimation.StopAnimating();
            isStopped = true;
        }
    }
}
