using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
public class customer : MonoBehaviour
{


   public string customerId; // Unique identifier for each custome
   public bool RoomAllotted = false;

   [SerializeField] Animation animator;
   [SerializeField] float SleepingTIme = 5f;
   [SerializeField] CustomerState currentState = CustomerState.waiting;

   private NavMeshAgent agent; // Reference to the NavMeshAgent component
   private RoomData roomData;
   private Transform roomDestination = null;


   [SerializeField]
   private float probExitingRoom = 0.9f; // Adjusted to half since there are only two states
   [SerializeField]
   private float probGoingToilet = 0.3f; // Adjusted to half since there are only two states
   [SerializeField]
   private float totalProbability = 1.0f; // Sum of the two probabilities


   #region Unity
   void Start()
   {
      ChangeState(CustomerState.waiting);
      // Initialize the NavMeshAgent
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animation>();

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

   #endregion

   #region Customer Behaviour logic
   private void MoveToRoom()
   {
      RoomAllotted = true;
      agent.destination = roomDestination.position;

   }


   private void Sleep()
   {
      roomData.room.SleepIn(gameObject);
      animator.AnimationPlay("Sleeping");
      animator.AnimationPlay("isWalking", false);
      animator.AnimationPlay("Idle", false);
      StartCoroutine(WaitAndTransition());
   }

   private IEnumerator WaitAndTransition()
   {
      yield return new WaitForSeconds(SleepingTIme); // Simulate sleeping time
                                                     //  ChangeState(CustomerState.GoingToilet);
      RandomizeState();
      roomData.room.SleepOver(gameObject);


   }
   private void ExitRoom()
   {

      agent.destination = DestinationManager.Instance.GetDestination("Exit").position;

   }
   private void GoToilet()
   {
      agent.destination = DestinationManager.Instance.GetDestination("Toilet").position;

   }

   private bool hasRandomizedState = false;
   public void RandomizeState()
   {
      if (hasRandomizedState)
      {
         // Prevent re-entry
         return;
      }

      // For example:
      float randomValue = UnityEngine.Random.value * totalProbability;

      if (randomValue <= probExitingRoom) ChangeState(CustomerState.ExitingRoom);
      else ChangeState(CustomerState.GoingToilet);

      // Mark as done
      hasRandomizedState = true;
   }


   public void ChangeState(CustomerState newState)
   {
      currentState = newState;
   }
   void RequestRoom()
   {
      EventManager.InvokeRoomRequested(gameObject);
   }

   #endregion
   public void AssginRoomData(RoomData _roomData)
   {

      roomData = _roomData;
   }


   public void AssginRoomDestination(Transform _roomDestination)
   {

      roomDestination = _roomDestination;
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
