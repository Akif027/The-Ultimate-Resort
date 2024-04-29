using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIGenerator : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform spawnPoint;
    public int maxCount = 4;
    public QueueManager queueManager;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1); // Initial delay before starting spawning

        while (true)
        {
            for (int i = 0; i < maxCount; i++)
            {
                NavMeshAgent newAgent = Instantiate(agent, spawnPoint.position, Quaternion.identity);
                newAgent.SetDestination(queueManager.getPoint().position);
                queueManager.availableAgents.Add(newAgent);
                yield return new WaitForSeconds(4); // Wait for 5 seconds before spawning the next agent
            }
            yield return new WaitForSeconds(8); // Wait for 15 seconds before starting a new wave of agents
        }
    }
}
