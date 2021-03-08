using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SpritesheetAnimationBehaviour : PlayableBehaviour
{
    public Texture2D spritesheetTexture;
    public int frameCount;
    public float framesPerSecond;
    public bool isPlaying;
}
