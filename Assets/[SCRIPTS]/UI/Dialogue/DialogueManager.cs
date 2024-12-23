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

        if (Input.GetButtonDown("A"))
        {
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

    public void StartDialogue()
    {
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
}
