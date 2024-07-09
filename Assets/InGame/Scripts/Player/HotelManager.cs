using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelManager : MonoBehaviour
{
   public SortSlot sortSlot;

   public Transform parent;

   public int objectCountMax = 30;
   public bool IsObjectMax
   {
      get
      {
         return sortSlot.ObjectCount >= objectCountMax;
      }
   }


   public bool AddObject(SortObject sortObject)
   {
      if (sortSlot.ObjectCount >= objectCountMax)
      {
         return false;
      }

      sortSlot.AddObject(sortObject);

      return true;
   }

   void Start()
   {

      sortSlot.transform.parent = parent;
   }
}
