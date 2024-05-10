using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateResort;
public class HotelManager : GameManager
{

   public ReceptionManager receptionManager;
   bool canAllocateRoom = false;

   protected override void UpdateGame()
   {

      if (canAllocateRoom)
      {
         receptionManager.AllocateRoom();
         canAllocateRoom = false;
      }
   }

   protected override void OnTriggerEnter(Collider other)
   {

      if (other.tag == "ReceptionManager")
      {

         canAllocateRoom = true;
      }
   }

}
