using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToiletPaperFactory : PlaceFactory
{
    public GameObject factoryObject;
    public float timeGenBox = 0.5f;

    protected IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeGenBox);
            if (sortSlot.SortObjects.Count < sortSlot.sortManagers.Count)
            {
                GenObject(1);
                // Debug.LogError("GENRATIOON");
            }
        }
    }

    protected virtual void GenObject(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(factoryObject);
            SortObject sortObject = g.GetComponent<SortObject>();
            sortSlot.AddObjectNotEffect(sortObject);
        }
    }
}
