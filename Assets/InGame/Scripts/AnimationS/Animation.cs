using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public Animator anim;


    public void AnimationPlay(string animName, bool isTrue)
    {
        anim.SetBool(animName, isTrue);

    }

    public void AnimationPlay(string animName)
    {
        anim.SetTrigger(animName);

    }
}