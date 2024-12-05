using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a CharacterController component
        CharacterController characterController = other.GetComponent<CharacterController>();

        // If a CharacterController component is found, log a message
        if (characterController != null)
        {
            Debug.Log("Player entered trigger");
        }
    }
}
