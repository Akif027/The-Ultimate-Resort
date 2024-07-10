using System;
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
    public void SetFloat(string name, float value)
    {
        if (anim == null)
        {
            return;
        }

        anim.SetFloat(name, value);
    }


    public int GetInt(string name)
    {
        if (anim == null)
        {
            return -1;
        }

        return anim.GetInteger(name);
    }

    public void SetInt(string name, int value)
    {
        if (anim == null)
        {
            return;
        }

        anim.SetInteger(name, value);
    }
    public void ActiveMove()
    {
        SetInt("State", 0);
    }

    public virtual void ActiveIdle()
    {
        SetInt("State", 1);
    }

    public void AnimationPlay(String value, bool _isTrue)
    {
        anim.SetBool(value, _isTrue);
    }

    void Update()
    {
        PlayCurrentStateAnimation();
    }
    public void SetBlendIdle(float value)
    {
        SetFloat("Idle_Blend", value);
    }

    public void SetBlendMove(float value)
    {
        SetFloat("Move_Blend", value);
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
