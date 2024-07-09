using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSlot : MonoBehaviour
{
    public List<SortManager> sortManagers = new List<SortManager>();
    public List<SortObject> SortObjects
    {
        get
        {
            List<SortObject> sos = new List<SortObject>();
            foreach (var item in sortManagers)
            {
                sos.AddRange(item.sortObjects);
            }
            return sos;
        }
    }

    public int ObjectCount
    {
        get
        {
            return SortObjects.Count;
        }
    }

    public SortObject EndObject
    {
        get
        {
            List<SortObject> sos = SortObjects;

            if (sos.Count == 0)
            {
                return null;
            }

            sos.Sort((SortObject a, SortObject b) => b.Position.y.CompareTo(a.Position.y));
            return sos[0];
        }
    }

    public void AddObject(SortObject sortObject)
    {
        sortManagers.Sort((SortManager a, SortManager b) => a.Height.CompareTo(b.Height));
        sortManagers[0].AddObject(sortObject);
    }

    public void AddObjectNotEffect(SortObject sortObject)
    {
        sortManagers.Sort((SortManager a, SortManager b) => a.Height.CompareTo(b.Height));
        sortManagers[0].AddObjectNotEffect(sortObject);
    }

    public bool RemoveObject(SortObject sortObject)
    {
        foreach (var item in sortManagers)
        {
            if (item.sortObjects.IndexOf(sortObject) >= 0)
            {
                item.RemoveObject(sortObject);
                return true;
            }
        }

        return false;
    }

}
