using System;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    public event Action OnResourceCollected;

    private Transform baseStation;
    private Material factionMaterial;
    private Renderer droneRenderer;

    public float collectionTime = 2f;

    private Transform targetResource;
    private NavMeshAgent agent;
    private bool carryingResource = false;

    private LineRenderer lineRenderer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        droneRenderer = GetComponentInChildren<Renderer>();
        lineRenderer = GetComponent<LineRenderer>();

        if (droneRenderer != null && factionMaterial != null)
        {
            droneRenderer.material = factionMaterial;
        }
    }

    public void Initialize(Transform assignedBase, Material assignedMaterial, float speed)
    {
        baseStation = assignedBase;
        factionMaterial = assignedMaterial;
        SetSpeed(speed);
    }

    public void SetSpeed(float speed)
    {
        if (agent != null)
        {
            agent.speed = speed;
        }
    }

    private void Update()
    {
        if (carryingResource && Vector3.Distance(transform.position, baseStation.position) < 1f)
        {
            DropResource();
        }
        else if (!carryingResource && targetResource == null)
        {
            FindNearestResource();
        }

        UpdatePathLine();
    }

    private void FindNearestResource()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
        float closestDistance = Mathf.Infinity;
        GameObject selectedResource = null;

        foreach (GameObject resource in resources)
        {
            if (ResourceTracker.IsResourceClaimed(resource)) continue;

            float distance = Vector3.Distance(transform.position, resource.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                selectedResource = resource;
            }
        }

        if (selectedResource != null)
        {
            targetResource = selectedResource.transform;
            ResourceTracker.ClaimResource(targetResource.gameObject);
            agent.SetDestination(targetResource.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource") && !carryingResource && other.gameObject == targetResource.gameObject)
        {
            StartCoroutine(CollectResource(other.gameObject));
        }
    }

    private System.Collections.IEnumerator CollectResource(GameObject resource)
    {
        yield return new WaitForSeconds(collectionTime);

        Destroy(resource);
        ResourceTracker.ReleaseResource(resource);
        carryingResource = true;

        OnResourceCollected?.Invoke(); // Вызываем событие сбора ресурса

        agent.SetDestination(baseStation.position);
    }

    private void DropResource()
    {
        carryingResource = false;
        targetResource = null;
        FindNearestResource();
    }

    private void UpdatePathLine()
    {
        if (agent == null || lineRenderer == null) return;

        NavMeshPath path = agent.path;
        if (path == null || path.corners.Length == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
    }
}
