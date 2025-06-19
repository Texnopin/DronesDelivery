using System.Collections.Generic;
using UnityEngine;

public class ResourceTracker : MonoBehaviour
{
    private static HashSet<GameObject> claimedResources = new HashSet<GameObject>();

    public static bool IsResourceClaimed(GameObject resource)
    {
        return claimedResources.Contains(resource);
    }

    public static void ClaimResource(GameObject resource)
    {
        claimedResources.Add(resource);
    }

    public static void ReleaseResource(GameObject resource)
    {
        claimedResources.Remove(resource);
    }
}
