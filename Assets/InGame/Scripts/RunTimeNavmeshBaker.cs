using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RunTimeNavmeshBaker : MonoBehaviour
{
    public NavMeshBuildSettings settings; // Assign this in the inspector with your desired settings
    public List<NavMeshBuildSource> sources; // Define your sources, e.g., your environment geometry
    public Bounds bounds; // Define the bounds for NavMesh baking

    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshDataInstance;


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            BakeNavMesh();
        }

    }
    public void BakeNavMesh()
    {
        // Create new NavMeshData
        navMeshData = new NavMeshData();

        // Update the NavMeshData synchronously
        if (NavMeshBuilder.UpdateNavMeshData(
            navMeshData, // NavMeshData to update
            settings, // Build settings
            sources, // Sources for NavMesh build
            bounds)) // Bounds for NavMesh build
        {
            // Add NavMeshData to the scene
            navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData, transform.position, transform.rotation);
            Debug.Log("NavMesh baked successfully.");
        }
        else
        {
            Debug.LogError("NavMesh baking failed.");
        }
    }
}
