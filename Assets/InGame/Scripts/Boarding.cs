using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UltimateResort;
public class Boarding : GameManager
{
    public static Boarding Instance { get; private set; }

    [SerializeField]
    GameObject customerPrefab;

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform[] queuePoints;

    [SerializeField] const int maxQueue = 4;
    [SerializeField] int queueFilled = 0;

    [SerializeField] float waitTime = 4;

    public List<GameObject> customerList = new List<GameObject>();

    protected override void Initialize()
    {
        //Singleton logic
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(customerGenerator());
    }

    public void CustomerDeparture()
    {
        if (queueFilled < 0) queueFilled--;
    }

    public void NextCustomer()
    {
        for (int i = 0; i < customerList.Count; i++)
        {
            if (customerList.Count > i + 1)
                customerList[i + 1].GetComponent<NavMeshAgent>().SetDestination(queuePoints[i].position);
        }
    }

    IEnumerator customerGenerator()
    {
        while (true)
        {
            if (queueFilled < maxQueue)
            {
                GameObject customer = Instantiate(customerPrefab, spawnPoint.position, quaternion.identity);
                customerList.Add(customer);
                customer.GetComponent<NavMeshAgent>().SetDestination(queuePoints[queueFilled].position);
                queueFilled++;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
