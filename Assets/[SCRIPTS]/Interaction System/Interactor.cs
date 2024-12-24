using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 5f;
    private IInteractable currentInteractable;
    [SerializeField] GameObject marker;
    [SerializeField] float markerSpeed = 5f;
    private Vector3 markerDefaultPosition;
    private Vector3 markerTargetPosition;

    private void Start()
    {
        markerDefaultPosition = marker.transform.position;
    }

    private void Update()
    {
        if (PlayerController.instance.playerInteractLocked)
    {
        return; // Skip interaction logic if player interaction is locked
    }
        DetectInteractable();
        MoveMarker();

        if (currentInteractable != null && Input.GetButtonDown("A") && !PlayerController.instance.playerInteractLocked)
        {
            currentInteractable.Interact();
        }
    }


    //When Interactor is looking for an interactable:
    //It is looking for CAPSULECOLLIDERS only. This is to avoid 
    //interacting with an NPC's SphereCollider that tells it to look at the player.

    private void DetectInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(interactionPoint.position, interactionRadius);
        currentInteractable = null;

        foreach (var hit in hits)
        {
            if (hit is CapsuleCollider && hit.TryGetComponent<IInteractable>(out var interactable))
            {
                currentInteractable = interactable;


                markerTargetPosition = hit.transform.position;
                break; // Prioritize the first interactable found
            }
        }

        if(currentInteractable == null)
        {
            markerTargetPosition = markerDefaultPosition;
        }
    }

    private void MoveMarker()
    {
        marker.transform.position = Vector3.Lerp(marker.transform.position, markerTargetPosition, Time.deltaTime * markerSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
    }
}
