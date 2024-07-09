using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SortObject : MonoBehaviour
{
    private SortManager sortManager;
    public float height;
    public AnimationCurve acYUp;
    public AnimationCurve acYDown;

    private List<SortObject> SortObjects
    {
        get
        {
            return sortManager.sortObjects;
        }
    }

    public int Index
    {
        get
        {
            return SortObjects.IndexOf(this);
        }
    }

    public Vector3 Position
    {
        get
        {
            float h = 0;

            if (Index == 0)
            {
                return Vector3.zero;
            }

            for (int i = 1; i <= Index; i++)
            {
                h += SortObjects[i - 1].height;
            }
            return Vector3.up * h;
        }
    }

    public void Initialize(SortManager sortManager)
    {
        this.sortManager = sortManager;
    }

    public void MoveToPosition(float time = 0.3f)
    {
        if (transform.localPosition == Position)
        {
            return;
        }

        AnimationCurve animationCurve = transform.localPosition.y < Position.y ? acYUp : acYDown;
        if (transform.localPosition.z == 0 && transform.localPosition.x == 0)
        {
            transform.DOLocalMoveY(Position.y, time);
        }
        else
        {
            transform.DOLocalMoveY(Position.y, time).SetEase(animationCurve);
        }

        transform.DOLocalMoveX(Position.x, time);
        transform.DOLocalMoveZ(Position.z, time);

        transform.eulerAngles = new Vector3(Random.RandomRange(0, 360), Random.RandomRange(0, 360), Random.RandomRange(0, 360));
        transform.DOLocalRotate(Vector3.zero, time);
    }

    public void MoveToWorldPosition(Transform target, float time = 0.3f)
    {
        Vector3 position = target.position;
        if (transform.position == position)
        {
            return;
        }

        AnimationCurve animationCurve = transform.position.y < position.y ? acYUp : acYDown;

        transform.DOMoveX(position.x, time);
        transform.DOMoveY(position.y, time).SetEase(animationCurve);
        transform.DOMoveZ(position.z, time);

        transform.eulerAngles = new Vector3(Random.RandomRange(0, 360), Random.RandomRange(0, 360), Random.RandomRange(0, 360));
        transform.DOLocalRotate(Vector3.zero, time);
    }

    public void ToPosition()
    {
        transform.localPosition = Position;
        transform.eulerAngles = sortManager.transform.eulerAngles;
    }
}
