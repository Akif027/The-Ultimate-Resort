using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : MonoBehaviour
{
    public float scaleUpDuration = 0.5f;
    public float scaleDownDuration = 0.5f;
    public float scaleMultiplier = 1.5f;
    public Color upgradeColor = Color.yellow;

    public Transform Efectpos;

    public void PlayOnUpgrade()
    {


        CameraTransition.Instance.FocusOnTarget(Efectpos);
        Effect.Instance.PlayEffect("Upgrade", Efectpos.position);
        DoTweenManager.PlayUpgradeAnimation(this.transform, scaleUpDuration, scaleDownDuration, scaleMultiplier, upgradeColor);
    }


}
