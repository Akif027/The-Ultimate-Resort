using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashAnimation : MonoBehaviour
{
    public Transform ani;
    public Vector3 target;
    private Vector3 firstPos;

    private void Start()
    {
        firstPos = ani.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        ani.DOLocalMove(target, 0.3f);
    }

    private void OnTriggerExit(Collider other)
    {
        ani.DOLocalMove(firstPos, 0.3f);
    }
}
