using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueLine", menuName = "Dialogue/Dialogue Line", order = 1)]
public class DialogueLineScriptableObject : ScriptableObject
{
    public string actorName; // The name of the speaker
    [TextArea(3, 10)] public string DialogueText; // The text of the dialogue
    public float textSpeed = 0.035f;

    public FacialAnimator.Eyes eyes;

}
