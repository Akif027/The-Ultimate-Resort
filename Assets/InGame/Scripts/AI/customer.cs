using System.Linq;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class customer : MonoBehaviour
{


   public string customerId; // Unique identifier for each custome
   public bool RoomAllotted = false;


   [SerializeField] Animation animator;
   [SerializeField] float Sleepingtime = 5f;
   [SerializeField] CustomerState currentState = CustomerState.waiting;

   private NavMeshAgent agent; // Reference to the NavMeshAgent component
   private RoomData roomData;
   private QueueManager queueManager;


   private Dictionary<CustomerState, float> stateProbabilities;
   [SerializeField] private List<StateProbability> stateProbabilitiesList;

   public Transform NextDestination = null;
   public Animation Animator
   {
      get { return animator; }
      private set { animator = value; }
   }

   #region Unity

   private void Awake()
   {
      // Initialize the dictionary from the list
      stateProbabilities = new Dictionary<CustomerState, float>();
      foreach (var sp in stateProbabilitiesList)
      {
         stateProbabilities[sp.state] = sp.probability;
      }
   }
   void Start()
   {
      queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
      ChangeState(CustomerState.waiting);
      // Initialize the NavMeshAgent
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animation>();


   }
   private void CustomerAnimationWalkAndIdle()
   {

      if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance + 0.1f)
      {
         if (currentState == CustomerState.GoingToilet)
         {


            animator.ChangeState(AnimationState.ToiletWalk);

         }
         else if (animator.CurrentState != AnimationState.Swim && animator.CurrentState != AnimationState.Sit)
         {

            animator.ChangeState(AnimationState.Walk);
         }


      }
      else
      {
         if (currentState == CustomerState.GoingToilet && animator.CurrentState != AnimationState.Swim)
         {

            animator.ChangeState(AnimationState.ToiletIdle);
         }
         else if (animator.CurrentState != AnimationState.Swim && animator.CurrentState != AnimationState.Sit)
         {

            //   animator.AnimationPlay("isWalking", false);
            animator.ChangeState(AnimationState.Idle);
         }




      }
   }
   void Update()
   {

      CustomerAnimationWalkAndIdle();
      switch (currentState)
      {

         case CustomerState.MovingToRoom:
            MoveToRoom();
            break;
         case CustomerState.Sleeping:
            Sleep();
            break;

         case CustomerState.GoingToilet:
            GoToilet();
            break;
         case CustomerState.SwimingPool:
            GoSwimming();
            break;
         case CustomerState.Exiting:
            ExitRoom();
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

      if (collider.tag == "ToBed")
      {
         ChangeState(CustomerState.Sleeping);
      }
      // if (collider.tag == "Toilet")
      // {
      //    RequestToilet();
      // }
   }
   void OnDisable()
   {
      // Reset the customer state to waiting
      currentState = CustomerState.waiting;

      // Reset RoomAllotted flag
      RoomAllotted = false;
      NextDestination = null;
      // Reset any other variables or flags as needed
   }
   #endregion

   #region Customer Behaviour logic
   private void MoveToRoom()
   {
      RoomAllotted = true;
      agent.destination = NextDestination.position;

   }

   private void Sleep()
   {
      roomData.room.SleepIn(gameObject);
      animator.ChangeState(AnimationState.Sleeping);
      TimerManager.Instance?.ScheduleAction(Sleepingtime, WakeUp);
   }

   private void WakeUp()
   {

      roomData.room.SleepOver(gameObject);
      // animator.AnimationPlay("Idle", true);
      // ChangeState(CustomerState.SwimingPool);
      // SetRandomCustomerState(CustomerState.MovingToRoom, CustomerState.waiting, CustomerState.GoingToilet, CustomerState.Exiting, CustomerState.Sleeping);
      //SetRandomCustomerState(CustomerState.MovingToRoom, CustomerState.waiting);
      SetRandomCustomerState(CustomerState.waiting, CustomerState.MovingToRoom, CustomerState.Sleeping);
   }
   private void ExitRoom()
   {

      agent.destination = DestinationManager.Instance.GetDestination("Exit").position;

   }
   private bool hasBeenAddedToToiletQueue = false;
   private void GoToilet()
   {
      if (!hasBeenAddedToToiletQueue)
      {
         queueManager.AddCustomerToToiletQueueAndList(gameObject);
         RequestToilet();
         hasBeenAddedToToiletQueue = true; // Set the flag to true after adding the customer to the queue
      }
   }
   private bool hasBeenAddedToPoolQueue = false;
   private void GoSwimming()
   {
      if (!hasBeenAddedToPoolQueue)
      {
         queueManager.AddCustomerToSwimmingPoolQueueAndList(gameObject);
         Requestpool();
         hasBeenAddedToPoolQueue = true; // Set the flag to true after adding the customer to the queue
      }
   }



   public void SetRandomCustomerState(params CustomerState[] excludeStates)
   {
      Array values = Enum.GetValues(typeof(CustomerState));
      var filteredValues = values.Cast<CustomerState>()
                                 .Where(state => !excludeStates.Contains(state))
                                 .ToArray();

      if (filteredValues.Length == 0)
      {
         Debug.LogError("No valid states available after exclusion.");
         return;
      }

      CustomerState randomState = GetStateByProbability(filteredValues);

      // Ensure the pool state is only set if the pool is upgraded
      if (randomState == CustomerState.SwimingPool && !UpgradeManager.Instance.IsPoolUpgraded())
      {
         SetRandomCustomerState(excludeStates.Append(CustomerState.SwimingPool).ToArray());
      }
      else
      {
         ChangeState(randomState);
      }

      Debug.Log("Random Customer State: " + currentState);
   }

   private CustomerState GetStateByProbability(CustomerState[] possibleStates)
   {
      float totalProbability = 0f;
      foreach (var state in possibleStates)
      {
         totalProbability += stateProbabilities[state];
      }

      float randomValue = UnityEngine.Random.value * totalProbability;
      float cumulativeProbability = 0f;

      foreach (var state in possibleStates)
      {
         cumulativeProbability += stateProbabilities[state];
         if (randomValue <= cumulativeProbability)
         {
            return state;
         }
      }

      return possibleStates[0]; // Fallback
   }

   public void ChangeState(CustomerState newState)
   {
      // Ensure the pool state is only set if the pool is upgraded
      if (newState == CustomerState.SwimingPool && !UpgradeManager.Instance.IsPoolUpgraded())
      {
         Debug.LogWarning("Cannot change to swimming pool state because the pool is not upgraded.");
         return;
      }

      currentState = newState;
   }
   void RequestRoom()
   {
      EventManager.InvokeRoomRequested(gameObject);
   }
   void RequestToilet()
   {
      EventManager.InvokeToiletRequested(this);
   }
   void Requestpool()
   {
      EventManager.InvokeSwimmingPoolRequested(this);
   }

   #endregion
   public void AssginRoomData(RoomData _roomData)
   {

      roomData = _roomData;
   }

   public void SetDestination()
   {

      agent.destination = NextDestination.position;
   }

   public void AssginDestination(Transform _destination)
   {

      NextDestination = _destination;
   }



}
public enum CustomerState
{
   waiting,
   MovingToRoom,
   Sleeping,
   Exiting,
   GoingToilet,
   SwimingPool,

}
[System.Serializable]
public class StateProbability
{
   public CustomerState state;
   public float probability;
}