using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 5f;
    [SerializeField] private Transform marker;

    private readonly List<Collider> interactables = new List<Collider>();
    private Collider nearestInteractable;
    private Vector3 markerDefaultPosition;

    private void Start()
    {
        markerDefaultPosition = marker.position;
    }

    private void Update()
    {
        FindNearestInteractable();
        UpdateMarkerPosition();
    }

    private void FindNearestInteractable()
    {
        nearestInteractable = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var interactable in interactables)
        {
            if (interactable != null) // Ensure the interactable is still valid
            {
                float distanceSqr = (interactable.transform.position - interactionPoint.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    nearestInteractable = interactable;
                }
            }
        }
    }

    private void UpdateMarkerPosition()
    {
        if (nearestInteractable != null)
        {
            Vector3 targetPosition = nearestInteractable.transform.position;
            marker.position = Vector3.Lerp(marker.position, targetPosition, Time.deltaTime * 5f);
        }
        else
        {
            // Return the marker to its default position
            marker.position = Vector3.Lerp(marker.position, markerDefaultPosition, Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out _)) // Check if the object implements IInteractable
        {
            interactables.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactables.Remove(other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
