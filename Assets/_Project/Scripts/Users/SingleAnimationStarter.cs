using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SingleAnimationStarter : MonoBehaviour
{

    [SerializeField] private AnimationClip _animationClip;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animationClip)
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(_animator.runtimeAnimatorController);

            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[0], _animationClip));

            aoc.ApplyOverrides(anims);
            _animator.runtimeAnimatorController = aoc;
        }


    }

}
