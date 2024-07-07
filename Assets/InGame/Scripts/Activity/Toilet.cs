using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Toilet : MonoBehaviour
{

   public bool IsOccupied { get; private set; } = false;

   [SerializeField] float toiletTime = 10f;
   [SerializeField] customer customer;
   public void Occupy(customer c)
   {
      customer = c;
      IsOccupied = true;

   }

   public void Vacate()
   {
      if (customer == null) return;
      IsOccupied = false;
      customer.ChangeState(CustomerState.Exiting);
      customer = null;
   }


   void OnTriggerEnter(Collider collider)
   {

      if (collider.tag == "Customer")
      {

         TimerManager.Instance.ScheduleAction(toiletTime, Vacate);
      }
   }



}
