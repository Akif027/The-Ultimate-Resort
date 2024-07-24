using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationHolder
{



    public CleanAnim cleanAnim;
    public AnimationHolder(CleanAnim cleanAnim)
    {
        this.cleanAnim = cleanAnim;
    }
    public void CleanAnimation()
    {
        // cleanAnim.PlayAnimation();
    }

    public void DirtyAnimation()
    {

        cleanAnim.PlayDirty();
    }
}


