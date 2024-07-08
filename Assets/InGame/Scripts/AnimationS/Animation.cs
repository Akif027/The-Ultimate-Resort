using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator anim;
    [SerializeField] AnimationState currentState;

    private Dictionary<AnimationState, string> animationMap;
    public AnimationState CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;
            PlayCurrentStateAnimation(); // Optionally trigger animation play here
        }
    }
    void Start()
    {
        animationMap = new Dictionary<AnimationState, string>
        {
            { AnimationState.Idle, "Idle" },
            { AnimationState.Walk, "Walk" },
            { AnimationState.Sleeping, "Sleeping" },
            { AnimationState.ToiletIdle, "ToiletIdle" },
            { AnimationState.ToiletWalk, "ToiletWalk" },
            { AnimationState.Sit, "Sit" },
            { AnimationState.Swim, "Swim" },
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
        if (animationMap.TryGetValue(CurrentState, out string animationName))
        {

            anim.Play(animationName);
        }
    }

    public void ChangeState(AnimationState newState)
    {
        currentState = newState;
    }

    //     public void AnimationPlay(string animName, bool isTrue)
    //     {
    //         anim.SetBool(animName, isTrue);
    //     }

    //     public void AnimationPlay(string animName)
    //     {
    //         anim.SetTrigger(animName);
    //     }
    // }
}
public enum AnimationState
{
    Idle,
    Walk,
    Sleeping,
    Jump,
    ToiletIdle,
    ToiletWalk,
    Sit,
    Swim
    // Add other states as needed
}
