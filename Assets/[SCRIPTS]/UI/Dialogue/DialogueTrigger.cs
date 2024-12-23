using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a script that is used for every instance of a conversation.
//Fill the lines[] array with DialogueLineScriptableObjects within the inspector
//This script will pass the lines to the dialogue manager when activated

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLineScriptableObject[] lines;
    FacialAnimator faceAnimator;
    bool isSpeaking;

    private void Start()
    {
        faceAnimator = GetComponentInChildren<FacialAnimator>();
        isSpeaking = false;

        // Directly subscribe to the event in Awake (since DialogueManager is always active)
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnDialogueIndexChanged += UpdateFaceAnimation;
        }
    }
    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnDialogueIndexChanged -= UpdateFaceAnimation;
        }
    }

    private void UpdateFaceAnimation(int currentIndex)
    {

        if (currentIndex >= 0 && currentIndex < lines.Length && isSpeaking)
        {
            faceAnimator.currentEyes = lines[currentIndex].eyes;
            faceAnimator.SetMouthRandom();
        }
        if (currentIndex == -1)
        {
            faceAnimator.currentEyes = faceAnimator.defaultEyes;
            faceAnimator.currentMouth = faceAnimator.defaultMouth;
        }
        if (currentIndex == -2)
        {
            faceAnimator.currentMouth = faceAnimator.defaultMouth;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SendDialogueToManager();
            isSpeaking = true;
            DialogueManager.instance.StartDialogue();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSpeaking = false;
            DialogueManager.instance.StopDialogue();
        }
    }

    void SendDialogueToManager()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.lines = null;
            DialogueManager.instance.lines = lines;
        }
    }

}
