using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator anim;
    private AnimationState currentState;

    private Dictionary<AnimationState, string> animationMap;

    void Start()
    {
        animationMap = new Dictionary<AnimationState, string>
        {
            { AnimationState.Idle, "Idle" },
            { AnimationState.Walk, "Walk" },
            { AnimationState.Run, "Run" },
            { AnimationState.Jump, "Jump" },
            // Add mappings for other states as needed
        };

        // Example of setting an initial state
        currentState = AnimationState.Idle;
    }

    void Update()
    {
        PlayCurrentStateAnimation();
    }

    private void PlayCurrentStateAnimation()
    {
        if (animationMap.TryGetValue(currentState, out string animationName))
        {
            AnimationPlay(animationName);
        }
    }

    public void ChangeState(AnimationState newState)
    {
        currentState = newState;
    }

    public void AnimationPlay(string animName, bool isTrue)
    {
        anim.SetBool(animName, isTrue);
    }

    public void AnimationPlay(string animName)
    {
        anim.SetTrigger(animName);
    }
}

public enum AnimationState
{
    Idle,
    Walk,
    Run,
    Jump,
    // Add other states as needed
}