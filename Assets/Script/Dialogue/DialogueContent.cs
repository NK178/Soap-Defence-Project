using UnityEngine;

[CreateAssetMenu(fileName = "DialogueContent", menuName = "Scriptable Objects/DialogueContent")]
public class DialogueContent : ScriptableObject
{
    public float textSpeed;
    [TextArea(1, 10)]
    public string text;
    public DIALOGUERESOLVE resolve; 
    public float pauseTime;
}
