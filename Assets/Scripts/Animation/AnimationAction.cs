using UnityEngine;

namespace UnityTemplateProjects.Animation
{
    public class AnimationAction
    {
        private readonly AnimationClip _animationClip;

        public AnimationAction(AnimationClip clip)
        {
            _animationClip = clip;
            AnimationClipLength = _animationClip.length;
        }

        public float AnimationClipLength { get; }

        public string AnimationClipName => _animationClip.name;
    }
}