using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager: MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI actorNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueObject;
    [HideInInspector] public DialogueLineScriptableObject[] lines;

    private bool dialogueScrollIsReady = false;

    public int index; //tracking where we are in our conversation typing sequence

    // Event to notify listeners when the dialogue index changes
    public delegate void DialogueIndexChanged(int currentIndex);
    public event DialogueIndexChanged OnDialogueIndexChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (lines == null || lines.Length == 0) return;


        //for some reason, this if statment is going through even though playerInteractLocked is true. 
        //debugging looks like it is becoming true too late to stop this if statement.
        //if I can find a way to stop this if statment from firing during the first interact, then our dialogue system is almost done. 

        if (PlayerController.instance.playerInteractLocked && dialogueScrollIsReady)
        {
            if (Input.GetButtonDown("A"))
            {
                Debug.Log("Input received, playerInteractLocked: " + PlayerController.instance.playerInteractLocked);
                if (dialogueText.text == lines[index].DialogueText) //if the current line is done typing
                {
                    NextLine(); //next line
                }
                else //if the line is not done
                {
                    StopAllCoroutines(); //stop typing
                    dialogueText.text = lines[index].DialogueText; //set the line to the finished line
                }
            }
        }
    }

    public bool IsDialogueActive()
    {
        return dialogueObject.activeSelf;
    }

    public void StartDialogue()
    {
        StartCoroutine(SetInteractionLock());

        Debug.Log("Dialogue started. playerInteractLocked: " + PlayerController.instance.playerInteractLocked);
        dialogueObject.SetActive(true);
        dialogueText.text = string.Empty;
        index = 0;
        actorNameText.text = lines[index].actorName;

        OnDialogueIndexChanged?.Invoke(index); // Notify listeners that dialogue has started
        StartCoroutine(TypeLine());
    }

    public void StopDialogue()
    {
        StopAllCoroutines(); //stop typing

        if (lines != null && lines.Length > 0)
        {
            dialogueText.text = lines[index].DialogueText; //set the line to the finished line
        }
        
        dialogueObject.SetActive(false);
        index = 0;

        lines = null;

        OnDialogueIndexChanged?.Invoke(-1); // Notify listeners that dialogue has stopped
        PlayerController.instance.playerInteractLocked = false;
        dialogueScrollIsReady = false;
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].DialogueText.ToCharArray())
        {
            dialogueText.text += c;

            OnDialogueIndexChanged?.Invoke(index);

            AudioManager.instance.DialogueScroll();
            yield return new WaitForSeconds(lines[index].textSpeed);

            if (dialogueText.text == lines[index].DialogueText)
            {
                OnDialogueIndexChanged?.Invoke(-2); //Reset Only Mouth
            }

        }
    }

    void NextLine()
    {
        if (lines == null || index >= lines.Length - 1)
        {
            StopDialogue();
            return;
        }

        if (index < lines.Length - 1)
        {
            index++;
            actorNameText.text = lines[index].actorName;
            dialogueText.text = string.Empty;

            OnDialogueIndexChanged?.Invoke(index); // Notify listeners of index change
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator SetInteractionLock()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.instance.playerInteractLocked = true;
        dialogueScrollIsReady = true;
    }
}
