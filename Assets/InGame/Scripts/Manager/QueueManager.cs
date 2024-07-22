using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class QueueManager : Manager
{
    public GameData gameData;

    // Use a Queue instead of a List for managing customers
    public Queue<GameObject> customerQueue = new Queue<GameObject>();
    public List<GameObject> customersList = new List<GameObject>();

    public Queue<GameObject> customerToiletQueue = new Queue<GameObject>();
    public List<GameObject> customersToiletList = new List<GameObject>();

    public Queue<GameObject> customerPoolQueue = new Queue<GameObject>();
    public List<GameObject> customersPoolList = new List<GameObject>();
    public Transform CustomerStart;
    public Transform ReceptionDesk;
    public int currentCustomerIndex = 0;
    [SerializeField] float Stoppingdistance = 3f;
    [SerializeField] bool queueStart = false;
    [SerializeField] bool isQueueUpdated = false; // not making it false

    public override void StartGame()
    {

        StartCoroutine(GenerateCustomersCoroutine(4));

    }

    IEnumerator GenerateCustomersCoroutine(int numberOfCustomers)
    {
        for (int i = 0; i < numberOfCustomers; i++)
        {
            GameObject customerInstance = ObjectPool.Instance.GetPooledObject("Customer");//Instantiate(gameData.RandomCharacter.GetRandomModel(), CustomerStart.position, Quaternion.identity);
            NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
            agent.Warp(CustomerStart.position);
            if (agent != null)
            {
                customersList.Add(customerInstance);
                isQueueUpdated = true;
                // Add the customer to the queue
                customerQueue.Enqueue(customerInstance);
                currentCustomerIndex++;

                // Optionally, wait for a short time before instantiating the next customer
                yield return new WaitForSeconds(3.5f); // Adjust delay as needed
            }
            else
            {
                Debug.LogError("Customer instance does not have a NavMeshAgent component.");
            }

        }
        queueStart = true;
    }

    public void AddCustomerToToiletQueueAndList(GameObject customer)
    {
        // Check if the customer is already in the list
        if (!customersToiletList.Contains(customer))
        {
            // Add the customer to the queue
            customerToiletQueue.Enqueue(customer);

            // Add the customer to the list
            customersToiletList.Add(customer);
        }
    }

    public void AddCustomerToSwimmingPoolQueueAndList(GameObject customer)
    {
        // Check if the customer is already in the list
        if (!customersPoolList.Contains(customer))
        {
            // Add the customer to the queue
            customerPoolQueue.Enqueue(customer);

            // Add the customer to the list
            customersPoolList.Add(customer);
        }
    }
    void Update()
    {
        if (customerQueue.Count < 4 && queueStart)
        {
            // Start the coroutine to generate a single customer
            StartCoroutine(GenerateSingleCustomer());

        }
        if (isQueueUpdated)
        {
            UpdateCustomerPositions();

        }
        if (customerToiletQueue.Count > 0)
        {
            UpdateToiletCustomerWaitingPositions();
        }
        if (customersPoolList.Count > 0)
        {
            UpdateSwimmingPoolCustomerWaitingPositions();
        }
    }
    public void RemoveCustomerFromToiletQueue(GameObject customerObj)
    {
        customerToiletQueue.Dequeue(); // Remove the first customer from the queue
        customersToiletList.Remove(customerObj); // Also remove from the list to maintain order

    }
    public void RemoveCustomerFromSwimmingPoolListAndQueue(GameObject customerObj)
    {
        customerPoolQueue.Dequeue(); // Remove the first customer from the queue
        customersPoolList.Remove(customerObj); // Also remove from the list to maintain order

    }
    public void RemoveCustomerFromQueue(GameObject customerObj)
    {
        customerQueue.Dequeue(); // Remove the first customer from the queue
        customersList.Remove(customerObj); // Also remove from the list to maintain order
        currentCustomerIndex--; // Decrement the current customer index
    }
    IEnumerator GenerateSingleCustomer()
    {
        // Generate a single customer
        GameObject customerInstance = ObjectPool.Instance.GetPooledObject("Customer");//Instantiate(gameData.RandomCharacter.GetRandomModel(), CustomerStart.position, Quaternion.identity);
        NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
        agent.Warp(CustomerStart.position);
        if (agent != null)
        {
            customersList.Add(customerInstance);
            isQueueUpdated = true;
            // Add the customer to the queue
            customerQueue.Enqueue(customerInstance);
            currentCustomerIndex++;

            // Optionally, wait for a short time before instantiating the next customer
            yield return new WaitForSeconds(3.5f); // Adjust delay as needed
        }
        else
        {
            Debug.LogError("Customer instance does not have a NavMeshAgent component.");
        }
    }
    void UpdateSwimmingPoolCustomerWaitingPositions()
    {
        if (customersPoolList.Count > 0)
        {
            for (int i = 0; i < customersPoolList.Count; i++)
            {
                GameObject customerInstance = customersPoolList[i];
                NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // Assuming you have a method to determine the next spot for the swimming pool customer
                    Vector3 nextSpot = DetermineNextSpot(i, DestinationManager.Instance.GetDestination("WaitingPoolPos"), customersPoolList); // Implement this method based on your game logic
                    agent.SetDestination(nextSpot);
                }
                else
                {
                    Debug.LogError("Swimming pool customer instance does not have a NavMeshAgent component.");
                }
            }
        }
    }
    void UpdateToiletCustomerWaitingPositions()
    {
        if (customersToiletList.Count > 0)
        {
            for (int i = 0; i < customersToiletList.Count; i++)
            {
                GameObject customerInstance = customersToiletList[i];
                NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // Assuming you have a method to determine the next spot for the toilet customer
                    Vector3 nextSpot = DetermineNextSpot(i, DestinationManager.Instance.GetDestination("Toilet"), customersToiletList); // Implement this method based on your game logic
                    agent.SetDestination(nextSpot);

                }
                else
                {
                    Debug.LogError("Toilet customer instance does not have a NavMeshAgent component.");
                }

            }
        }
    }

    void UpdateCustomerPositions()
    {
        if (customersList.Count > 0)
        {
            for (int i = 0; i < customersList.Count; i++)
            {
                GameObject customerInstance = customersList[i];
                NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // Determine the next spot for the customer
                    Vector3 nextSpot = DetermineNextSpot(i, DestinationManager.Instance.GetDestination("ReceptionDeskStop"), customersList);

                    agent.SetDestination(nextSpot);
                }
                else
                {
                    Debug.LogError("Customer instance does not have a NavMeshAgent component.");
                }
            }
        }
    }

    Vector3 DetermineNextSpot(int currentCustomerIndex, Transform StopPos, List<GameObject> customersInQueue)
    {
        // If it's the first customer, set the destination to ReceptionDesk
        if (currentCustomerIndex == 0)
        {

            return StopPos.position;
        }
        else
        {
            GameObject frontCustomer = customersInQueue[currentCustomerIndex - 1];
            Vector3 currentCustomerPosition = frontCustomer.transform.position;

            // Calculate the position directly behind the front customer
            Vector3 positionBehind = currentCustomerPosition - frontCustomer.transform.forward * Stoppingdistance;


            return positionBehind;
        }
    }

}
