using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFactory : MonoBehaviour
{
    public SortSlot sortSlot;

    public float timeRate = 0.1f;
    protected float timeCount;
    public int slotMax = 50;

    public bool IsMax
    {
        get
        {
            return sortSlot.ObjectCount >= slotMax;
        }
    }

    public void Add(SortObject so)
    {
        if (IsMax)
        {
            Destroy(so.gameObject);
            return;
        }

        sortSlot.AddObject(so);
    }

    public void Put(HotelManager c)
    {
        SortObject sortObject = sortSlot.EndObject;

        if (sortObject != null)
        {
            if (c.AddObject(sortObject))
            {
                sortSlot.RemoveObject(sortObject);
            }
        }
    }

    private bool isCharacterInRange = false;
    private HotelManager characterInRange;

    protected void OnTriggerEnter(Collider other)
    {
        HotelManager character = other.GetComponent<HotelManager>();
        if (character != null)
        {
            isCharacterInRange = true;
            characterInRange = character;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HotelManager>() == characterInRange)
        {
            isCharacterInRange = false;
            characterInRange = null;
        }
    }

    void FixedUpdate()
    {
        if (isCharacterInRange)
        {
            timeCount += Time.fixedDeltaTime;
            if (timeCount >= timeRate)
            {
                Put(characterInRange);
                timeCount = 0; // Reset timer after action
            }
        }
    }
}
