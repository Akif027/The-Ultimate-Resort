using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Toilet : MonoBehaviour
{

   public bool IsOccupied { get; private set; } = false;

   [SerializeField] float toiletTime = 10f;
   [SerializeField] customer customer;
   [SerializeField] bool needToiletPaper = false;
   public int countToiletUseage = 0;

   public float timeRate = 0.1f;
   public Transform point;
   protected float timeCount;


   void Start()
   {
      if (needToiletPaper)
      {
         UIManager.Instance.ShowUIElement(gameObject.name);

      }
      else
      {
         UIManager.Instance.HideUIElement(gameObject.name);
      }

   }
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
      countToiletUseage++;
      if (countToiletUseage > 2)
      {
         needToiletPaper = true;
         UIManager.Instance.ShowUIElement(gameObject.name);
      }
   }


   void OnTriggerEnter(Collider collider)
   {

      if (collider.tag == "Customer")
      {
         if (!IsOccupied) return;
         TimerManager.Instance.ScheduleAction(toiletTime, Vacate);
      }
      HotelManager character = collider.GetComponent<HotelManager>();

      if (character != null && needToiletPaper)
      {
         Get(character.sortSlot);
      }
   }


   public virtual void Get(SortSlot ss)
   {
      SortObject sortObject = ss.EndObject;

      if (sortObject != null)
      {
         ss.RemoveObject(sortObject);
         float time = 0.3f;
         sortObject.MoveToWorldPosition(point, time);
         Destroy(sortObject.gameObject, time);
         countToiletUseage = 0;
         needToiletPaper = false;
         UIManager.Instance.HideUIElement(gameObject.name);
      }
   }


}
