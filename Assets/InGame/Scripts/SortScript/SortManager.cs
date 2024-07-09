using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortManager : MonoBehaviour
{
    public List<SortObject> sortObjects = new List<SortObject>();

    public int ObjectCount
    {
        get
        {
            return sortObjects.Count;
        }
    }

    public float Height
    {
        get
        {
            float h = 0;
            foreach (var item in sortObjects)
            {
                h += item.height;
            }

            return h;
        }
    }

    public void AddObject(SortObject sortObject)
    {
        sortObjects.Add(sortObject);
        sortObject.transform.parent = transform;
        sortObject.Initialize(this);
        sortObject.MoveToPosition();
    }

    public void AddObjectNotEffect(SortObject sortObject)
    {
        sortObjects.Add(sortObject);
        sortObject.transform.parent = transform;
        sortObject.Initialize(this);
        sortObject.ToPosition();
    }

    public void RemoveObject(SortObject sortObject)
    {
        sortObjects.Remove(sortObject);
        ReMoveToPositions();
    }

    public void ReMoveToPositions()
    {
        foreach (var item in sortObjects)
        {
            item.MoveToPosition();
        }
    }

    public bool HasSortObject(SortObject sortObject)
    {
        return sortObjects.IndexOf(sortObject) >= 0;
    }
}