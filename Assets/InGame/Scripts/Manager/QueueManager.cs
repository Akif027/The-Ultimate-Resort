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
            GameObject customerInstance = Instantiate(gameData.RandomCharacter.GetRandomModel(), CustomerStart.position, Quaternion.identity);
            NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
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
        // Debug.Log(customerQueue.Count);
        // if (Input.GetMouseButtonDown(0)) //temp
        // {
        //     if (customerQueue.Count > 0)
        //     {
        //         customerQueue.Dequeue(); // Remove the first customer from the queue
        //         NavMeshAgent agent = customersList[0].GetComponent<NavMeshAgent>();
        //         agent.SetDestination(tempo.position);
        //         customersList.RemoveAt(0); // Also remove from the list to maintain order
        //         currentCustomerIndex--; // Decrement the current customer index
        //         isQueueUpdated = true;
        //     }
        // }

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
        GameObject customerInstance = Instantiate(gameData.RandomCharacter.GetRandomModel(), CustomerStart.position, Quaternion.identity);
        NavMeshAgent agent = customerInstance.GetComponent<NavMeshAgent>();
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
                    // Assuming you have a method to determine the next spot for the customer
                    Vector3 nextSpot = DetermineNextSpot(i); // Implement this method based on your game logic
                    agent.SetDestination(nextSpot);

                }
                else
                {
                    Debug.LogError("Customer instance does not have a NavMeshAgent component.");
                }

            }

        }

    }
    Vector3 DetermineNextSpot(int currentCustomer)
    {


        // If it's the first customer, set the destination to ReceptionDesk
        if (currentCustomer == 0)
        {

            return ReceptionDesk.position;
        }
        else
        {

            GameObject frontCustomer = customersList[currentCustomer - 1];
            Vector3 currentCustomerPosition = frontCustomer.transform.position;
            Vector3 positionBehindRight = currentCustomerPosition + frontCustomer.transform.right * 2 - frontCustomer.transform.forward * Stoppingdistance;
            return positionBehindRight;
        }
    }
}
