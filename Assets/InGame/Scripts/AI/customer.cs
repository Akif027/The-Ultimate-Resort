using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class customer : MonoBehaviour
{
   public string customerId; // Unique identifier for each customer


   [SerializeField] bool RoomAllotted = false;
   [SerializeField] Animation animator;
   private NavMeshAgent agent; // Reference to the NavMeshAgent component
   public Transform roomDestination = null;
   public Transform toiletDestination = null;

   public CustomerState currentState = CustomerState.waiting;
   Rigidbody rb;

   void Start()
   {
      ChangeState(CustomerState.waiting);
      // Initialize the NavMeshAgent
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animation>();
      rb = GetComponent<Rigidbody>();
   }

   void Update()
   {

      if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance + 0.2)
      {

         // The customer is moving, play the walking animation
         animator.AnimationPlay("isWalking", true);
         animator.AnimationPlay("Idle", false);
      }
      else
      {

         animator.AnimationPlay("isWalking", false);
         animator.AnimationPlay("Idle", true);
         // The customer is idle or not moving significantly, stop the walking animation

      }

      switch (currentState)
      {

         case CustomerState.MovingToRoom:
            MoveToRoom();
            break;
         case CustomerState.Sleeping:
            Sleep();
            break;
         case CustomerState.ExitingRoom:
            ExitRoom();
            break;
         case CustomerState.GoingToilet:
            GoToilet();
            break;
      }
   }


   private void MoveToRoom()
   {
      RoomAllotted = true;
      agent.destination = roomDestination.position;


      // Transition to Sleeping state after reaching the room
      // StartCoroutine(WaitAndTransition());
   }

   private IEnumerator WaitAndTransition()
   {
      yield return new WaitForSeconds(1); // Simulate sleeping time
      ChangeState(CustomerState.ExitingRoom);
   }

   private void Sleep()
   {
      // animator.AnimationPlay("isWalking", false);
      // animator.AnimationPlay("Idle", true);
   }
   private void ExitRoom()
   {
      agent.destination = roomDestination.position + Vector3.up * 5; // Exit the room
                                                                     // Transition to GoingToilet state after exiting the room
      StartCoroutine(WaitAndTransition());
   }

   private void GoToilet()
   {
      agent.destination = toiletDestination.position;
      // Transition back to MovingToRoom after using the toilet
      StartCoroutine(WaitAndTransition());
   }

   public void ChangeState(CustomerState newState)
   {
      currentState = newState;
   }
   void RequestRoom()
   {
      EventManager.InvokeRoomRequested(gameObject);
   }



   void OnTriggerEnter(Collider collider)
   {
      if (collider.tag == "Reception")
      {
         RequestRoom();

         Debug.Log("room Requested");
      }

      if (collider.tag == "Bed")
      {
         ChangeState(CustomerState.Sleeping);
      }

   }


}
public enum CustomerState
{
   waiting,
   MovingToRoom,
   Sleeping,
   ExitingRoom,
   GoingToilet
}
